using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class AnimateTitle : MonoBehaviour
{
    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            rect.DOShakeScale(.5f, .1f, 2);

            yield return new WaitForSecondsRealtime(.5f);
        }
    }
}
