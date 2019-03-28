using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.IO;
using System;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasScaler))]
public class MainMenu : MonoBehaviour
{
    #region Public Variables
    [Header("Pages")]
    public RectTransform mainMenu;
    public RectTransform saveGames;
    public RectTransform settings;

    [Header("Save Games")]
    public GameObject saveGame1Go;
    public GameObject saveGame2Go;
    public GameObject saveGame3Go;

    [Header("Button")]
    public Vector3 scaleHover;
    public Vector3 saveGameScaleHover;

    [Header("Sounds")]
    public AudioClip hoverButton;
    public AudioClip backgroundMuisc;

    [Header("Main selected buttons")]
    public Button mainMenuButton;
    public Button saveGamesButton;
    public Slider settingsButton;
    #endregion

    #region Private Variables
    private RectTransform rect;
    private CanvasScaler scaler;

    private CanvasGroup mainMenuGroup;
    private CanvasGroup saveGamesGroup;
    private CanvasGroup settingsGroup;
    
    private bool mainMenuActive     = true;
    private bool saveGamesActive    = false;
    private bool settingsActive     = false;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
    private void Start()
    {
        // Get variables
        rect    = GetComponent<RectTransform>();
        scaler  = GetComponent<CanvasScaler>();

        mainMenuGroup   = mainMenu.GetComponent<CanvasGroup>();
        saveGamesGroup  = saveGames.GetComponent<CanvasGroup>();
        settingsGroup   = settings.GetComponent<CanvasGroup>();

        // Set variables
        if (Screen.width > 1920)
        {
            scaler.referenceResolution = new Vector2(2560, 1080);
        }

        // Make sure the other screens are nog visible
        settings.DOMoveX(-Screen.width / 2, 0);
        saveGames.DOMoveX(Screen.width * 1.5f, 0);

        // Play sound
        AudioManager.instance.music.clip = backgroundMuisc;
        AudioManager.instance.music.Play();

        // Select main button
        if (GameManager.instance.isUsingController)
        {
            mainMenuButton.Select();
        }

        // Load save games
        LoadSaveGames();
    }

    /*
     * Update is called each frame.
     */
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (saveGamesActive)
            {
                ShowMainMenuFromSaveGames();
            }
            else if (settingsActive)
            {
                ShowMainMenuFromSettings();
            }
        }
    }

    /* 
     * Show Save Games page
     */
    public void ShowSaveGames()
    {
        mainMenu.DOMoveX(-Screen.width / 2, 2);
        saveGames.DOMoveX(Screen.width / 2, 2);

        mainMenuActive = false;
        saveGamesActive = true;

        mainMenuGroup.interactable = false;
        mainMenuGroup.blocksRaycasts = false;

        saveGamesGroup.interactable = true;
        saveGamesGroup.blocksRaycasts = true;

        if (GameManager.instance.isUsingController)
        {
            saveGamesButton.Select();
        }
    }

    /*
     * Go back to Main menu from save games
     */
    public void ShowMainMenuFromSaveGames()
    {
        mainMenu.DOMoveX(Screen.width / 2, 2);
        saveGames.DOMoveX(Screen.width * 1.5f, 2);

        mainMenuActive = true;
        saveGamesActive = false;

        saveGamesGroup.interactable = false;
        saveGamesGroup.blocksRaycasts = false;

        mainMenuGroup.interactable = true;
        mainMenuGroup.blocksRaycasts = true;

        if (GameManager.instance.isUsingController)
        {
            mainMenuButton.Select();
        }
    }

    /*
     * Show Settings page
     */
    public void ShowSettings()
    {
        mainMenu.DOMoveX(Screen.width * 1.5f, 2);
        settings.DOMoveX(Screen.width / 2, 2);

        mainMenuActive = false;
        settingsActive = true;

        mainMenuGroup.interactable = false;
        mainMenuGroup.blocksRaycasts = false;

        settingsGroup.interactable = true;
        settingsGroup.blocksRaycasts = true;

        if (GameManager.instance.isUsingController)
        {
            settingsButton.Select();
        }
    }

    /*
     * Go back to main menu from settings page
     */
    public void ShowMainMenuFromSettings()
    {
        mainMenu.DOMoveX(Screen.width / 2, 2);
        settings.DOMoveX(-Screen.width / 2, 2);

        mainMenuActive = true;
        settingsActive = false;

        settingsGroup.interactable = false;
        settingsGroup.blocksRaycasts = false;

        mainMenuGroup.interactable = true;
        mainMenuGroup.blocksRaycasts = true;

        if (GameManager.instance.isUsingController)
        {
            mainMenuButton.Select();
        }
    }

    /*
     * Quit Game
     */
    public void Quit()
    {
        Application.Quit();
    }

    /*
     * Wehenever the player is hovering over button with his mouse.
     */
    public void OnHoverEnter(RectTransform rect)
    {
        AudioManager.instance.sfx.PlayOneShot(hoverButton);

        if (rect.gameObject.name.Contains("Save game"))
        {
            rect.DOBlendableScaleBy(saveGameScaleHover, .5f);
            return;
        }

        rect.DOBlendableScaleBy(scaleHover, .5f);
    }

    /*
     * Whenever the player stops hovering over button with his mouse
     */
    public void OnHoverExit(RectTransform rect)
    {
        if (rect.gameObject.name.Contains("Save game"))
        {
            rect.DOBlendableScaleBy(-saveGameScaleHover, .5f);
            return;
        }

        rect.DOBlendableScaleBy(-scaleHover, .5f);
    }

    /*
     * User clicked on a save game so we need to load it
     * Create the file if it is a new save game
     */
     public void LoadGame(int saveGameIndex)
    {
        SaveGameManager.instance.LoadGame(saveGameIndex);
    }

    /*
     * Load every save game to display them in the UI
     */
    private void LoadSaveGames()
    {
        SaveGameManager.instance.ReadSaveFile(ref SaveGameManager.instance.saveGame1, saveGame1Go, SaveGameManager.instance.saveLocation + "0.rpg");
        SaveGameManager.instance.ReadSaveFile(ref SaveGameManager.instance.saveGame2, saveGame2Go, SaveGameManager.instance.saveLocation + "1.rpg");
        SaveGameManager.instance.ReadSaveFile(ref SaveGameManager.instance.saveGame3, saveGame3Go, SaveGameManager.instance.saveLocation + "2.rpg");
    }
}
