using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class EnterLevelTrigger : MonoBehaviour
{
    [Header("Settings")]
    public bool level1;
    public bool level2;
    public bool level3;
    public bool boss;

    private SaveGame saveGame;

    private void Start()
    {
        saveGame = SaveGameManager.instance.currentSaveGame;
    }

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
