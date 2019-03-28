using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    #region Public Variables
    [Header("Audio")]
    [Tooltip("The music that should be played in the background.")]
    public AudioClip backgroundMusic;
    [Tooltip("The sound effect that is played whenever the boss attacks.")]
    public AudioClip attackSound;

    [Header("Health")]
    [Tooltip("The amount of health the boss starts with.")]
    public float health = 100;
    [Tooltip("Please assign the health slider of the boss here.")]
    public Slider healthSlider;

    [Header("Other")]
    public CanvasGroup finishedWindow;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to start the background music and set the health of the boss.
     */
    private void Start()
    {
        // Play backgroun music
        if (AudioManager.instance)
        {
            AudioManager.instance.music.clip = backgroundMusic;
            AudioManager.instance.music.Play();
        }

        // Set health
        healthSlider.maxValue   = health;
        healthSlider.value      = health;
    }

    /*
     * The boss's attack method
     */
    public void Attack(int heightState)
    {
        // Play attack sound
        if (AudioManager.instance)
        {
            AudioManager.instance.sfx.PlayOneShot(attackSound);
        }

        // Attack
        if (heightState != 0)
        {
            PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-.5f, -2), Quaternion.Euler(0, 0, 110));
            PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-.5f, -1.5f), Quaternion.Euler(0, 0, 100));
        }

        PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-.5f, -1), Quaternion.Euler(0, 0, 90));
        PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-.5f, -.5f), Quaternion.Euler(0, 0, 90));
        PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-.5f, 0), Quaternion.Euler(0, 0, 90));
        PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-.5f, .5f), Quaternion.Euler(0, 0, 90));
        PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-.5f, 1), Quaternion.Euler(0, 0, 90));

        if (heightState != 2)
        {
            PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-.5f, 1.5f), Quaternion.Euler(0, 0, 80));
            PoolManager.instance.InstantiateObject("Bullet", (Vector2)transform.position + new Vector2(-.5f, 2), Quaternion.Euler(0, 0, 70));
        }
    }

    /*
     * Whenever the player hits the boss the boss should be damaged.
     * Damage handler
     */
    public void DoDamage(int amount)
    {
        float newHealth = health - amount;

        if (newHealth < 0)
        {
            newHealth = 0;
        }

        health              = newHealth;
        healthSlider.value  = health;

        if (health == 0)
        {
            // Dead
            BossDied();
        }
    }

    /*
     * Whenever the boss dies, We have to show the player that he won.
     */
    private void BossDied()
    {
        // Save game
        SaveGame sg         = SaveGameManager.instance.currentSaveGame;
        sg.finishedBoss     = true;
        sg.progress         = 100;
        sg.coins            = CharacterManager.player.coins;

        SaveGameManager.instance.SaveGameToFile();

        // Show finished
        Time.timeScale = 0;
        finishedWindow.DOFade(1, .5f);
        finishedWindow.interactable     = true;
        finishedWindow.blocksRaycasts   = true;

        if (GameManager.instance.isUsingController)
        {
            finishedWindow.GetComponentInChildren<Button>().Select();
        }
    }
}