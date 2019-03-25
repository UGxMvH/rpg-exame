using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Arrow : MonoBehaviour, PoolInterface
{
    private bool stuck;
    private new SpriteRenderer renderer;
    private Rigidbody2D rb;

    public float speed;
    public AudioClip[] hitSounds;
    public AudioClip[] shootSounds;

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
        AudioManager.instance.sfx.PlayOneShot(shootSounds[Random.Range(0, shootSounds.Length)]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!stuck)
        {
            if (collision.gameObject.tag == "Static")
            {
                AudioManager.instance.sfx.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
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
        }
    }

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
