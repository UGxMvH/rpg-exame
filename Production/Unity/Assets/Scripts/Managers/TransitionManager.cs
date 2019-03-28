using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    #region Public Variables
    public static TransitionManager instance;

    public RectTransform circle;
    public bool transistioning = false;
    #endregion

    /*
     * Awake is called when the script instance is being loaded.
     * We use it to set a static refrence to the TransitionManager.
     * Also we collect the required components here before they can be used in other start functions
     */
    private void Awake()
    {
        instance = this;

        GetComponent<Canvas>().enabled = true;
    }

    /*
     * Close view with circle getting smaller
     */
    public void CircleIn()
    {
        StartCoroutine(CircleAnimate(true));
    }

    /*
     * Open veiw with circle getting bigger
     */
    public void CircleOut()
    {
        StartCoroutine(CircleAnimate(false));
    }

    /*
     * Animate circle in or out
     */
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
