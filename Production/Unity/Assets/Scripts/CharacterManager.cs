using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterAnimator))]
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager player;
    public enum Direction { North, East, South, West };

    private Rigidbody2D body;
    private CharacterAnimator animator;
    private float lastVerticalInput;
    private float lastHorizontalInput;

    internal Direction currentDirection;
    internal int coins;
    internal int potions;
    internal float horizontal;
    internal float vertical;
    internal float currentHealth;
    internal bool isIdle = true;
    internal bool isAttacking = false;
    internal bool isDead = false;
    internal bool usingMobile = false;
    internal Room AiRoom;

    [Header("Settings")]
    public bool overworld;
    public bool isAI;
    public LayerMask AiDetectLayer;
    public float runSpeed = 20.0f;
    public int health = 10;
    public int damage = 4;
    public Slider healthSlider;
    public Text coinText;
    public Text potionText;
    public CanvasGroup diedWindow;
    public GameObject interactMSG;

    private void Awake()
    {
        if (!isAI)
        {
            player = this;
        }
    }

    private void Start()
    {
        // Gather components
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<CharacterAnimator>();

        if (healthSlider)
        {
            // Set health slider values if there is a health slider
            healthSlider.minValue = 0;
            healthSlider.maxValue = health;
            healthSlider.value = health;
            currentHealth = health;
        }

        // Check if is AI
        if (isAI)
        {
            // Choose random direaction to walk to
            switch(Random.Range(0, 3))
            {
                case 0:
                    vertical = 1;
                    break;
                case 1:
                    horizontal = 1;
                    break;
                case 2:
                    vertical = - 1;
                    break;
                case 3:
                    horizontal = - 1;
                    break;
            }

            // Start attacking
            StartCoroutine(EnemyAttack());
        }
    }

    private void Update()
    {
        // Check if is AI
        if (isAI)
        {
            UpdateAIMovement();
        }
        else
        {
            if (!overworld && Input.GetButtonDown("Attack") && !isAttacking)
            {
                isAttacking = true;
                StartCoroutine(PlayerAttack());
            }

            if (!overworld && Input.GetButtonDown("Potion") && potions > 0)
            {
                UsePotion();
            }

            // Gives a value between -1 and 1
            if (!isAttacking)
            {
                if (!usingMobile)
                {
                    horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
                    vertical = Input.GetAxisRaw("Vertical"); // -1 is down
                }
            }
            else
            {
                horizontal = 0;
                vertical = 0;
            }

            UpdateLastInputTimes();
        }

        UpdateIdleState();
        UpdateDirection();
    }

    private void UsePotion()
    {
        // Minus 1 potion
        potions--;

        // Update potion text
        if (potionText)
        {
            potionText.text = potions.ToString() + "x";
        }

        // Calculate new health
        float newHealth = currentHealth + 10;

        // Make sure we do not exceed our max health
        if (newHealth > health)
        {
            newHealth = health;
        }

        // Update health
        currentHealth = newHealth;

        // Update health slider
        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void UpdateAIMovement()
    {
        // Don't do when enemy is dead
        if (isDead) return;

        RaycastHit2D hit;

        // Let's do some raycasts one needs to be filling in hit
        if (hit = Physics2D.CircleCast((Vector2)transform.position + new Vector2(0, .2f), .2f, transform.up, .1f, layerMask: AiDetectLayer))
        {
            if (currentDirection == Direction.North)
            {
                horizontal = 1;
                vertical = 0;
            }
        }

        if (currentDirection == Direction.East && (hit = Physics2D.CircleCast((Vector2)transform.position + new Vector2(.5f, -0.3f), .2f, transform.right, .2f, layerMask: AiDetectLayer)))
        {
            horizontal = 0;
            vertical = -1;
        }

        if (currentDirection == Direction.South && (hit = Physics2D.CircleCast((Vector2)transform.position + new Vector2(0, -.75f), .2f, -transform.up, .1f, layerMask: AiDetectLayer)))
        {
            horizontal = -1;
            vertical = 0;
        }

        if (currentDirection == Direction.West && (hit = Physics2D.CircleCast((Vector2)transform.position + new Vector2(-.5f, -0.3f), .2f, -transform.right, .2f, layerMask: AiDetectLayer)))
        {
            horizontal = 0;
            vertical = 1;
        }
    }

    private void UpdateLastInputTimes()
    {
        // Update horizontal time
        if (horizontal == 0)
        {
            lastHorizontalInput = 0;
        }
        else
        {
            if (lastHorizontalInput == 0)
            {
                lastHorizontalInput = Time.time;
            }
        }

        // Update vertical time
        if (vertical == 0)
        {
            lastVerticalInput = 0;
        }
        else
        {
            if (lastVerticalInput == 0)
            {
                lastVerticalInput = Time.time;
            }
        }
    }

    public IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(.3f);

        Vector2 pos = transform.position;
        Quaternion rot = Quaternion.identity;

        if (currentDirection == Direction.North)
        {
            pos += new Vector2(0, .5f);
        }

        if (currentDirection == Direction.East)
        {
            pos += new Vector2(.6f, -.15f);
            rot = Quaternion.Euler(0, 0, -90);
        }

        if (currentDirection == Direction.South)
        {
            pos += new Vector2(0, -.8f);
            rot = Quaternion.Euler(0, 0, 180);
        }

        if (currentDirection == Direction.West)
        {
            pos += new Vector2(-.6f, -.15f);
            rot = Quaternion.Euler(0, 0, 90);
        }

        PoolManager.instance.InstantiateObject("Arrow", pos, rot);
    }

    private IEnumerator EnemyAttack()
    {
        while (true)
        {
            // Wait for attack
            yield return new WaitForSeconds(Random.Range(.5f, 2f));

            // Attack animation
            yield return StartCoroutine(animator.EnemyAttack());

            // Attack 2 types
            if (Random.Range(0, 100) % 2 == 0)
            {
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(0, 1), Quaternion.identity);
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(1, 0), Quaternion.Euler(0, 0, -90));
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(0, -1), Quaternion.Euler(0, 0, 180));
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-1, 0), Quaternion.Euler(0, 0, 90));
            }
            else
            {
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-1, 1), Quaternion.Euler(0, 0, 45));
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-1, -1), Quaternion.Euler(0, 0, 135));
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(1, 1), Quaternion.Euler(0, 0, -45));
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(1, -1), Quaternion.Euler(0, 0, -135));
            }
        }
    }

    // Update direction
    private void UpdateDirection()
    {
        if (isAI || body.velocity == Vector2.zero)
        {
            // Get direction
            if (vertical == 1)
            {
                currentDirection = Direction.North;
            }
            else if (vertical == -1)
            {
                currentDirection = Direction.South;
            }
            else if (horizontal  == 1)
            {
                currentDirection = Direction.East;
            }
            else if (horizontal  == -1)
            {
                currentDirection = Direction.West;
            }
        }
        else
        {
            // Get direction
            if (body.velocity.y > 0)
            {
                currentDirection = Direction.North;
            }
            else if (body.velocity.y < 0)
            {
                currentDirection = Direction.South;
            }
            else if (body.velocity.x > 0)
            {
                currentDirection = Direction.East;
            }
            else if (body.velocity.x < 0)
            {
                currentDirection = Direction.West;
            }
        }
    }

    private void UpdateIdleState()
    {
        if (body.velocity == Vector2.zero)
        {
            isIdle = true;
        }
        else
        {
            isIdle = false;
        }
    }

    private void FixedUpdate()
    {
        float movementX = horizontal;
        float movementY = vertical;

        // Block diognal movement
        if (movementX != 0 && movementY != 0 && !isAI)
        {
            if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].Contains("Xbox"))
            {
                if (Mathf.Abs(movementX) >= Mathf.Abs(movementY))
                {
                    movementY = 0;
                }
                else
                {
                    movementX = 0;
                }
            }
            else
            {
                // Check which key is pressed the latest
                if (lastVerticalInput >= lastHorizontalInput)
                {
                    movementX = 0;
                }
                else
                {
                    movementY = 0;
                }
            }
        }

        body.velocity = new Vector2(movementX * runSpeed, movementY * runSpeed);
    }

    private void Dead()
    {
        if (isAI)
        {
            // Dead
            isDead = true;

            // Hide healthbar
            healthSlider.gameObject.SetActive(false);

            // Stop movement
            horizontal = 0;
            vertical = 0;
            body.velocity = Vector2.zero;

            // Tell room i'm dead
            AiRoom.EnemyDied(this);

            // Drop coin
            if (Random.Range(0, 100) > 25)
            {
                PoolManager.instance.InstantiateObject("Coin", transform.position, Quaternion.identity, LevelManager.instace.currentRoom.transform);
            }

            // Animate dead
            animator.Die();
        }
        else
        {
            // Player died
            Time.timeScale = 0;
            diedWindow.gameObject.SetActive(true);
            diedWindow.GetComponent<RectTransform>().DOAnchorPosY(0, 1);
            diedWindow.DOFade(1, 1);
        }
    }

    public void DoDamage(float damage)
    {
        float newHealth = currentHealth - damage;

        if (newHealth <= 0)
        {
            // Health = 0
            newHealth = 0;

            // Dead
            Dead();
        }

        // Damage
        currentHealth = newHealth;

        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }

        // Show hit
        if (currentHealth != 0)
        {
            StartCoroutine(animator.GotHit());
        }
    }

    public void AddCoin(int amount = 1)
    {
        coins += amount;

        if (coinText)
        {
            coinText.text = coins.ToString("000");
        }
    }

    public bool RemoveCoins(int price)
    {
        if (coins - price < 0)
        {
            return false;
        }

        coins -= price;

        if (coinText)
        {
            coinText.text = coins.ToString("000");
        }

        return true;
    }

    public void AddPotion(int amount = 1)
    {
        potions += amount;

        if (potionText)
        {
            potionText.text = potions.ToString() + "x";
        }
    }
}
