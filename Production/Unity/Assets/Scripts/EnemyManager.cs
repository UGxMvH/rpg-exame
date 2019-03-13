using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyManager : MonoBehaviour
{
    private Rigidbody2D rb;

    public float movementSpeed = 200;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(transform.up * movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.isStatic)
        {
            // It's probally a wall
            Debug.Log("Whoops hit a wall!");
        }
    }
}
