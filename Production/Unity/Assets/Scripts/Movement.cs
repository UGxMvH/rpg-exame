using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public enum Direction { North, East, South, West };

    private Rigidbody2D body;
    private float lastVerticalInput;
    private float lastHorizontalInput;

    internal Direction currentDirection;
    internal float horizontal;
    internal float vertical;
    internal bool isIdle = true;

    public float runSpeed = 20.0f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        UpdateLastInputTimes();
        UpdateIdleState();
        UpdateDirection();
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

    // Update direction
    private void UpdateDirection()
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

    void FixedUpdate()
    {
        float movementX = horizontal;
        float movementY = vertical;

        // Block diognal movement
        if (movementX != 0 && movementY != 0)
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
}
