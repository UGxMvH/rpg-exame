using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Private variables
    private List<CharacterManager> enemies = new List<CharacterManager>();

    private Door northDoor;
    private Door eastDoor;
    private Door southDoor;
    private Door westDoor;

    // Public variables
    public bool containsEnemies;
    public bool isShopRoom;
    public bool isLastRoom;
    public bool isPuzzle1;

    public LevelManager myGenerator;
    public Vector2 virtualLoc;
    public Vector2 size;

    public Room northRoom;
    public Room eastRoom;
    public Room southRoom;
    public Room westRoom;

    public Transform walls;
    public Transform doors;
    public Transform floor;
    public Transform traps;

    public bool doorsOpen;

    public void Init()
    {
        CreateSubTransforms();
    }

    // Level generated now finalize rooms
    public void Finish()
    {
        if (isShopRoom)
        {
            ReplaceWithShop();
        }

        if (isPuzzle1)
        {
            ReplaceWithPuzzle1();
        }

        GetNeighbors();
        CreateDoors();
        SetUpRoom();
    }

    private void ReplaceWithShop()
    {
        // Remove all childs
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        // Add shop room
        Transform trans = Instantiate(myGenerator.shop, transform).transform;

        walls = trans.GetChild(0);
        doors = trans.GetChild(1);
        floor = trans.GetChild(2);
        traps = trans.GetChild(3);

        size = new Vector2(11, 9);
    }

    private void ReplaceWithPuzzle1()
    {
        // Remove all childs
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        // Add shop room
        Transform trans = Instantiate(myGenerator.puzzle1, transform).transform;

        walls = trans.GetChild(0);
        doors = trans.GetChild(1);
        floor = trans.GetChild(2);
        traps = trans.GetChild(3);

        size = new Vector2(9, 7);

        // Fix camera
        SmoothCamera camera = Camera.main.GetComponent<SmoothCamera>();
        camera.offset = new Vector2(Mathf.Floor(size.x / 2), Mathf.Floor(size.y / 2));
        camera.GetComponent<Camera>().orthographicSize = size.y / 2 + 2;
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

    private void CreateDoors()
    {
        // North door generation
        if (northRoom)
        {
            northDoor = Instantiate(myGenerator.normalDoorTop, new Vector2(Mathf.Floor(size.x / 2), size.y), Quaternion.identity, doors.transform).GetComponent<Door>();
            northDoor.room = this;
        }

        if (eastRoom)
        {
            eastDoor = Instantiate(myGenerator.normalDoorRight, new Vector2(size.x, Mathf.Floor(size.y / 2)), Quaternion.identity, doors.transform).GetComponent<Door>();
            eastDoor.room = this;
        }

        if (southRoom)
        {
            southDoor = Instantiate(myGenerator.normalDoorBottom, new Vector2(Mathf.Floor(size.x / 2), -1), Quaternion.identity, doors.transform).GetComponent<Door>();
            southDoor.room = this;
        }

        if (westRoom)
        {
            westDoor = Instantiate(myGenerator.normalDoorLeft, new Vector2(-1, Mathf.Floor(size.y / 2)), Quaternion.identity, doors.transform).GetComponent<Door>();
            westDoor.room = this;
        }
    }

    private void SetUpRoom()
    {
        if (containsEnemies)
        {
            // Lets calculate how many
            int min = 2;
            int max = (int)Mathf.Floor(size.x - 3);

            if (max <= min)
            {
                max = min + 1;
            }

            int amount = Random.Range(min, max);

            for (int i = 0; i < amount; i++)
            {
                // create enemy in room
                GameObject prefab = myGenerator.enemies[Random.Range(0, myGenerator.enemies.Length)];

                // Choose location
                Vector2 pos = new Vector2(Random.Range(2, size.x - 3), Random.Range(2, size.y - 3));

                CharacterManager enemy = Instantiate(prefab, pos, Quaternion.identity, transform).GetComponent<CharacterManager>();

                if (enemy)
                {
                    enemy.AiRoom = this;

                    enemies.Add(enemy);
                }
            }
        }

        if (isLastRoom)
        {
            Door door = null;
            // This is the last door so we need to create an exit door
            if (!northDoor)
            {
                northDoor = door = Instantiate(myGenerator.normalDoorTop, new Vector2(Mathf.Floor(size.x / 2), size.y), Quaternion.identity, doors.transform).GetComponent<Door>();

                Instantiate(myGenerator.topIcon, new Vector2(Mathf.Floor(size.x / 2), size.y), Quaternion.identity, doors.transform);
            }
            else if (!eastDoor)
            {
                eastDoor = door = Instantiate(myGenerator.normalDoorRight, new Vector2(size.x, Mathf.Floor(size.y / 2)), Quaternion.identity, doors.transform).GetComponent<Door>();

                Instantiate(myGenerator.rightIcon, new Vector2(size.x, Mathf.Floor(size.y / 2)), Quaternion.identity, doors.transform);
            }
            else if (!southDoor)
            {
                southDoor = door = Instantiate(myGenerator.normalDoorBottom, new Vector2(Mathf.Floor(size.x / 2), -1), Quaternion.identity, doors.transform).GetComponent<Door>();

                Instantiate(myGenerator.bottomIcon, new Vector2(Mathf.Floor(size.x / 2), -1), Quaternion.identity, doors.transform);
            }
            else if (!westDoor)
            {
                westDoor = door = Instantiate(myGenerator.normalDoorLeft, new Vector2(-1, Mathf.Floor(size.y / 2)), Quaternion.identity, doors.transform).GetComponent<Door>();

                Instantiate(myGenerator.leftIcon, new Vector2(-1, Mathf.Floor(size.y / 2)), Quaternion.identity, doors.transform);
            }
            else
            {
                // Every position is taken so just take the north door
                Destroy(northDoor.gameObject);

                northDoor = door = Instantiate(myGenerator.normalDoorTop, new Vector2(Mathf.Floor(size.x / 2), size.y), Quaternion.identity, doors.transform).GetComponent<Door>();

                Instantiate(myGenerator.topIcon, new Vector2(Mathf.Floor(size.x / 2), size.y), Quaternion.identity, doors.transform);
            }

            // Set-up door
            door.finishDoor = true;
            door.room = this;
        }
    }

    private void CreateSubTransforms()
    {
        walls = new GameObject("Walls").transform;
        walls.SetParent(transform);

        doors = new GameObject("Doors").transform;
        doors.SetParent(transform);

        floor = new GameObject("Floor").transform;
        floor.SetParent(transform);

        traps = new GameObject("Traps").transform;
        traps.SetParent(transform);
    }

    public void EnteredRoom()
    {
        // If there are no enemies open doors!
        if (enemies.Count == 0 && !doorsOpen)
        {
            OpenDoors();
        }
    }

    public void LeaveRoom(Door door)
    {
        Room newRoom = null;

        // Find which room
        if (door == northDoor)
        {
            newRoom = northRoom;
        }
        else if (door == eastDoor)
        {
            newRoom = eastRoom;
        }
        else if (door == southDoor)
        {
            newRoom = southRoom;
        }
        else if (door == westDoor)
        {
            newRoom = westRoom;
        }

        // Move to new room
        myGenerator.TransistRoom(this, newRoom);
    }

    public void OpenDoors()
    {
        if (doorsOpen)
        {
            return;
        }

        StartCoroutine(UnlockAlldoors());
    }

    public void EnemyDied(CharacterManager enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            OpenDoors();
        }
    }

    private IEnumerator UnlockAlldoors()
    {
        if (northDoor)
        {
            StartCoroutine(northDoor.AnimateOpen());
        }

        if (eastDoor)
        {
            StartCoroutine(eastDoor.AnimateOpen());
        }

        if (southDoor)
        {
            StartCoroutine(southDoor.AnimateOpen());
        }

        if (westDoor)
        {
            StartCoroutine(westDoor.AnimateOpen());
        }

        yield return new WaitForSeconds(.3f);

        doorsOpen = true;
    }
}
