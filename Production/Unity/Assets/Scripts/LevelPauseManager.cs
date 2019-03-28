using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPauseManager : MonoBehaviour
{
    public CanvasGroup pauseWindow;

    private bool isPaused;

    // Update is called once per frame
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

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        isPaused = true;
        pauseWindow.blocksRaycasts = true;
        pauseWindow.interactable = true;
        pauseWindow.DOFade(1, .5f);

        if (GameManager.instance.isUsingController)
        {
            pauseWindow.GetComponentInChildren<UnityEngine.UI.Button>().Select();
        }
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        isPaused = false;
        pauseWindow.blocksRaycasts = false;
        pauseWindow.interactable = false;
        pauseWindow.DOFade(0, .5f);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadOverworld()
    {
        SceneManager.LoadScene(1);
    }
}
