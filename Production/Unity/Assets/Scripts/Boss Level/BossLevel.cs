using UnityEngine;
using UnityEngine.SceneManagement;

public class BossLevel : MonoBehaviour
{
    /*
     * Restart boss level
     */
    public void Restart()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /*
     * Exit level and go to Overworld
     */
    public void GoToOverworld()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(1);
    }

    /*
     * Exit level and go to main menu
     */
    public void GoToMainMenu()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }
}
