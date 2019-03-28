using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class AnimateTitle : MonoBehaviour
{
    #region Private Variables
    private RectTransform rect;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     * Start animation
     */
    private void Start()
    {
        rect = GetComponent<RectTransform>();

        StartCoroutine(Animate());
    }

    /*
     * Animate Text title
     */
    private IEnumerator Animate()
    {
        while (true)
        {
            rect.DOShakeScale(.5f, .1f, 2);

            yield return new WaitForSecondsRealtime(.5f);
        }
    }
}
