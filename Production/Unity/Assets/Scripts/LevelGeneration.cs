using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Dictionary<Vector2, Room> rooms = new Dictionary<Vector2, Room>();

    [Header("Settings")]
    public int minSize;
    public int maxSize;
    public int maxRooms = 10;
    public int minRooms = 5;
    public new SmoothCamera camera;

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

    // Start is called before the first frame update
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

        // Gen main room
       StartCoroutine(GenerateRoom(Vector2.zero));
    }

    private int MakeOdd(int i)
    {
        if (i % 2 == 0)
        {
            i++;
        }

        return i;
    }

    private IEnumerator GenerateRoom(Vector2 virtualLoc)
    {
        Debug.Log(virtualLoc);

        // Default variables
        bool lastRoom = false;
        bool mainRoom = false;
        bool northDoor = false;
        bool eastDoor = false;
        bool southDoor = false;
        bool westDoor = false;

        if (rooms.Count == maxRooms)
        {
            lastRoom = true;
        }
        else if (rooms.Count > maxRooms)
        {
            yield break;
        }

        // Create base gameobject
        GameObject roomGo = new GameObject("Room #" + (rooms.Count + 1));

        Room room = roomGo.AddComponent<Room>();
        room.myGenerator = this;
        room.virtualLoc = virtualLoc;
        room.Init();

        rooms.Add(virtualLoc, room);

        // Main room check
        if (rooms.Count == 0)
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

        // Generate root of room
        int sizeX = MakeOdd(Random.Range(minSize, maxSize));
        yield return new WaitForEndOfFrame();
        int sizeY = MakeOdd(Random.Range(minSize, sizeX));

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

        // Generate room
        for (int x = 0; x <  sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                // Create left wall
                if (x == 0 && leftWalls.Length > 0)
                {
                    GameObject wall = leftWalls[Random.Range(0, leftWalls.Length)];
                    Instantiate(wall, (Vector2)room.transform.position + new Vector2(x - 1, y), Quaternion.identity, room.transform);
                }

                // Create right wall
                if (x == (sizeX - 1) && rightWalls.Length > 0)
                {
                    GameObject wall = rightWalls[Random.Range(0, rightWalls.Length)];
                    Instantiate(wall, (Vector2)room.transform.position + new Vector2(x + 1, y), Quaternion.identity, room.transform);
                }

                // Create bottom wall
                if (y == 0 && bottomWalls.Length > 0)
                {
                    GameObject wall = bottomWalls[Random.Range(0, bottomWalls.Length)];
                    Instantiate(wall, (Vector2)room.transform.position + new Vector2(x, y - 1), Quaternion.identity, room.transform);
                }

                // Create top wall
                if (y == (sizeY - 1) && topWalls.Length > 0)
                {
                    GameObject wall = topWalls[Random.Range(0, topWalls.Length)];
                    Instantiate(wall, new Vector2(x, y + 1), Quaternion.identity, room.transform);
                }

                // Create bottom left corner
                if (x == 0 && y == 0 && cornerBottomLeft)
                {
                    Instantiate(cornerBottomLeft, new Vector2(x - 1, y - 1), Quaternion.identity, room.transform);
                }

                // Create bottom right corner
                if (x == (sizeX - 1)  && y == 0 && cornerBottomRight)
                {
                    Instantiate(cornerBottomRight, new Vector2(x + 1, y - 1), Quaternion.identity, room.transform);
                }

                // Create top left corner
                if (x == 0 && y == (sizeY - 1) && cornerTopLeft)
                {
                    Instantiate(cornerTopLeft, new Vector2(x - 1, y + 1), Quaternion.identity, room.transform);
                }

                // Create top right corner
                if (x == (sizeX - 1) && y == (sizeY - 1) && cornerTopRight)
                {
                    Instantiate(cornerTopRight, new Vector2(x + 1, y + 1), Quaternion.identity, room.transform);
                }

                // Left door generation
                if (x == 0 && y == (int)Mathf.Floor(sizeY / 2) && westDoor)
                {
                    Instantiate(normalDoorLeft, new Vector2(x - 1, y), Quaternion.identity, room.transform);
                }

                // Right door generation
                if (x == (sizeX - 1) && y == (int)Mathf.Floor(sizeY / 2) && eastDoor)
                {
                    Instantiate(normalDoorRight, new Vector2(x + 1, y), Quaternion.identity, room.transform);
                }

                // Top door generation
                if (y == (sizeY - 1) && x == (int)Mathf.Floor(sizeX / 2) && northDoor)
                {
                    Instantiate(normalDoorTop, new Vector2(x, y + 1), Quaternion.identity, room.transform);
                }

                // Bottom door generation
                if (y == 0 && x == (int)Mathf.Floor(sizeX / 2) && southDoor)
                {
                    Instantiate(normalDoorBottom, new Vector2(x, y - 1), Quaternion.identity, room.transform);
                }

                // Pick random tile
                GameObject tile = floorTiles[Random.Range(0, floorTiles.Length)];
                   
                // Create floor tile
                Instantiate(tile, new Vector2(x, y), Quaternion.identity, room.transform);
            }
        }

        // Check if it is the start room
        if (mainRoom)
        {
            // Set camera focus on this room
            camera.target = room.transform;

            // Set offset to focus on the middle of the room
            camera.offset = new Vector2(sizeX / 2, sizeY / 2);
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
                    StartCoroutine(GenerateRoom(loc));
                }
            }

            if (eastDoor)
            {
                Vector2 loc = new Vector2(virtualLoc.x + 1, virtualLoc.y);

                if (!rooms.ContainsKey(loc))
                {
                    StartCoroutine(GenerateRoom(loc));
                }
            }

            if (southDoor)
            {
                Vector2 loc = new Vector2(virtualLoc.x, virtualLoc.y - 1);

                if (!rooms.ContainsKey(loc))
                {
                    StartCoroutine(GenerateRoom(loc));
                }
            }

            if (westDoor)
            {
                Vector2 loc = new Vector2(virtualLoc.x - 1, virtualLoc.y);

                if (!rooms.ContainsKey(loc))
                {
                    StartCoroutine(GenerateRoom(loc));
                }
            }
        }
    }
}
