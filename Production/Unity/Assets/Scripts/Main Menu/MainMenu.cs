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

    private RectTransform rect;
    private CanvasScaler scaler;

    private void Start()
    {
        // Get variables
        rect = GetComponent<RectTransform>();
        scaler = GetComponent<CanvasScaler>();

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
    }

    public void ShowSaveGames()
    {
        mainMenu.DOMoveX(-Screen.width / 2, 2);
        saveGames.DOMoveX(Screen.width / 2, 2);
    }

    public void ShowMainMenuFromSaveGames()
    {
        mainMenu.DOMoveX(Screen.width / 2, 2);
        saveGames.DOMoveX(Screen.width * 1.5f, 2);
    }

    public void ShowSettings()
    {
        mainMenu.DOMoveX(Screen.width * 1.5f, 2);
        settings.DOMoveX(Screen.width / 2, 2);
    }

    public void ShowMainMenuFromSettings()
    {
        mainMenu.DOMoveX(Screen.width / 2, 2);
        settings.DOMoveX(-Screen.width / 2, 2);
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
