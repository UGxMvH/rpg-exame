using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class EnemyBullet : MonoBehaviour, PoolInterface
{
    public float speed;
    public float damage;

    // Start is called before the first frame update
    public void OnStart()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed);
    }

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
