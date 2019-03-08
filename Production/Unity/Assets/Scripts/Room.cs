using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public LevelGeneration myGenerator;
    public Vector2 virtualLoc;

    public Room northRoom;
    public Room eastRoom;
    public Room southRoom;
    public Room westRoom;

    public GameObject walls;
    public GameObject floor;
    public GameObject traps;

    public bool doorsOpen = true;

    public void Init()
    {
        // Get neighbors
        GetNeighbors();

        CreateSubTransforms();
    }

    private void GetNeighbors()
    {
        Vector2 north = new Vector2(virtualLoc.x, virtualLoc.y + 1);
        Vector2 east = new Vector2(virtualLoc.x + 1, virtualLoc.y);
        Vector2 south = new Vector2(virtualLoc.x, virtualLoc.y - 1);
        Vector2 west = new Vector2(virtualLoc.x - 1, virtualLoc.y);

        if (myGenerator.rooms.ContainsKey(north))
        {
            northRoom = myGenerator.rooms[north];
        }

        if (myGenerator.rooms.ContainsKey(east))
        {
            eastRoom = myGenerator.rooms[east];
        }

        if (myGenerator.rooms.ContainsKey(south))
        {
            southRoom = myGenerator.rooms[south];
        }

        if (myGenerator.rooms.ContainsKey(west))
        {
            westRoom = myGenerator.rooms[west];
        }
    }

    private void CreateSubTransforms()
    {
        walls = new GameObject("Walls");
        walls.transform.SetParent(transform);

        floor = new GameObject("Floor");
        floor.transform.SetParent(transform);

        traps = new GameObject("Traps");
        traps.transform.SetParent(transform);
    }
}
