using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Arrow : MonoBehaviour, PoolInterface
{
    #region Public Variables
    public float speed;
    public AudioClip[] hitSounds;
    public AudioClip[] shootSounds;
    #endregion

    #region Private Variables
    private bool stuck;
    private new SpriteRenderer renderer;
    private Rigidbody2D rb;
    #endregion

    /*
     * OnStart is a interface function that is called whenever this GameObject is used by the PoolManager.
     */
    public void OnStart()
    {
        if (!renderer)
        {
            renderer = GetComponent<SpriteRenderer>();
        }
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        rb.velocity = Vector2.zero;
        stuck = false;
        renderer.enabled = true;
        rb.AddForce(transform.up * speed);

        // Play shoot sound
        if (AudioManager.instance)
        {
            AudioManager.instance.sfx.PlayOneShot(shootSounds[Random.Range(0, shootSounds.Length)]);
        }
    }

    /*
     * OnTriggerEnter2D is called whenever a object enters the collider of this GameObject.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!stuck)
        {
            if (collision.gameObject.tag == "Static")
            {
                if (AudioManager.instance)
                {
                    AudioManager.instance.sfx.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
                }

                stuck = true;
                StartCoroutine(Dissapear());
                rb.velocity = Vector2.zero;
                return;
            }

            CharacterManager character = collision.gameObject.GetComponent<CharacterManager>();

            if (character)
            {
                // Damage character
                character.DoDamage(CharacterManager.player.damage);
            }

            Boss boss = collision.gameObject.GetComponent<Boss>();

            if (boss)
            {
                // Damage boss
                boss.DoDamage(CharacterManager.player.damage);
            }
        }
    }

    /*
     * Asynchronous code
     * Called whenever the arrow needs to disapear
     */
    private IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(1);

        renderer.enabled = false;

        yield return new WaitForSeconds(.5f);

        renderer.enabled = true;

        yield return new WaitForSeconds(.5f);

        renderer.enabled = false;

        yield return new WaitForSeconds(.5f);

        renderer.enabled = true;

        yield return new WaitForSeconds(.5f);

        renderer.enabled = false;

        gameObject.SetActive(false);
    }
}
