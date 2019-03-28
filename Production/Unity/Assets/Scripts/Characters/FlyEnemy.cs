using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EasyAnimate))]
public class FlyEnemy : MonoBehaviour
{
    #region Public Variables
    public Transform target;
    public float range;
    public float speed;
    public float damage;
    public Sprite[] left;
    public Sprite[] right;
    public Sprite[] dead;
    public AudioClip dieSound;
    #endregion

    #region Private Variables
    private Rigidbody2D rb;
    private EasyAnimate animator;
    private bool isDead;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
    private void Start()
    {
        rb          = GetComponent<Rigidbody2D>();
        animator    = GetComponent<EasyAnimate>();

        target      = CharacterManager.player.transform;
    }

    /* 
     * FixedUpdate is called every fixed frame-rate frame whenever the physics in Unity update.
     * We use it to set our velocity (movement).
     * Check if player is close enough if so than follow.
     * Also datermine which animation we need to run.
     */
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

    /*
     * OnTriggerEnter2D is called whenever a object enters the collider of this GameObject.
     */
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

    /*
     * Change radion into a Vector2.
     * @var float radian
     * returns Vector2
     */
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    /* 
     * Change degree in a Vector2
     * @var float degree
     * returns Vector2
     */
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    /*
     * Asynchronous code
     * The fly enemy died
     * Plays sound and drops coin with 75% rate.
     */
    private IEnumerator Die()
    {
        // Play dead sound
        AudioManager.instance.sfx.PlayOneShot(dieSound);

        isDead = true;
        rb.velocity = Vector2.zero;

        // Drop coin
        if (Random.Range(0, 100) > 25)
        {
            PoolManager.instance.InstantiateObject("Coin", transform.position, Quaternion.identity, LevelManager.instace.currentRoom.transform);
        }

        animator.sprites = dead;

        yield return new WaitForSeconds(animator.speed * (dead.Length - 1));

        gameObject.SetActive(false);
    }
}
