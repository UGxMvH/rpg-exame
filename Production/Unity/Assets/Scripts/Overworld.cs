using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overworld : MonoBehaviour
{
    public AudioClip backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.music.clip = backgroundMusic;
        AudioManager.instance.music.Play();
    }
}
