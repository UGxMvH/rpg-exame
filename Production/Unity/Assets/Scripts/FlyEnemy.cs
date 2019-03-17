using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class FlyEnemy : MonoBehaviour
{
    private Rigidbody2D rb;

    public Transform target;
    public float range;
    public float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, target.position) <= range)
        {
            Vector2 v2 = target.position - transform.position;
            float angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;

            Debug.Log(angle);

            rb.velocity = DegreeToVector2(angle);
        }
        else
        {
            // If can't see player stop moving
            rb.velocity = Vector2.zero;
        }
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
}
