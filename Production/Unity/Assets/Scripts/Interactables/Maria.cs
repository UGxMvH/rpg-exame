using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maria : Interactable
{
    public override void OnInteract()
    {
        base.OnInteract();

        // Remove text balloon
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        // Tell storyline
    }
}
