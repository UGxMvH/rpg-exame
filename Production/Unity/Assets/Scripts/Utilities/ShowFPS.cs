using UnityEngine;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    #region Public Variables
    public Text fpsText;
    public float deltaTime;
    #endregion

    /*
     * Update is called each frame.
     */
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }
}
