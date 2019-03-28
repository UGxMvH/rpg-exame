using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Public Variables
    public static AudioManager instance;

    public AudioSource music;
    public AudioSource sfx;
    #endregion

    /*
     * Awake is called when the script instance is being loaded.
     * We use it to set a static refrence to the audiomanager.
     */
    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
            return;
        }

        instance = this;
    }
}
