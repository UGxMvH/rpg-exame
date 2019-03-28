using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Door : MonoBehaviour
{
    #region Public Variables
    public Room room;
    public bool finishDoor;
    public Sprite[] doorAnimation;
    #endregion

    #region Private Variables
    private new SpriteRenderer renderer;
    #endregion

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    /*
     * OnTriggerEnter2D is called whenever a object enters the collider of this GameObject.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterManager>() && !collision.gameObject.GetComponent<CharacterManager>().isAI && room.doorsOpen)
        {
            if (finishDoor)
            {
                // Level finished

                // Save game
                SaveGame sg = SaveGameManager.instance.currentSaveGame;

                if (!sg.finishedLvl1)
                {
                    sg.finishedLvl1 = true;
                    sg.progress = 25;
                }
                else if (!sg.finisehedLvl2)
                {
                    sg.finisehedLvl2 = true;
                    sg.progress = 50;
                }
                else if (!sg.finishedLvl3)
                {
                    sg.finishedLvl3 = true;
                    sg.progress = 75;
                }
                else
                {
                    sg.finishedBoss = true;
                    sg.progress = 100;
                }

                sg.health       = CharacterManager.player.health;
                sg.maxHealth    = CharacterManager.player.health;
                sg.coins        = CharacterManager.player.coins;
                sg.damage       = CharacterManager.player.damage;

                // Save game
                SaveGameManager.instance.SaveGameToFile();

                // Tell level that its finished
                room.myGenerator.FinishedLevel();
            }
            else
            {
                // Go to the next room
                room.LeaveRoom(this);
            }
        }
    }

    /*
     * Asynchronous code
     * Animation to open this door
     */
    public IEnumerator AnimateOpen()
    {
        int index = 0;

        while(index < doorAnimation.Length)
        {
            renderer.sprite = doorAnimation[index];

            index++;

            yield return new WaitForSeconds(.04f);
        }
    }
}
