using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{
    #region Private Variables
    private bool canInteract;
    #endregion

    /*
     * Set interactable state
     * @var bool newValue (Is interactable?)
     * @var GameObject go (Which GameObject is colliding)
     */
    private void SetInteractable(bool newValue, GameObject go)
    {
        // Check if player collided
        CharacterManager player = go.GetComponent<CharacterManager>();

        if (player && !player.isAI)
        {
            // Change canInteract bool
            canInteract = newValue;

            // Hide/Show interact text
            if (canInteract)
            {
                player.interactMSG.SetActive(true);
            }
            else
            {
                player.interactMSG.SetActive(false);
            }
        }
    }

    /*
     * Called every frame.
     */
    public virtual void Update()
    {
        // If player pressed attack button and can interact
        if (Input.GetButtonDown("Attack") && canInteract)
        {
            // Fire OnInteract methode
            OnInteract();
        }
    }

    /*
     * Overridable
     * Called whenever a player is interacting with this GameObject.
     */
    public virtual void OnInteract()
    {
        CharacterManager.player.interactMSG.SetActive(false);
    }

    #region collider functions
    /*
     * Called whenever a GameObject is colliding with this GameObject
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetInteractable(true, collision.gameObject);
    }

    /*
     * Called whenever a GameObject stops colliding with this GameObject
     */
    private void OnCollisionExit2D(Collision2D collision)
    {
        SetInteractable(false, collision.gameObject);
    }

    /*
     * Called whenever a GameObject is entering the trigger area of this collider
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetInteractable(true, collision.gameObject);
    }

    /*
     * Called whenever a GameObject is leaving the trigger area of this collider
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        SetInteractable(false, collision.gameObject);
    }
    #endregion
}
