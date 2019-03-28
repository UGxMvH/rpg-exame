using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterManager))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterAnimator : MonoBehaviour
{
    #region Public Variables
    [Header("Settings")]
    [Range(0.1f, 0.5f)]
    public float animationSpeed;
    public Color hitColor;

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

    [Header("Dead sprites")]
    public Sprite[] dieSprites;
    #endregion

    #region Private Variables
    private new SpriteRenderer renderer;
    private CharacterManager character;
    private Sprite[] usingArray = null;
    private int currentIndex    = 0;
    private Color normalColor;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to get the required components.
     * And we set some variables and start the animation coroutine.
     */
    private void Start()
    {
        // Gather required components
        renderer    = GetComponent<SpriteRenderer>();
        character   = GetComponent<CharacterManager>();

        // Gather default variables
        normalColor = renderer.material.color;

        // Start animation coroutine
        StartCoroutine(Animate());
    }
    
    /*
     * Update is called each frame.
     * Here we update the sprite each frame
     */
    private void Update()
    {
        // Set correct sprite
        UpdateSprite();
    }

    /*
     * Updating the sprite so we have a smooth animation
     */
    private void UpdateSprite()
    {
        // Check if not dead
        if (character.isDead)
        {
            return;
        }

        // Check if attacking
        if (character.isAttacking)
        {
            if (character.currentDirection == CharacterManager.Direction.North)
            {
                updateArray(attackArrowNorth);
                renderer.flipX = false;
            }
            else if (character.currentDirection == CharacterManager.Direction.South)
            {
                updateArray(attackArrowSouth);
                renderer.flipX = false;
            }
            else if (character.currentDirection == CharacterManager.Direction.East)
            {
                updateArray(attackArrowEast);
                renderer.flipX = false;
            }
            else if (character.currentDirection == CharacterManager.Direction.West)
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
        } else if (character.isIdle) {
            // character is idle so get idle animation
            if (character.currentDirection == CharacterManager.Direction.North)
            {
                renderer.sprite = idleNorth;
                renderer.flipX = false;
            }
            else if (character.currentDirection == CharacterManager.Direction.South)
            {
                renderer.sprite = idleSouth;
                renderer.flipX = false;
            }
            else if (character.currentDirection == CharacterManager.Direction.East)
            {
                renderer.sprite = idleEast;
                renderer.flipX = false;
            }
            else if (character.currentDirection == CharacterManager.Direction.West)
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
            if (character.currentDirection == CharacterManager.Direction.North)
            {
                updateArray(walkNorth);
                renderer.flipX = false;
            }
            else if (character.currentDirection == CharacterManager.Direction.South)
            {
                updateArray(walkSouth);
                renderer.flipX = false;
            }
            else if (character.currentDirection == CharacterManager.Direction.East)
            {
                updateArray(walkEast);
                renderer.flipX = false;
            }
            else if (character.currentDirection == CharacterManager.Direction.West)
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

    /*
     * Update the animation array.
     * Repalce sprite array if needed and start animation from the beginning
     * @var Sprite[] sprite array
     */
    private void updateArray(Sprite[] newArray)
    {
        // Check if array is diffrent
        if (newArray != usingArray)
        {
            // Start from beginning
            currentIndex = 0;
        }

        // Use new array
        usingArray = newArray;
    }

    /*
     * Asynchornus piece of code that runs in the background.
     * We aniamte the sprites here and replace the sprite over time
     */
    private IEnumerator Animate()
    {
        while(true)
        {
            // Check if we have all conditions to know that we are running
            if ((!character.isIdle || character.isAttacking || character.isDead) && usingArray != null)
            {
                // Check if index is within range
                if (currentIndex >= usingArray.Length)
                {
                    // Check if is A.I. That died
                    if (character.isAI && character.isDead)
                    {
                        gameObject.SetActive(false);
                    }

                    // Reset index
                    currentIndex = 0;
                    
                    // Reset attacking
                    if (character.isAttacking)
                    {
                        character.isAttacking = false;
                        continue;
                    }
                }

                // Animate
                if (usingArray != null)
                {
                    if (usingArray.Length > 0)
                    {
                        renderer.sprite = usingArray[currentIndex];
                    }
                }

                // Count index up
                currentIndex++;
            }

            yield return new WaitForSeconds(animationSpeed);
        }
    }

    /*
     * The character has died.
     * Start playing die animation
     */
    public void Die()
    {
        // Update using array
        updateArray(dieSprites);
    }

    /*
     * The character got hit.
     * Show hit animation
     */
    public IEnumerator GotHit()
    {
        // Change color to hit
        renderer.color = hitColor;

        // Keep this color a little while
        yield return new WaitForSeconds(.25f);

        // Change color back
        renderer.color = normalColor;
    }

    /*
     * Show attack animation for enemy
     */
    public IEnumerator EnemyAttack()
    {
        renderer.color = Color.white;
        
        yield return new WaitForSeconds(.5f);

        renderer.color = normalColor;

        yield return new WaitForSeconds(.5f);

        renderer.color = Color.white;

        yield return new WaitForSeconds(.5f);

        renderer.color = normalColor;

        yield return new WaitForSeconds(.5f);

        renderer.color = Color.white;

        yield return new WaitForSeconds(.5f);

        renderer.color = normalColor;

        yield return new WaitForSeconds(.15f);

        renderer.color = Color.white;

        yield return new WaitForSeconds(.15f);

        renderer.color = normalColor;

        yield return new WaitForSeconds(.15f);

        renderer.color = Color.white;

        yield return new WaitForSeconds(.15f);

        renderer.color = normalColor;

        yield return new WaitForSeconds(.15f);

        renderer.color = Color.white;

        yield return new WaitForSeconds(.15f);

        renderer.color = normalColor;
    }
}
