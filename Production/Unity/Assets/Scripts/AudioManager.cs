using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource music;
    public AudioSource sfx;

    private void Awake()
    {
        instance = this;
    }
}
