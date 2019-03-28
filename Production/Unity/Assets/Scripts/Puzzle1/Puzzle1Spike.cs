using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Puzzle1Spike : MonoBehaviour
{
    #region Public Variables
    public new SpriteRenderer renderer;
    public bool spikesOut;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        spikesOut = true;
    }

    /*
     * OnTriggerEnter2D is called whenever a object enters the collider of this GameObject.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!spikesOut)
        {
            return;
        }

        CharacterManager player = collision.gameObject.GetComponent<CharacterManager>();

        if (player && !player.isAI)
        {
            player.DoDamage(1000);
        }
    }
}
