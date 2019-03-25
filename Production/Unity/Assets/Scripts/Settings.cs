using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public MainMenu mainMenu;

    internal float musicVolume = .75f;
    internal float sfxVolume = 1;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeMusicVolume(Slider slider)
    {
        musicVolume = slider.value;

        if (mainMenu)
        {
            mainMenu.music.volume = musicVolume;
        }
    }

    public void changeSFXVolume(Slider slider)
    {
        sfxVolume = slider.value;

        if (mainMenu)
        {
            mainMenu.sfx.volume = sfxVolume;
        }
    }
}
