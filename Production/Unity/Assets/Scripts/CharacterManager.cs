using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterAnimator))]
public class CharacterManager : MonoBehaviour
{
    public enum Direction { North, East, South, West };

    private Rigidbody2D body;
    private CharacterAnimator animator;
    private float lastVerticalInput;
    private float lastHorizontalInput;
    private float currentHealth;

    internal Direction currentDirection;
    internal float horizontal;
    internal float vertical;
    internal bool isIdle = true;
    internal bool isAttacking = false;
    internal bool isDead = false;

    public bool isAI;
    public LayerMask AiDetectLayer;
    public float runSpeed = 20.0f;
    public int health = 10;
    public UnityEngine.UI.Slider healthSlider;

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
            if (Input.GetButtonDown("Attack") && !isAttacking)
            {
                isAttacking = true;
                StartCoroutine(PlayerAttack());
            }

            // Gives a value between -1 and 1
            if (!isAttacking)
            {
                horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
                vertical = Input.GetAxisRaw("Vertical"); // -1 is down
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

    private void UpdateAIMovement()
    {
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

        if (currentDirection == Direction.South && (hit = Physics2D.CircleCast((Vector2)transform.position + new Vector2(0, -.7f), .2f, -transform.up, .1f, layerMask: AiDetectLayer)))
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

    private IEnumerator PlayerAttack()
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
            yield return new WaitForSeconds(3);

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
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(1, 1), Quaternion.Euler(0, 0, 45));
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(1, -1), Quaternion.Euler(0, 0, 135));
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-1, 1), Quaternion.Euler(0, 0, -45));
                PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-1, -1), Quaternion.Euler(0, 0, -135));
            }
        }
    }

    // Update direction
    private void UpdateDirection()
    {
        /*if (true)
        {*/
            // Get direction
            if (vertical > 0)
            {
                currentDirection = Direction.North;
            }
            else if (vertical < 0)
            {
                currentDirection = Direction.South;
            }
            else if (horizontal > 0)
            {
                currentDirection = Direction.East;
            }
            else if (horizontal < 0)
            {
                currentDirection = Direction.West;
            }
        /*}
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
        }*/
    }

    private void UpdateIdleState()
    {
        if (body.velocity.y == 0 && body.velocity.x == 0)
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

            // Animate dead
            animator.Die();

            // Tell room i'm dead
        }
    }

    public void DoDamage(float damage)
    {
        float newHealth = currentHealth - damage;

        if (newHealth <= 0)
        {
            // Dead
            if (healthSlider)
            {
                Dead();
            }

            return;
        }

        // Damage
        currentHealth = newHealth;

        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }

        // Show hit
        StartCoroutine(animator.GotHit());
    }
}
