using DG.Tweening;
using UnityEngine;

public class OpenShop : Interactable
{
    #region Private Variables
    private bool isOpen;
    #endregion

    /*
     * Called whenever the player is interacting with this GameObject
     */
    public override void OnInteract()
    {
        base.OnInteract();

        // Show shop and pause game
        ShopManager.instance.shopWindow.DOFade(1, .5f);
        ShopManager.instance.shopWindow.interactable    = true;
        ShopManager.instance.shopWindow.blocksRaycasts  = true;

        if (GameManager.instance.isUsingController)
        {
            ShopManager.instance.shopWindow.GetComponentInChildren<UnityEngine.UI.Button>().Select();
        }

        Time.timeScale = 0;
        isOpen = true;
    }

    /*
     * Update is called each frame.
     */
    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Cancel") && isOpen)
        {
            Time.timeScale = 1;
            ShopManager.instance.shopWindow.DOFade(0, .5f);
            ShopManager.instance.shopWindow.interactable    = false;
            ShopManager.instance.shopWindow.blocksRaycasts  = false;
        }
    }
}
