using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Overworld : MonoBehaviour
{
    #region Public Variables
    public AudioClip backgroundMusic;

    public CanvasGroup pauseWindow;

    public GameObject overlayLvl2;
    public GameObject overlayLvl3;
    public GameObject overlayLvlBoss;
    public GameObject overlayFinishedGame;

    public Text interactText;
    #endregion

    #region Private Variables
    private bool inPauseMenu;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
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

        // Change interact text if using controller
        if (GameManager.instance.isUsingController)
        {
            interactText.text = "Press \"A\" to interact!";
        }
    }

    /*
     * Update is called each frame.
     */
    private void Update()
    {
        // Check if player presses pause button
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

    /*
     * Open pause menu
     */
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

    /*
     * Close pause menu
     */
    public void ClosePauseMenu()
    {
        inPauseMenu = false;
        Time.timeScale = 1;
        pauseWindow.DOFade(0, .5f);
        pauseWindow.interactable = false;
        pauseWindow.blocksRaycasts = false;
    }

    /*
     * Exit overworld and go back to main menu
     */
    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
