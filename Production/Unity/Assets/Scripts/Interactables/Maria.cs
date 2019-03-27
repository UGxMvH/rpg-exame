using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Maria : Interactable
{
    public CanvasGroup StoryLine;
    public Text speech;
    public float speed = 0.1f;

    private Coroutine co;
    private bool isTalking;
    private char[] speechText;

    private void Start()
    {
        // Get speech
        speechText = speech.text.ToCharArray();
    }

    public override void OnInteract()
    {
        base.OnInteract();

        // Remove text balloon
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        // Tell storyline
        if (!isTalking)
        {
            co = StartCoroutine(TellStory());
        }
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Attack") || Input.GetKeyDown(KeyCode.Escape))
        {
            if(isTalking)
            {
                FinishDialog();
            }
        }
    }

    private IEnumerator TellStory()
    {
        yield return new WaitForEndOfFrame();

        isTalking = true;

        // Clear text
        speech.text = "";

        // Pause game
        Time.timeScale = 0;

        StoryLine.DOFade(1, 1);

        yield return new WaitForSecondsRealtime(1);

        for (int i = 0; i < speechText.Length; i++)
        {
            speech.text += speechText[i];

            yield return new WaitForSecondsRealtime(speed);
        }
    }

    private void FinishDialog()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }

        StoryLine.DOFade(0, 1);
        Time.timeScale = 1;
        isTalking = false;
    }
}
