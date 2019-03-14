using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterManager : MonoBehaviour
{
    public enum Direction { North, East, South, West };

    private Rigidbody2D body;
    private float lastVerticalInput;
    private float lastHorizontalInput;
    public float currentHealth;

    internal Direction currentDirection;
    internal float horizontal;
    internal float vertical;
    internal bool isIdle = true;
    internal bool isAttacking = false;

    public bool isAI;
    public float runSpeed = 20.0f;
    public int health = 10;
    public UnityEngine.UI.Slider healthSlider;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();

        if (healthSlider)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = health;
            healthSlider.value = health;
            currentHealth = health;
        }

        if (isAI)
        {
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
        }
    }

    private void Update()
    {
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
        if (hit = Physics2D.CircleCast((Vector2)transform.position + new Vector2(0, .2f), .2f, transform.up, .1f))
        {
            if (currentDirection == Direction.North)
            {
                horizontal = 1;
                vertical = 0;
            }
        }

        if (currentDirection == Direction.East && (hit = Physics2D.CircleCast((Vector2)transform.position + new Vector2(.5f, -0.3f), .2f, transform.right, .2f)))
        {
            horizontal = 0;
            vertical = -1;
        }

        if (currentDirection == Direction.South && (hit = Physics2D.CircleCast((Vector2)transform.position + new Vector2(0, -.7f), .2f, -transform.up, .1f)))
        {
            horizontal = -1;
            vertical = 0;
        }

        if (currentDirection == Direction.West && (hit = Physics2D.CircleCast((Vector2)transform.position + new Vector2(-.5f, -0.3f), .2f, -transform.right, .2f)))
        {
            Debug.Log(hit.transform.name);
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

    // Update direction
    private void UpdateDirection()
    {
        if (isAI)
        {
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

    public void DoDamage(float damage)
    {
        float newHealth = currentHealth - damage;

        if (newHealth <= 0)
        {
            // Dead

            return;
        }

        // Damage
        currentHealth = newHealth;

        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }
    }
}
