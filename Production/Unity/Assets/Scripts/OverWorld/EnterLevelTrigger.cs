using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class EnterLevelTrigger : MonoBehaviour
{
    [Header("Settings")]
    public int buildIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterManager player = collision.GetComponent<CharacterManager>();

        if (player && !player.isAI)
        {
            SceneManager.LoadScene(buildIndex);
        }
    }
}
