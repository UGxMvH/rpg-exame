using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(SpriteRenderer))]
public class HeroAnimations : MonoBehaviour
{
    #region Private Variables
    private new SpriteRenderer renderer;
    private PlayerManager player;
    public Sprite[] usingArray = null;
    public int currentIndex = 0;
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

    [Header("Arrow attack animations")]
    public Sprite[] attackArrowNorth;
    public Sprite[] attackArrowEast;
    public Sprite[] attackArrowSouth;
    public Sprite[] attackArrowWest;
    #endregion

    private void Start()
    {
        // Gather required components
        renderer = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerManager>();

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
        // Check if attacking
        if (player.isAttacking)
        {
            if (player.currentDirection == PlayerManager.Direction.North)
            {
                updateArray(attackArrowNorth);
                renderer.flipX = false;
            }
            else if (player.currentDirection == PlayerManager.Direction.South)
            {
                updateArray(attackArrowSouth);
                renderer.flipX = false;
            }
            else if (player.currentDirection == PlayerManager.Direction.East)
            {
                updateArray(attackArrowEast);
                renderer.flipX = false;
            }
            else if (player.currentDirection == PlayerManager.Direction.West)
            {
                // Check if there is a west sprite otherwise use flip
                if (idleWest)
                {
                    updateArray(attackArrowWest);
                    renderer.flipX = false;
                }
                else
                {
                    updateArray(attackArrowEast);
                    renderer.flipX = true;
                }
            }
        } else if (player.isIdle) {
            // Player is idle so get idle animation
            if (player.currentDirection == PlayerManager.Direction.North)
            {
                renderer.sprite = idleNorth;
                renderer.flipX = false;
            }
            else if (player.currentDirection == PlayerManager.Direction.South)
            {
                renderer.sprite = idleSouth;
                renderer.flipX = false;
            }
            else if (player.currentDirection == PlayerManager.Direction.East)
            {
                renderer.sprite = idleEast;
                renderer.flipX = false;
            }
            else if (player.currentDirection == PlayerManager.Direction.West)
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
            if (player.currentDirection == PlayerManager.Direction.North)
            {
                updateArray(walkNorth);
                renderer.flipX = false;
            }
            else if (player.currentDirection == PlayerManager.Direction.South)
            {
                updateArray(walkSouth);
                renderer.flipX = false;
            }
            else if (player.currentDirection == PlayerManager.Direction.East)
            {
                updateArray(walkEast);
                renderer.flipX = false;
            }
            else if (player.currentDirection == PlayerManager.Direction.West)
            {
                // Check if there is a west sprite otherwise use flip
                if (idleWest)
                {
                    updateArray(walkWest);
                    renderer.flipX = false;
                }
                else
                {
                    updateArray(walkEast);
                    renderer.flipX = true;
                }
            }
        }
    }

    private void updateArray(Sprite[] newArray)
    {
        if (newArray != usingArray)
        {
            currentIndex = 0;
        }

        usingArray = newArray;
    }

    private IEnumerator Animate()
    {
        while(true)
        {
            // Check if we have all conditions to know that we are running
            if ((!player.isIdle || player.isAttacking) && usingArray != null)
            {
                // Check if index is within range
                if (currentIndex >= usingArray.Length)
                {
                    currentIndex = 0;
                    
                    if (player.isAttacking)
                    {
                        player.isAttacking = false;
                        continue;
                    }
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
