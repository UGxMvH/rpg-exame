using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EasyAnimate : MonoBehaviour
{
    public float speed = 0.1f;
    public Sprite[] sprites;

    private int currentIndex = 0;
    private new SpriteRenderer renderer;
    private Coroutine co;

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
        while(true)
        {
            if (currentIndex >= sprites.Length)
            {
                currentIndex = 0;
            }

            if (renderer)
            {
                renderer.sprite = sprites[currentIndex];
            }

            currentIndex++;

            yield return new WaitForSeconds(speed);
        }
    }
}
