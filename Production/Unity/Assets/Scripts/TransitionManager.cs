using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    public RectTransform circle;
    public bool transistioning = false;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogWarning("Whoops! There can only be one Transition Manager");
        }

        instance = this;

        GetComponent<Canvas>().enabled = true;
    }

    public void CircleIn()
    {
        StartCoroutine(CircleAnimate(true));
    }

    public void CircleOut()
    {
        StartCoroutine(CircleAnimate(false));
    }

    public IEnumerator CircleAnimate(bool animateIn)
    {
        transistioning = true;

        if (animateIn)
        {
            while (circle.localScale.x > 0)
            {
                Vector3 newScale = circle.localScale - new Vector3(0.05f, 0.05f, 0);
                if (newScale.x < 0)
                {
                    newScale.x = 0;
                    newScale.y = 0;
                }

                circle.localScale = newScale;

                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (circle.localScale.x < 3)
            {
                Vector3 newScale = circle.localScale + new Vector3(0.05f, 0.05f, 0);

                circle.localScale = newScale;

                yield return new WaitForSeconds(0.01f);
            }
        }

        transistioning = false;
    }
}
