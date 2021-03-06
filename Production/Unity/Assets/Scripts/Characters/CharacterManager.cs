﻿using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterAnimator))]
public class CharacterManager : MonoBehaviour
{
    #region Static Variables And Enums
    public static CharacterManager player;
    public enum Direction { North, East, South, West };
    #endregion

    #region Internal Variables
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
    #endregion

    #region Public Variables
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
    public Text potionUsageText;
    public CanvasGroup diedWindow;
    public GameObject interactMSG;
    public AudioClip dieSound;
    public AudioClip enemyAttackSound;
    #endregion

    #region Private Variables
    private Rigidbody2D body;
    private CharacterAnimator animator;
    private float lastVerticalInput;
    private float lastHorizontalInput;
    #endregion

    /*
     * Awake is called when the script instance is being loaded.
     * We use it to set a static refrence to the player.
     */
    private void Awake()
    {
        if (!isAI)
        {
            player = this;
        }
    }

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     * Also for enemies we choose a random location they start walking to.
     */
    private void Start()
    {
        // Gather components
        body        = GetComponent<Rigidbody2D>();
        animator    = GetComponent<CharacterAnimator>();

        // Set values from save game
        if (SaveGameManager.instance)
        {
            SaveGame sg     = SaveGameManager.instance.currentSaveGame;
            health          = sg.maxHealth;
            currentHealth   = sg.health;
            damage          = sg.damage;
            coins           = sg.coins;
            potions         = sg.potions;

            if (potionText)
            {
                potionText.text = potions + "x";
            }

            if (coinText)
            {
                coinText.text = coins.ToString("000");
            }
        }

        if (healthSlider)
        {
            // Set health slider values if there is a health slider
            healthSlider.minValue   = 0;
            healthSlider.maxValue   = health;
            healthSlider.value      = health;
        }

        if (potionUsageText && GameManager.instance.isUsingController)
        {
            potionUsageText.text = "Press \"RB\" to use";
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

    /*
     * Update is called each frame
     * We use it to process player input and AI behaviour.
     */
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

    /*
     * Player uses potion
     */
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

    /*
     * Update AI movement.
     * Set direction and movement input for the AI
     */
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

    /*
     * Keep track of the time a input is down for the player
     */
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

    /*
     * Asynchronous code to attack.
     * Wehenver the player attacks we need to wait till the animation is done and then fire the arrow.
     */
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

    /*
     * Enemy attack
     * Show enemy attack animation and fire bullets.
     * 2 Options which we choose a randomly attack. Diagonal or straight.
     */
    private IEnumerator EnemyAttack()
    {
        while (true)
        {
            // Wait for attack
            yield return new WaitForSeconds(Random.Range(.5f, 2f));

            // Attack animation
            yield return StartCoroutine(animator.EnemyAttack());

            // Play Attack sound
            AudioManager.instance.sfx.PlayOneShot(enemyAttackSound);

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

    /*
     * Update direction based on velocity or input
     */
    private void UpdateDirection()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

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

    /* 
     * Updates if the player is idle or not.
     */
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

    /* 
     * FixedUpdate is called every fixed frame-rate frame whenever the physics in Unity update.
     * We use it to set our velocity (movement)
     */
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

    /*
     * Character died.
     * Play dead animation and for AI's drop a coin
     * If player dies show died window
     */
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

            // play sound
            AudioManager.instance.sfx.PlayOneShot(dieSound);

            // Animate dead
            animator.Die();
        }
        else
        {
            // Player died
            Time.timeScale = 0;
            diedWindow.interactable     = true;
            diedWindow.blocksRaycasts   = true;
            diedWindow.gameObject.SetActive(true);
            diedWindow.GetComponent<RectTransform>().DOAnchorPosY(0, 1);
            diedWindow.DOFade(1, 1);

            if (GameManager.instance.isUsingController)
            {
                diedWindow.GetComponentInChildren<Button>().Select();
            }
        }
    }

    /*
     * Do damage to the AI or player
     */
    public void DoDamage(float damage)
    {
        // Don't do damage while transisting
        if (TransitionManager.instance && TransitionManager.instance.transistioning)
        {
            return;
        }

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

    /*
     * Add coin(s) to the player
     */
    public void AddCoin(int amount = 1)
    {
        coins += amount;

        if (coinText)
        {
            coinText.text = coins.ToString("000");
        }
    }

    /*
     * Remove coin(s) from the player
     */
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

    /*
     * Add potions to the player
     */
    public void AddPotion(int amount = 1)
    {
        potions += amount;

        if (potionText)
        {
            potionText.text = potions.ToString() + "x";
        }
    }
}
