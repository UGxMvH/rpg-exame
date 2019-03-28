using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Maria : Interactable
{
    #region Public Variables
    public CanvasGroup StoryLine;
    public Text speech;
    public float speed = 0.1f;
    #endregion

    #region Private Variables
    private Coroutine co;
    private bool isTalking;
    private char[] speechText;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     * Also destroy the balloon if level 1 is already finished.
     */
    private void Start()
    {
        // Get speech
        speechText = speech.text.ToCharArray();

        // Remove text balloon if finished level 1
        if (SaveGameManager.instance.currentSaveGame.finishedLvl1)
        {
            // Remove text balloon
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }

    /*
     * Called whenever the player is interacting with this GameObject
     */
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

    /*
     * Update is called each frame.
     */
    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Attack") || Input.GetButtonDown("Cancel"))
        {
            if(isTalking)
            {
                FinishDialog();
            }
        }
    }

    /*
     * Tell storyline to player.
     */
    private IEnumerator TellStory()
    {
        yield return new WaitForEndOfFrame();

        isTalking = true;

        // Clear text
        speech.text = "";

        // Pause game
        Time.timeScale = 0;

        StoryLine.DOFade(1, .5f);

        yield return new WaitForSecondsRealtime(1);

        for (int i = 0; i < speechText.Length; i++)
        {
            speech.text += speechText[i];

            yield return new WaitForSecondsRealtime(speed);
        }
    }

    /*
     * Finish dialog and make it disapear
     */
    private void FinishDialog()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }

        StoryLine.DOFade(0, .5f);
        Time.timeScale = 1;
        isTalking = false;
    }
}
