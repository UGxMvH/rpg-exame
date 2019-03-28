using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPauseManager : MonoBehaviour
{
    #region Public Variables
    public CanvasGroup pauseWindow;
    #endregion

    #region Private Variables
    private bool isPaused;
    #endregion

    /*
     * Update is called each frame.
     */
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
        }
    }

    /*
     * Open pause menu
     */
    public void OpenPauseMenu()
    {
        Time.timeScale  = 0;
        isPaused        = true;
        pauseWindow.blocksRaycasts  = true;
        pauseWindow.interactable    = true;
        pauseWindow.DOFade(1, .5f);

        if (GameManager.instance.isUsingController)
        {
            pauseWindow.GetComponentInChildren<UnityEngine.UI.Button>().Select();
        }
    }

    /*
     * Close Pause menu
     */
    public void ClosePauseMenu()
    {
        Time.timeScale  = 1;
        isPaused        = false;
        pauseWindow.blocksRaycasts  = false;
        pauseWindow.interactable    = false;
        pauseWindow.DOFade(0, .5f);
    }

    /*
     * Exit level and go to main menu
     */
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    /*
     * Exit level and go to Overworld
     */
    public void LoadOverworld()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
