﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Public Variables
    public static LevelManager instace;
    public Dictionary<Vector2, Room> rooms = new Dictionary<Vector2, Room>();

    [Header("Settings")]
    public int minSize;
    public int maxSize;
    public int maxRooms = 10;
    public int minRooms = 5;
    public new SmoothCamera camera;
    public Transform player;
    public GameObject shop;
    public GameObject puzzle1;
    public AudioClip levelMusic;
    public CanvasGroup finishedWindow;

    [Header("Tiles")]
    public GameObject[] floorTiles;
    public GameObject[] leftWalls;
    public GameObject[] rightWalls;
    public GameObject[] topWalls;
    public GameObject[] bottomWalls;
    public GameObject cornerTopLeft;
    public GameObject cornerTopRight;
    public GameObject cornerBottomLeft;
    public GameObject cornerBottomRight;

    [Header("Doors")]
    public GameObject normalDoorLeft;
    public GameObject normalDoorRight;
    public GameObject normalDoorTop;
    public GameObject normalDoorBottom;

    [Header("Last Door")]
    public GameObject topIcon;
    public GameObject rightIcon;
    public GameObject bottomIcon;
    public GameObject leftIcon;

    [Header("Enemies")]
    public GameObject[] enemies;

    [Header("Transistion")]
    public RectTransform circle;

    [HideInInspector]
    public Room currentRoom;
    #endregion

    #region Private Variables
    private bool hasShop = false;
    #endregion

    /*
     * Awake is called when the script instance is being loaded.
     * We use it to set a static refrence to the LevelManager.
     */
    private void Awake()
    {
        instace = this;
    }

    /*
     * Start is called before the first frame update.
     * We use it to gether the required components and set default variables.
     */
    void Start()
    {
        // Check for mistakes
        if (floorTiles.Length == 0)
        {
            Debug.LogError("No floor tile specified");
        }

        if (maxSize <= minSize)
        {
            maxSize = minSize;
        }

        // Generate level
        StartCoroutine(GenerateLevel());

        // Start music
        if (AudioManager.instance)
        {
            AudioManager.instance.music.clip = levelMusic;
            AudioManager.instance.music.Play();
        }
    }

    /*
     * Make sure a integer is odd
     */
    private int MakeOdd(int i)
    {
        if (i % 2 == 0)
        {
            i++;
        }

        return i;
    }

    /*
     * Transist rooms.
     * Leave room A and go to room B.
     */
    public void TransistRoom(Room from, Room to)
    {
        StartCoroutine(TransistRooms(from, to));
    }

    /*
     * Restart level
     */
    public void RetryLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /*
     * Exit this level and go to OverWorld
     */
    public void BackToOverworld()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    /*
     * Level completed!
     */
    public void FinishedLevel()
    {
        // Finished
        Time.timeScale = 0;
        finishedWindow.DOFade(1, .5f);
        finishedWindow.GetComponent<RectTransform>().DOAnchorPosY(0, 1);
        finishedWindow.interactable     = true;
        finishedWindow.blocksRaycasts   = true;

        if (GameManager.instance.isUsingController)
        {
            finishedWindow.GetComponentInChildren<UnityEngine.UI.Button>().Select();
        }
    }

    /* 
     * Randomly generate a complete level.
     */
    private IEnumerator GenerateLevel()
    {
        // Fluid fill rooms
        yield return StartCoroutine(GenerateRoom(Vector2.zero));

        // Safety fallback
        if (rooms.Count < minRooms)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Choose random room that is going to be the shop
        int lowestRoom = minRooms - 2;
        if (lowestRoom <= 0)
        {
            lowestRoom = 1;
        }

        rooms.Values.ElementAt(Random.Range(lowestRoom, rooms.Count - 2)).isShopRoom = true;

        // Set last room
        rooms.Values.ElementAt(rooms.Count - 1).isLastRoom = true;

        // Set puzzle room
        if (SaveGameManager.instance)
        {
            if (!SaveGameManager.instance.currentSaveGame.finishedLvl1)
            {
                rooms.Values.ElementAt(0).isPuzzle1 = true;
            }
        }
        else
        {
            rooms.Values.ElementAt(0).isPuzzle1 = true;
        }

        // Rooms are generated let them finish and generate doors
        foreach (KeyValuePair<Vector2, Room> room in rooms)
        {
            // Diffrent kind of rooms
            if (room.Key != Vector2.zero && !room.Value.isShopRoom)
            {
                room.Value.containsEnemies = true;
            }

            // Finish room
            room.Value.Finish();
        }

        // Show room
        yield return StartCoroutine(TransitionManager.instance.CircleAnimate(false));

        // Open doors
        rooms[Vector2.zero].OpenDoors();
    }

    /*
     * Generate a room
     */
    private IEnumerator GenerateRoom(Vector2 virtualLoc)
    {
        // Default variables
        bool lastRoom   = false;
        bool mainRoom   = false;
        bool northDoor  = false;
        bool eastDoor   = false;
        bool southDoor  = false;
        bool westDoor   = false;

        if (rooms.Count == maxRooms)
        {
            lastRoom = true;
        }
        else if (rooms.Count > maxRooms)
        {
            yield break;
        }

        // Generate sizes
        int sizeX = MakeOdd(Random.Range(minSize, maxSize));
        yield return new WaitForEndOfFrame();
        int sizeY = MakeOdd(Random.Range(minSize, sizeX));

        // Create base gameobject
        GameObject roomGo = new GameObject("Room #" + (rooms.Count + 1));

        Room room = roomGo.AddComponent<Room>();
        room.myGenerator = this;
        room.virtualLoc = virtualLoc;
        room.size = new Vector2(sizeX, sizeY);
        room.Init();

        rooms.Add(virtualLoc, room);

        // Main room check
        if (rooms.Count == 1)
        {
            mainRoom = true;
        }

        // Check rooms on my sides
        if (room.northRoom)
        {
            northDoor = true;
        }

        if (room.eastRoom)
        {
            eastDoor = true;
        }

        if (room.southRoom)
        {
            southDoor = true;
        }

        if (room.westRoom)
        {
            westDoor = true;
        }

        // Datermine locations of doors
        // Top door check
        yield return new WaitForEndOfFrame();
        if (!northDoor && !lastRoom && Random.Range(1, 100) % 2 == 0)
        {
            // Yes we want a top door
            northDoor = true;
        }

        // Right door check
        yield return new WaitForEndOfFrame();
        if (!eastDoor && !lastRoom && Random.Range(1, 100) % 2 == 0)
        {
            // Yes we want a top door
            eastDoor = true;
        }

        // Bottom door check
        yield return new WaitForEndOfFrame();
        if (!southDoor && !lastRoom && Random.Range(1, 100) % 2 == 0)
        {
            // Yes we want a top door
            southDoor = true;
        }

        // Left door check
        yield return new WaitForEndOfFrame();
        if (!westDoor && !lastRoom && Random.Range(1, 100) % 2 == 0)
        {
            // Yes we want a top door
            westDoor = true;
        }
        
        // Make sure we get the minimum amount of rooms
        if (!northDoor && !eastDoor && !southDoor && !westDoor && rooms.Count < minRooms)
        {
            switch(Random.Range(1, 4))
            {
                case 1:
                    northDoor = true;
                    break;
                case 2:
                    eastDoor = true;
                    break;
                case 3:
                    southDoor = true;
                    break;
                case 4:
                    westDoor = true;
                    break;
            }
        }

        // Generate room
        for (int x = 0; x <  sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                // Create left wall
                if (x == 0 && leftWalls.Length > 0)
                {
                    GameObject wall = leftWalls[Random.Range(0, leftWalls.Length)];
                    Instantiate(wall, new Vector2(x - 1, y), Quaternion.identity, room.walls.transform);
                }

                // Create right wall
                if (x == (sizeX - 1) && rightWalls.Length > 0)
                {
                    GameObject wall = rightWalls[Random.Range(0, rightWalls.Length)];
                    Instantiate(wall, new Vector2(x + 1, y), Quaternion.identity, room.walls.transform);
                }

                // Create bottom wall
                if (y == 0 && bottomWalls.Length > 0)
                {
                    GameObject wall = bottomWalls[Random.Range(0, bottomWalls.Length)];
                    Instantiate(wall, new Vector2(x, y - 1), Quaternion.identity, room.walls.transform);
                }

                // Create top wall
                if (y == (sizeY - 1) && topWalls.Length > 0)
                {
                    GameObject wall = topWalls[Random.Range(0, topWalls.Length)];
                    Instantiate(wall, new Vector2(x, y + 1), Quaternion.identity, room.walls.transform);
                }

                // Create bottom left corner
                if (x == 0 && y == 0 && cornerBottomLeft)
                {
                    Instantiate(cornerBottomLeft, new Vector2(x - 1, y - 1), Quaternion.identity, room.walls.transform);
                }

                // Create bottom right corner
                if (x == (sizeX - 1)  && y == 0 && cornerBottomRight)
                {
                    Instantiate(cornerBottomRight, new Vector2(x + 1, y - 1), Quaternion.identity, room.walls.transform);
                }

                // Create top left corner
                if (x == 0 && y == (sizeY - 1) && cornerTopLeft)
                {
                    Instantiate(cornerTopLeft, new Vector2(x - 1, y + 1), Quaternion.identity, room.walls.transform);
                }

                // Create top right corner
                if (x == (sizeX - 1) && y == (sizeY - 1) && cornerTopRight)
                {
                    Instantiate(cornerTopRight, new Vector2(x + 1, y + 1), Quaternion.identity, room.walls.transform);
                }

                // Pick random tile
                GameObject tile = floorTiles[Random.Range(0, floorTiles.Length)];
                   
                // Create floor tile
                Instantiate(tile, new Vector2(x, y), Quaternion.identity, room.floor.transform);
            }
        }

        // Check if it is the start room
        if (mainRoom)
        {
            // Set camera focus on this room
            camera.target = room.transform;

            // Set offset to focus on the middle of the room
            camera.offset = new Vector2(sizeX / 2, sizeY / 2);

            camera.GetComponent<Camera>().orthographicSize = sizeY / 2 + 2;

            // Set current room
            currentRoom = room;
        }
        else
        {
            // Firstly disable room since we'll start in the main room
            roomGo.SetActive(false);
        }

        // Generate sub rooms
        if (!lastRoom)
        {
            if (northDoor)
            {
                Vector2 loc = new Vector2(virtualLoc.x, virtualLoc.y + 1);

                if (!rooms.ContainsKey(loc))
                {
                    yield return StartCoroutine(GenerateRoom(loc));
                }
            }

            if (eastDoor)
            {
                Vector2 loc = new Vector2(virtualLoc.x + 1, virtualLoc.y);

                if (!rooms.ContainsKey(loc))
                {
                    yield return StartCoroutine(GenerateRoom(loc));
                }
            }

            if (southDoor)
            {
                Vector2 loc = new Vector2(virtualLoc.x, virtualLoc.y - 1);

                if (!rooms.ContainsKey(loc))
                {
                    yield return StartCoroutine(GenerateRoom(loc));
                }
            }

            if (westDoor)
            {
                Vector2 loc = new Vector2(virtualLoc.x - 1, virtualLoc.y);

                if (!rooms.ContainsKey(loc))
                {
                    yield return StartCoroutine(GenerateRoom(loc));
                }
            }
        }
    }

    /*
     * Asynchronous code
     * Transist rooms smoothly with an animation.
     */
    private IEnumerator TransistRooms(Room from, Room to)
    {
        if (TransitionManager.instance.transistioning)
        {
            yield break;
        }

        // Spawn in circle
        yield return StartCoroutine(TransitionManager.instance.CircleAnimate(true));

        // Hide old room
        from.gameObject.SetActive(false);

        // Make new room active
        to.gameObject.SetActive(true);

        // Set camera to new room
        camera.target = to.transform;
        camera.offset = new Vector2(Mathf.Floor(to.size.x / 2), Mathf.Floor(to.size.y / 2));
        camera.GetComponent<Camera>().orthographicSize = to.size.y / 2 + 2;

        // Move player to correct location
        if (from.northRoom == to)
        {
            player.position = new Vector2(Mathf.Floor(to.size.x / 2), 0);
        }

        if (from.eastRoom == to)
        {
            player.position = new Vector2(0, Mathf.Floor(to.size.y / 2) + 0.25f);
        }

        if (from.southRoom == to)
        {
            player.position = new Vector2(Mathf.Floor(to.size.x / 2), to.size.y - 1);
        }

        if (from.westRoom == to)
        {
            player.position = new Vector2(to.size.x - 1, Mathf.Floor(to.size.y / 2) + 0.25f);
        }

        yield return new WaitForSeconds(.5f);

        currentRoom = to;

        // Hide circle
        yield return StartCoroutine(TransitionManager.instance.CircleAnimate(false));

        to.EnteredRoom();
    }
}
