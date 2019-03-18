using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EasyAnimate))]
public class FlyEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private EasyAnimate animator;
    private bool isDead;

    public Transform target;
    public float range;
    public float speed;
    public float damage;
    public Sprite[] left;
    public Sprite[] right;
    public Sprite[] dead;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<EasyAnimate>();
        target = CharacterManager.player.transform;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (Vector2.Distance(transform.position, target.position) <= range)
        {
            Vector2 v2 = target.position - transform.position;
            float angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;

            Debug.Log(angle);

            rb.velocity = DegreeToVector2(angle);

            if (rb.velocity.x > 0)
            {
                animator.sprites = right;
            }
            else
            {
                animator.sprites = left;
            }
        }
        else
        {
            // If can't see player stop moving
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead || TransitionManager.instance.transistioning)
        {
            return;
        }

        if (collision.GetComponent<Arrow>())
        {
            StartCoroutine(Die());
            return;
        }

        CharacterManager character = collision.gameObject.GetComponent<CharacterManager>();

        if (character && !character.isAI)
        {
            character.DoDamage(damage);
            StartCoroutine(Die());
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

    private IEnumerator Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;

        // Drop coin
        if (Random.Range(0, 100) > 25)
        {
            PoolManager.instance.InstantiateObject("Coin", transform.position, Quaternion.identity, LevelManager.instace.currentRoom.transform);
        }

        animator.sprites = dead;

        yield return new WaitForSeconds(animator.speed * dead.Length);

        gameObject.SetActive(false);
    }
}
