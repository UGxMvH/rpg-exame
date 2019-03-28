using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Overworld : MonoBehaviour
{
    public AudioClip backgroundMusic;

    public CanvasGroup pauseWindow;

    public GameObject overlayLvl2;
    public GameObject overlayLvl3;
    public GameObject overlayLvlBoss;
    public GameObject overlayFinishedGame;

    private bool inPauseMenu;

    // Start is called before the first frame update
    private void Start()
    {
        AudioManager.instance.music.clip = backgroundMusic;
        AudioManager.instance.music.Play();

        // enable overlay if needed
        SaveGame sg = SaveGameManager.instance.currentSaveGame;

        if (sg.finishedBoss)
        {
            overlayFinishedGame.SetActive(true);
        }
        else if (sg.finishedLvl3)
        {
            overlayLvlBoss.SetActive(true);
        }
        else if (sg.finisehedLvl2)
        {
            overlayLvl3.SetActive(true);
        }
        else if (sg.finishedLvl1)
        {
            overlayLvl2.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (inPauseMenu)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
        }
    }

    private void OpenPauseMenu()
    {
        inPauseMenu = true;
        Time.timeScale = 0;
        pauseWindow.DOFade(1, .5f);
        pauseWindow.interactable = true;
        pauseWindow.blocksRaycasts = true;

        if (GameManager.instance.isUsingController)
        {
            pauseWindow.GetComponentInChildren<Button>().Select();
        }
    }

    public void ClosePauseMenu()
    {
        inPauseMenu = false;
        Time.timeScale = 1;
        pauseWindow.DOFade(0, .5f);
        pauseWindow.interactable = false;
        pauseWindow.blocksRaycasts = false;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
