using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterManager player = collision.gameObject.GetComponent<CharacterManager>();

        if (player)
        {
            if (!player.isAI)
            {
                player.AddCoin();
                gameObject.SetActive(false);
            }
        }
    }
}
