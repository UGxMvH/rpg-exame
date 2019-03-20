using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{
    private bool canInteract;

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

    public virtual void Update()
    {
        // If player pressed attack button and can interact
        if (Input.GetButtonDown("Attack") && canInteract)
        {
            // Fire OnInteract methode
            OnInteract();
        }
    }

    // What happens when we interact with this object (overridable)
    public virtual void OnInteract()
    {
        CharacterManager.player.interactMSG.SetActive(false);
    }

    #region collider functions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetInteractable(true, collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetInteractable(false, collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetInteractable(true, collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetInteractable(false, collision.gameObject);
    }
    #endregion
}
