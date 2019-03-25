using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    internal AudioManager audioManager;
    internal float musicVolume = .75f;
    internal float sfxVolume = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    public void ChangeMusicVolume(Slider slider)
    {
        musicVolume = slider.value;

        audioManager.music.volume = musicVolume;
    }

    public void changeSFXVolume(Slider slider)
    {
        sfxVolume = slider.value;

        audioManager.sfx.volume = sfxVolume;
    }
}
