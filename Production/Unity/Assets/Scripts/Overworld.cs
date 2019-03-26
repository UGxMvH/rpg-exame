using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overworld : MonoBehaviour
{
    public AudioClip backgroundMusic;

    public GameObject overlayLvl2;
    public GameObject overlayLvl3;
    public GameObject overlayLvlBoss;
    public GameObject overlayFinishedGame;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.music.clip = backgroundMusic;
        AudioManager.instance.music.Play();

        // enable overlay if needed
        SaveGame sg = SaveGameManager.instance.currentSaveGame;

        if (sg.finishedBoss)
        {
            overlayFinishedGame.SetActive(true);
        }
        else if (sg.finishedLvl3)
        {
            overlayLvlBoss.SetActive(true);
        }
        else if (sg.finisehedLvl2)
        {
            overlayLvl3.SetActive(true);
        }
        else if (sg.finishedLvl1)
        {
            overlayLvl2.SetActive(true);
        }
    }
}
