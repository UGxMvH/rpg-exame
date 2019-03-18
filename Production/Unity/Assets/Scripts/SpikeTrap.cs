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

    public Sprite[] sprites;
    public float speed = 0.1f;
    public float delay = 2;

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

    private IEnumerator Animate()
    {
        while (true)
        {
            if (currentIndex >= sprites.Length)
            {
                currentIndex = 0;
                yield return new WaitForSeconds(delay);
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
}
