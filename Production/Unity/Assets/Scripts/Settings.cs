using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    #region Public Variables
    public static Settings instance;
    #endregion

    #region Internal Variables
    internal AudioManager audioManager;
    internal float musicVolume = .75f;
    internal float sfxVolume = 1;
    #endregion

    /*
     * Awake is called when the script instance is being loaded.
     * We use it to set a static refrence to the Settings.
     */
    private void Awake()
    {
        instance = this;
    }

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    /*
     * Change volume of background music
     */
    public void ChangeMusicVolume(Slider slider)
    {
        musicVolume = slider.value;

        audioManager.music.volume = musicVolume;
    }

    /*
     * Change volume of SFX sounds
     */
    public void changeSFXVolume(Slider slider)
    {
        sfxVolume = slider.value;

        audioManager.sfx.volume = sfxVolume;
    }
}
