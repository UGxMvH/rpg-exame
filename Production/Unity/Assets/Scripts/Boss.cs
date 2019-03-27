using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip backgroundMusic;
    public AudioClip attackSound;

    [Header("Health")]
    public float health = 100;
    public Slider healthSlider;

    [Header("Other")]
    public GameObject finishedWindow;

    private void Start()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.music.clip = backgroundMusic;
            AudioManager.instance.music.Play();
        }

        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

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

    public void DoDamage(int amount)
    {
        float newHealth = health - amount;

        if (newHealth < 0)
        {
            newHealth = 0;
        }

        health = newHealth;
        healthSlider.value = health;

        if (health == 0)
        {
            // Dead
            BossDied();
        }
    }

    private void BossDied()
    {
        // Save game
        SaveGame sg = SaveGameManager.instance.currentSaveGame;
        sg.finishedBoss = true;
        SaveGameManager.instance.SaveGameToFile();

        // Show finished
        Time.timeScale = 0;
        finishedWindow.SetActive(true);
    }
}
