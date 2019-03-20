using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : Interactable
{
    private bool isOpen;

    public override void OnInteract()
    {
        // Show shop and pause game
        ShopManager.instance.shopWindow.SetActive(true);
        Time.timeScale = 0;
        isOpen = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Back") && isOpen)
        {
            Time.timeScale = 1;
            ShopManager.instance.shopWindow.SetActive(false);
            isOpen = false;
        }
    }
}
