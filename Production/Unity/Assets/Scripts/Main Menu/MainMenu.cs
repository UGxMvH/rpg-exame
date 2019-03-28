using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasScaler))]
public class MainMenu : MonoBehaviour
{
    [Header("Pages")]
    public RectTransform mainMenu;
    public RectTransform saveGames;
    public RectTransform settings;

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

    private RectTransform rect;
    private CanvasScaler scaler;

    private CanvasGroup mainMenuGroup;
    private CanvasGroup saveGamesGroup;
    private CanvasGroup settingsGroup;
    
    private bool mainMenuActive     = true;
    private bool saveGamesActive    = false;
    private bool settingsActive     = false;

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
    }

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

    public void Quit()
    {
        Application.Quit();
    }

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

    public void OnHoverExit(RectTransform rect)
    {
        if (rect.gameObject.name.Contains("Save game"))
        {
            rect.DOBlendableScaleBy(-saveGameScaleHover, .5f);
            return;
        }

        rect.DOBlendableScaleBy(-scaleHover, .5f);
    }
}
