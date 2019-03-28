using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyBullet : MonoBehaviour, PoolInterface
{
    #region Public Variables
    public float speed;
    public float damage;
    #endregion

    #region Private Variables
    private Rigidbody2D rb;
    #endregion

    /*
     * OnStart is a interface function that is called whenever this GameObject is used by the PoolManager.
     */
    public void OnStart()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * speed);
    }

    /*
     * OnTriggerEnter2D is called whenever a object enters the collider of this GameObject.
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CharacterManager character = collision.gameObject.GetComponent<CharacterManager>();

        // Check if we have a character
        if (character)
        {
            // Do damage if not AI
            if (!character.isAI)
            {
                character.DoDamage(damage);
            }
        }

        gameObject.SetActive(false);
    }
}
