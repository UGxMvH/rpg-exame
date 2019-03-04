using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(SpriteRenderer))]
public class HeroAnimations : MonoBehaviour
{
    public enum Direction { North, East, South, West };

    #region Private Variables
    private new SpriteRenderer renderer;
    private Movement movement;
    private Direction currentDirection;
    private bool isIdle = true;
    private Sprite[] usingArray = null;
    private int currentIndex = 0;
    #endregion

    #region Public Variables
    [Header("Settings")]
    [Range(0.1f, 0.5f)]
    public float animationSpeed;

    [Header("Idle sprites")]
    public Sprite idleNorth;
    public Sprite idleEast;
    public Sprite idleSouth;
    public Sprite idleWest;

    [Header("Walk sprites")]
    public Sprite[] walkNorth;
    public Sprite[] walkEast;
    public Sprite[] walkSouth;
    public Sprite[] walkWest;
    #endregion

    private void Start()
    {
        // Gather required components
        renderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();

        // Start animation coroutine
        StartCoroutine(Animate());
    }

    #region Update methods
    // Update animations each frame.
    private void Update()
    {
        // Get idleState
        UpdateIdleState();

        // Get direction
        UpdateDirection();

        // Set correct sprite
        UpdateSprite();
    }

    private void UpdateIdleState()
    {
        if (movement.vertical == 0 && movement.horizontal == 0)
        {
            isIdle = true;
        }
        else
        {
            isIdle = false;
        }
    }

    // Update direction
    private void UpdateDirection()
    {
        // Get direction
        if (movement.vertical > 0)
        {
            currentDirection = Direction.North;
        }
        else if (movement.vertical < 0)
        {
            currentDirection = Direction.South;
        }
        else if (movement.horizontal > 0)
        {
            currentDirection = Direction.East;
        }
        else if (movement.horizontal < 0)
        {
            currentDirection = Direction.West;
        }
    }

    // Animate
    private void UpdateSprite()
    {
        // Check if is idle
        if (isIdle)
        {
            // Player is idle so get idle animation
            if (currentDirection == Direction.North)
            {
                renderer.sprite = idleNorth;
                renderer.flipX = false;
            }
            else if (currentDirection == Direction.South)
            {
                renderer.sprite = idleSouth;
                renderer.flipX = false;
            }
            else if (currentDirection == Direction.East)
            {
                renderer.sprite = idleEast;
                renderer.flipX = false;
            }
            else if (currentDirection == Direction.West)
            {
                // Check if there is a west sprite otherwise use flip
                if (idleWest)
                {
                    renderer.sprite = idleWest;
                    renderer.flipX = false;
                }
                else
                {
                    renderer.sprite = idleEast;
                    renderer.flipX = true;
                }
            }

        }
        else
        {
            // Get correct array
            if (currentDirection == Direction.North)
            {
                usingArray = walkNorth;
                renderer.flipX = false;
            }
            else if (currentDirection == Direction.South)
            {
                usingArray = walkSouth;
                renderer.flipX = false;
            }
            else if (currentDirection == Direction.East)
            {
                usingArray = walkEast;
                renderer.flipX = false;
            }
            else if (currentDirection == Direction.West)
            {
                // Check if there is a west sprite otherwise use flip
                if (idleWest)
                {
                    usingArray = walkWest;
                    renderer.flipX = false;
                }
                else
                {
                    usingArray = walkEast;
                    renderer.flipX = true;
                }
            }
        }
    }

    private IEnumerator Animate()
    {
        while(true)
        {
            // Check if we have all conditions to know that we are running
            if (!isIdle && usingArray != null)
            {
                // Check if index is within range
                if (currentIndex >= usingArray.Length)
                {
                    currentIndex = 0;
                }

                // Animate
                renderer.sprite = usingArray[currentIndex];

                // Count index up
                currentIndex++;
            }

            yield return new WaitForSeconds(animationSpeed);
        }
    }
    #endregion
}
