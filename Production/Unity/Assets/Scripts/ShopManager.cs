using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject shopWindow;
    public Text error;
    public Text success;

    private Coroutine errorCo;
    private Coroutine successCo;

    private void Awake()
    {
        instance = this;
    }

    public void BuyHealthUpgrade()
    {
        if (BuyItem(11))
        {
            // Upgrade health
            CharacterManager.player.health += 5;
            CharacterManager.player.healthSlider.maxValue = CharacterManager.player.health;

            successCo = StartCoroutine(Success("Your max health had been increased to " + CharacterManager.player.health));
        }
    }

    public void BuyDamageUpgrade()
    {
        if (BuyItem(11))
        {
            // Upgrade damage
            CharacterManager.player.damage += 2;

            successCo = StartCoroutine(Success("Your arrow damage had been increased to " + CharacterManager.player.damage));
        }
    }

    public void BuyHealthPotion()
    {
        if (BuyItem(5))
        {
            // Add healthPotion
            CharacterManager.player.AddPotion();

            successCo = StartCoroutine(Success("You succesfully bought a health potion!"));
        }
    }

    public bool BuyItem(int price)
    {
        // Check if player has enoug money
        if (!CharacterManager.player.RemoveCoins(price))
        {

            // We don't have enough money
            // Hide success message if shown
            if (successCo != null)
            {
                StopCoroutine(successCo);
                success.gameObject.SetActive(false);
            }

            // Show message that we do not have anough money
            if (errorCo == null)
            {
                errorCo = StartCoroutine(Error());
            }

            // Deny sale
            return false;
        }

        // Check if not enough money message is showing
        if (errorCo != null)
        {
            StopCoroutine(errorCo);
            error.gameObject.SetActive(false);
        }

        // Allow sale
        return true;
    }

    private IEnumerator Error()
    {
        error.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(3);

        error.gameObject.SetActive(false);

        errorCo = null;
    }

    private IEnumerator Success(string text)
    {
        success.text = text;
        success.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(3);

        success.gameObject.SetActive(false);

        successCo = null;
    }
}
