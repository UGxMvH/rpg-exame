using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class SpikeTrap : MonoBehaviour
{
    private Coroutine co;
    private new SpriteRenderer renderer;
    private int currentIndex;
    private bool canDamage = true;
    private bool spikesOut = false;

    public Sprite[] sprites;
    public float speed = 0.1f;
    public float delay = 2;
    public float damage = 3;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        co = StartCoroutine(Animate());
    }

    private void OnDisable()
    {
        StopCoroutine(Animate());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (spikesOut && canDamage)
        {
            CharacterManager player = collision.gameObject.GetComponent<CharacterManager>();

            if (player)
            {
                player.DoDamage(damage);
                StartCoroutine(Cooldown());
            }
        }
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            if (currentIndex >= sprites.Length)
            {
                currentIndex = 0;
                spikesOut = false;
                yield return new WaitForSeconds(delay);
            }

            if (currentIndex == sprites.Length -1)
            {
                spikesOut = true;
            }

            if (renderer)
            {
                renderer.sprite = sprites[currentIndex];
            }

            if (currentIndex == 0)
            {
                yield return new WaitForSeconds(delay);
            }

            currentIndex++;

            yield return new WaitForSeconds(speed);
        }
    }

    private IEnumerator Cooldown()
    {
        canDamage = false;

        yield return new WaitForSeconds(1);

        canDamage = true;
    }
}
