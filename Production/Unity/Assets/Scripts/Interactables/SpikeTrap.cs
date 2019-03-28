using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class SpikeTrap : MonoBehaviour
{
    #region Public Variables
    public Sprite[] sprites;
    public float speed  = 0.1f;
    public float delay  = 2;
    public float damage = 3;
    #endregion

    #region Private Variables
    private Coroutine co;
    private new SpriteRenderer renderer;
    private int currentIndex;
    private bool canDamage = true;
    private bool spikesOut = false;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    /*
     * OnDisable is called when the behaviour becomes enabled
     */
    private void OnEnable()
    {
        co = StartCoroutine(Animate());
    }

    /*
     * OnDisable is called when the behaviour becomes disabled
     */
    private void OnDisable()
    {
        StopCoroutine(Animate());
    }

    /*
     * Called each frame whenever a GameObject is in the trigger.
     */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (spikesOut && canDamage)
        {
            CharacterManager player = collision.gameObject.GetComponent<CharacterManager>();

            if (player)
            {
                player.DoDamage(damage);
                StartCoroutine(Cooldown());
            }
        }
    }

    /*
     * Asynchronous code.
     * Animate spike trap
     */
    private IEnumerator Animate()
    {
        while (true)
        {
            if (currentIndex >= sprites.Length)
            {
                currentIndex = 0;
                spikesOut = false;
                yield return new WaitForSeconds(delay);
            }

            if (currentIndex == sprites.Length -1)
            {
                spikesOut = true;
            }

            if (renderer)
            {
                renderer.sprite = sprites[currentIndex];
            }

            if (currentIndex == 0)
            {
                yield return new WaitForSeconds(delay);
            }

            currentIndex++;

            yield return new WaitForSeconds(speed);
        }
    }

    /*
     * Asynchronous code.
     * Cooldown before hitting the player again.
     */
    private IEnumerator Cooldown()
    {
        canDamage = false;

        yield return new WaitForSeconds(1);

        canDamage = true;
    }
}
