using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public Variables
    public static GameManager instance;

    public bool isUsingController = false;
    #endregion

    /*
     * Awake is called when the script instance is being loaded.
     * We use it to set a static refrence to the GameManager.
     */
    private void Awake()
    {
        instance = this;

        if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].Contains("Xbox"))
        {
            isUsingController = true;
        }
    }
}
