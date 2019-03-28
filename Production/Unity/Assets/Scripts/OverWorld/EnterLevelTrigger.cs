using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class EnterLevelTrigger : MonoBehaviour
{
    #region Public Variables
    [Header("Settings")]
    public bool level1;
    public bool level2;
    public bool level3;
    public bool boss;
    #endregion

    #region Private Variables
    private SaveGame saveGame;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
    private void Start()
    {
        saveGame = SaveGameManager.instance.currentSaveGame;
    }

    /*
     * OnTriggerEnter2D is called whenever a object enters the collider of this GameObject.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterManager player = collision.GetComponent<CharacterManager>();

        if (player && !player.isAI)
        {
            if (level1 && !saveGame.finishedLvl1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            if (level2 && !saveGame.finisehedLvl2 && saveGame.finishedLvl1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            if (level3 && !saveGame.finishedLvl3 && saveGame.finisehedLvl2)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            if (boss && saveGame.finishedLvl3 && !saveGame.finishedBoss)
            {
                SceneManager.LoadScene(3);
            }
        }
    }
}
