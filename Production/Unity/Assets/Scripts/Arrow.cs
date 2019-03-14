using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Arrow : MonoBehaviour, PoolInterface
{
    public float speed;
    public float damage;

    public void OnStart()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CharacterManager character = collision.gameObject.GetComponent<CharacterManager>();

        if (character)
        {
            // Damage character
            Debug.Log("Do damage");
            character.DoDamage(damage);
        }

        // Bye bye arrow
        gameObject.SetActive(false);
    }
}
