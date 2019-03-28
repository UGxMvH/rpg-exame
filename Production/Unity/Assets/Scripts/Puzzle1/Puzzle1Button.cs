using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Puzzle1Button : MonoBehaviour
{
    #region Public Variables
    public Puzzle1Spike[] spikes;
    public Sprite spikesOut;
    public Sprite spikesIn;
    public Sprite on;
    public Sprite off;
    public AudioClip audioOn;
    public AudioClip audioOff;
    #endregion

    #region Private Variables
    private BoxCollider2D boxCollider;
    private new SpriteRenderer renderer;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        renderer    = GetComponent<SpriteRenderer>();
    }

    /*
     * OnTriggerEnter2D is called whenever a object enters the collider of this GameObject.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckIfPressed();
    }

    /*
     * OnTriggerEnter2D is called whenever a object leaves the collider of this GameObject.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckIfPressed();
    }

    /*
     * The button is Pressed
     */
    private void Pressed()
    {
        // Check if spikes are already off
        if (!spikes[0].spikesOut)
        {
            return;
        }

        if (AudioManager.instance)
        {
            AudioManager.instance.sfx.PlayOneShot(audioOn);
        }

        foreach(Puzzle1Spike trap in spikes)
        {
            trap.renderer.sprite = spikesIn;
            trap.spikesOut = false;
        }

        renderer.sprite = on;
    }

    /*
     * The button is unpressed
     */
    private void UnPressed()
    {
        // Check if spikes are already on
        if (spikes[0].spikesOut)
        {
            return;
        }

        if (AudioManager.instance)
        {
            AudioManager.instance.sfx.PlayOneShot(audioOff);
        }

        foreach (Puzzle1Spike trap in spikes)
        {
            trap.renderer.sprite = spikesOut;
            trap.spikesOut = true;
        }

        renderer.sprite = off;
    }

    /*
     * Check if the button is pressed by the player or box
     */
    private void CheckIfPressed()
    {
        Collider2D[] results = new Collider2D[10];
        int amount = boxCollider.OverlapCollider(new ContactFilter2D(), results);

        for (int i = 0; i < amount; i++)
        {
            // Check if player or box is on this button
            CharacterManager player = results[i].gameObject.GetComponent<CharacterManager>();

            if (player && !player.isAI)
            {
                Pressed();
                return;
            }

            if (results[i].gameObject.GetComponent<Puzzle1Box>())
            {
                Pressed();
                return;
            }
        }

        UnPressed();
    }
}
