using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    [Header("Pages")]
    public RectTransform mainMenu;
    public RectTransform saveGames;

    [Header("Button")]
    public Vector3 scaleHover;
    public Vector3 saveGameScaleHover;

    [Header("Sounds")]
    public AudioClip hoverButton;

    private new AudioSource audio;
    private RectTransform rect;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        rect = GetComponent<RectTransform>();

        saveGames.DOMoveX(Screen.width * 1.5f, 0);
    }

    public void ShowSaveGames()
    {
        mainMenu.DOMoveX(-Screen.width / 2, 2);
        saveGames.DOMoveX(Screen.width / 2, 2);
    }

    public void ShowMainMenu()
    {
        mainMenu.DOMoveX(Screen.width / 2, 2);
        saveGames.DOMoveX(Screen.width * 1.5f, 2);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnHoverEnter(RectTransform rect)
    {
        audio.PlayOneShot(hoverButton);

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
