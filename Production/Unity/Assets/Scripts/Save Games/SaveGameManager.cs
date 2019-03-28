using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveGameManager : MonoBehaviour
{
    #region Public Variables
    public static SaveGameManager instance;

    public SaveGame saveGame1;
    public SaveGame saveGame2;
    public SaveGame saveGame3;
    #endregion

    #region Internal Variables
    internal SaveGame currentSaveGame;
    internal string saveLocation;
    internal string currentFile;
    #endregion

    /*
     * Save current game to file
     */
    public void SaveGameToFile()
    {
        // Set title
        currentSaveGame.title = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();

        // Remove file if already exists
        if (File.Exists(currentFile))
        {
            File.Delete(currentFile);
        }

        // Save
        File.WriteAllText(currentFile, JsonUtility.ToJson(currentSaveGame));
    }

    /*
     * Set a static variable to this class so we can access data via other scripts
     */
    private void Awake()
    {
        instance = this;

        // Don't destroy on load
        DontDestroyOnLoad(gameObject);

        // Set save game location
        saveLocation = Application.dataPath + "/savegames/";

        // Make sure location exists
        Directory.CreateDirectory(saveLocation);
    }

    /*
     * User clicked on a save game so we need to load it
     * Create the file if it is a new save game
     */
     public void LoadGame(int saveGameIndex)
    {
        // Get correct save game
        SaveGame sg = null;

        switch (saveGameIndex)
        {
            case 0:
                sg = saveGame1;
                break;
            case 1:
                sg = saveGame2;
                break;
            case 2:
                sg = saveGame3;
                break;
        }

        // Set variables
        currentSaveGame = sg;
        currentFile = saveLocation + saveGameIndex + ".rpg";

        // if empty create first save :D
        if (sg.title == "")
        {
            // First save
            SaveGameToFile();
        }

        // Load new scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /*
     * Read a save file to Update the save game list with the correct data
     */
    public void ReadSaveFile(ref SaveGame sg, GameObject go, string file)
    {
        if (File.Exists(file))
        {
            try
            {
                sg = JsonUtility.FromJson<SaveGame>(File.ReadAllText(file));

                // Change UI for save game
                go.transform.GetChild(0).GetComponent<Text>().text = sg.title;
                go.transform.GetChild(1).GetComponent<Text>().text = "Progress: " + sg.progress + "%";
            }
            catch (Exception e)
            {
                Debug.Log("Save game error: " + e);
            }
        }
    }
}
