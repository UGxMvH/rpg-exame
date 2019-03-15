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

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while(true)
        {
            if (currentIndex >= sprites.Length)
            {
                currentIndex = 0;
            }

            renderer.sprite = sprites[currentIndex];

            currentIndex++;

            yield return new WaitForSeconds(speed);
        }
    }
}
