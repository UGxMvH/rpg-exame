using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(SpriteRenderer))]
public class HeroAnimations : MonoBehaviour
{
    #region Private Variables
    private new SpriteRenderer renderer;
    private Movement movement;
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
        // Set correct sprite
        UpdateSprite();
    }

    // Animate
    private void UpdateSprite()
    {
        // Check if is idle
        if (movement.isIdle)
        {
            // Player is idle so get idle animation
            if (movement.currentDirection == Movement.Direction.North)
            {
                renderer.sprite = idleNorth;
                renderer.flipX = false;
            }
            else if (movement.currentDirection == Movement.Direction.South)
            {
                renderer.sprite = idleSouth;
                renderer.flipX = false;
            }
            else if (movement.currentDirection == Movement.Direction.East)
            {
                renderer.sprite = idleEast;
                renderer.flipX = false;
            }
            else if (movement.currentDirection == Movement.Direction.West)
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
            if (movement.currentDirection == Movement.Direction.North)
            {
                usingArray = walkNorth;
                renderer.flipX = false;
            }
            else if (movement.currentDirection == Movement.Direction.South)
            {
                usingArray = walkSouth;
                renderer.flipX = false;
            }
            else if (movement.currentDirection == Movement.Direction.East)
            {
                usingArray = walkEast;
                renderer.flipX = false;
            }
            else if (movement.currentDirection == Movement.Direction.West)
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
            if (!movement.isIdle && usingArray != null)
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
