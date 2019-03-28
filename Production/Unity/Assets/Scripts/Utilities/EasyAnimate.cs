using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EasyAnimate : MonoBehaviour
{
    #region Public Variables
    public float speed = 0.1f;
    public Sprite[] sprites;
    #endregion

    #region Public Variables
    private int currentIndex = 0;
    private new SpriteRenderer renderer;
    private Coroutine co;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    /*
     * OnDisable is called when the behaviour becomes enabled
     */
    private void OnEnable()
    {
        co = StartCoroutine(Animate());
    }

    /*
     * OnDisable is called when the behaviour becomes disabled
     */
    private void OnDisable()
    {
        StopCoroutine(Animate());
    }

    /*
     * Animate
     * Loop through sprite array
     */
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
