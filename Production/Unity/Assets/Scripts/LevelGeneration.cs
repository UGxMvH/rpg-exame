using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    private List<GameObject> rooms = new List<GameObject>();

    [Header("Settings")]
    public int minSize;
    public int maxSize;
    public int maxRooms = 10;
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
        StartCoroutine(GenerateRoom(true, Vector2.zero));
    }

    private int MakeOdd(int i)
    {
        if (i % 2 == 0)
        {
            i++;
        }

        return i;
    }

    IEnumerator GenerateRoom(bool startRoom, Vector2 location)
    {
        // Default variables
        bool leftDoor = false;
        bool rightDoor = false;
        bool topDoor = false;
        bool bottomDoor = false;

        // Generate root of room
        GameObject room = new GameObject("Room #" + (rooms.Count + 1));
        room.transform.position = location;
        room.AddComponent<Room>();
        rooms.Add(room);

        // Datermine size
        yield return new WaitForEndOfFrame();
       
        int sizeX = MakeOdd(Random.Range(minSize, maxSize));

        yield return new WaitForEndOfFrame();

        int sizeY = MakeOdd(Random.Range(minSize, sizeX));

        // Datermine locations of doors
        // Left door check
        if (Random.Range(1, 100) % 2 == 0)
        {
            // Yes we want a top door
            leftDoor = true;
        }

        // Right door check
        if (Random.Range(1, 100) % 2 == 0)
        {
            // Yes we want a top door
            rightDoor = true;
        }

        // Top door check
        if (Random.Range(1, 100) % 2 == 0)
        {
            // Yes we want a top door
            topDoor = true;
        }

        // Bottom door check
        if (Random.Range(1, 100) % 2 == 0)
        {
            // Yes we want a top door
            bottomDoor = true;
        }

        // Generate room
        for (int x = 0; x < sizeX; x++)
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
                    Instantiate(wall, (Vector2)room.transform.position + new Vector2(x, y + 1), Quaternion.identity, room.transform);
                }

                // Create bottom left corner
                if (x == 0 && y == 0 && cornerBottomLeft)
                {
                    Instantiate(cornerBottomLeft, (Vector2)room.transform.position + new Vector2(x - 1, y - 1), Quaternion.identity, room.transform);
                }

                // Create bottom right corner
                if (x == (sizeX - 1)  && y == 0 && cornerBottomRight)
                {
                    Instantiate(cornerBottomRight, (Vector2)room.transform.position + new Vector2(x + 1, y - 1), Quaternion.identity, room.transform);
                }

                // Create top left corner
                if (x == 0 && y == (sizeY - 1) && cornerTopLeft)
                {
                    Instantiate(cornerTopLeft, (Vector2)room.transform.position + new Vector2(x - 1, y + 1), Quaternion.identity, room.transform);
                }

                // Create top right corner
                if (x == (sizeX - 1) && y == (sizeY - 1) && cornerTopRight)
                {
                    Instantiate(cornerTopRight, (Vector2)room.transform.position + new Vector2(x + 1, y + 1), Quaternion.identity, room.transform);
                }

                // Left door generation
                if (x == 0 && y == (int)Mathf.Floor(sizeY / 2) && leftDoor)
                {
                    Instantiate(normalDoorLeft, (Vector2)room.transform.position + new Vector2(x - 1, y), Quaternion.identity, room.transform);
                }

                // Right door generation
                if (x == (sizeX - 1) && y == (int)Mathf.Floor(sizeY / 2) && rightDoor)
                {
                    Instantiate(normalDoorRight, (Vector2)room.transform.position + new Vector2(x + 1, y), Quaternion.identity, room.transform);
                }

                // Top door generation
                if (y == (sizeY - 1) && x == (int)Mathf.Floor(sizeX/2) && topDoor)
                {
                    Instantiate(normalDoorTop, (Vector2)room.transform.position + new Vector2(x, y + 1), Quaternion.identity, room.transform);
                }

                // Bottom door generation
                if (y == 0 && x == (int)Mathf.Floor(sizeX / 2) && bottomDoor)
                {
                    Instantiate(normalDoorBottom, (Vector2)room.transform.position + new Vector2(x, y - 1), Quaternion.identity, room.transform);
                }

                // Pick random tile
                GameObject tile = floorTiles[Random.Range(0, floorTiles.Length)];
                   
                // Create floor tile
                Instantiate(tile, (Vector2)room.transform.position + new Vector2(x, y), Quaternion.identity, room.transform);
            }
        }

        // Check if it is the start room
        if (startRoom)
        {
            // Set camera focus on this room
            camera.target = room.transform;

            // Set offset to focus on the middle of the room
            camera.offset = new Vector2(sizeX / 2, sizeY / 2);
        }
    }
}
