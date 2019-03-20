using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : Interactable
{
    private bool isOpen;

    public override void OnInteract()
    {
        base.OnInteract();

        // Show shop and pause game
        ShopManager.instance.shopWindow.SetActive(true);
        Time.timeScale = 0;
        isOpen = true;
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Cancel") && isOpen)
        {
            Time.timeScale = 1;
            ShopManager.instance.shopWindow.SetActive(false);
         }
    }
}
