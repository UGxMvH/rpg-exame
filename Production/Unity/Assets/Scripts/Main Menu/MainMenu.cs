using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    [Header("Button")]
    public Vector3 scaleHover;

    [Header("Sounds")]
    public AudioClip hoverButton;

    private new AudioSource audio;
    private RectTransform rect;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        rect = GetComponent<RectTransform>();
    }

    public void GoToOverworld()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnHoverEnter(RectTransform rect)
    {
        audio.PlayOneShot(hoverButton);
        rect.DOBlendableScaleBy(scaleHover, .5f);
    }

    public void OnHoverExit(RectTransform rect)
    {
        rect.DOBlendableScaleBy(-scaleHover, .5f);
    }
}
