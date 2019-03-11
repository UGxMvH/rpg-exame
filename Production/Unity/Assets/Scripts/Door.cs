using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Room room;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        room.LeaveRoom(gameObject);
    }
}
