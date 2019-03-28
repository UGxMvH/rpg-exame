using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isUsingController = false;

    private void Awake()
    {
        instance = this;

        if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0].Contains("Xbox"))
        {
            isUsingController = true;
        }
    }
}
