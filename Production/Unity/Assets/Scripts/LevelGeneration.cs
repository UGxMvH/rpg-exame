using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    private List<GameObject> rooms = new List<GameObject>();

    [Header("Settings")]
    public int minSize;
    public int maxSize;
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
        StartCoroutine(GenerateRoom(true));
    }

    private int MakeOdd(int i)
    {
        if (i % 2 == 0)
        {
            i++;
        }

        return i;
    }

    IEnumerator GenerateRoom(bool startRoom)
    {
        // Generate root of room
        GameObject room = new GameObject("Room #" + (rooms.Count + 1));
        rooms.Add(room);

        // Datermine size
        yield return new WaitForEndOfFrame();
       
        int sizeX = MakeOdd(Random.Range(minSize, maxSize));

        yield return new WaitForEndOfFrame();

        int sizeY = MakeOdd(Random.Range(minSize, sizeX));

        // Generate room
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                // Create left wall
                if (x == 0 && leftWalls.Length > 0)
                {
                    GameObject wall = leftWalls[Random.Range(0, leftWalls.Length)];
                    Instantiate(wall, new Vector2(x - 1, y), Quaternion.identity, room.transform);
                }

                // Create right wall
                if (x == (sizeX - 1) && rightWalls.Length > 0)
                {
                    GameObject wall = rightWalls[Random.Range(0, rightWalls.Length)];
                    Instantiate(wall, new Vector2(x + 1, y), Quaternion.identity, room.transform);
                }

                // Create bottom wall
                if (y == 0 && bottomWalls.Length > 0)
                {
                    GameObject wall = bottomWalls[Random.Range(0, bottomWalls.Length)];
                    Instantiate(wall, new Vector2(x, y - 1), Quaternion.identity, room.transform);
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

                // Pick random tile
                GameObject tile = floorTiles[Random.Range(0, floorTiles.Length)];
                   
                // Create floor tile
                Instantiate(tile, new Vector2(x, y), Quaternion.identity, room.transform);
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
