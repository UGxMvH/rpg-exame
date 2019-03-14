using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Door : MonoBehaviour
{
    private new SpriteRenderer renderer;

    public Room room;
    public Sprite[] doorAnimation;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterManager>() && !collision.gameObject.GetComponent<CharacterManager>().isAI && room.doorsOpen)
        {
            room.LeaveRoom(this);
        }
    }

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
