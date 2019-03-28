using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Coin : MonoBehaviour
{
    #region Public Variables
    public AudioClip soundEffect;
    #endregion

    /*
     * OnTriggerEnter2D is called whenever a object enters the collider of this GameObject.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterManager player = collision.gameObject.GetComponent<CharacterManager>();

        if (player)
        {
            if (!player.isAI)
            {
                player.AddCoin();
                gameObject.SetActive(false);

                // Play sound effect
                AudioManager.instance.sfx.PlayOneShot(soundEffect);
            }
        }
    }
}
