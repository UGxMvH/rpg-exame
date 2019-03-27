using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Puzzle1Spike : MonoBehaviour
{
    public new SpriteRenderer renderer;
    public bool spikesOut;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        spikesOut = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!spikesOut)
        {
            return;
        }

        CharacterManager player = collision.gameObject.GetComponent<CharacterManager>();

        if (player && !player.isAI)
        {
            player.DoDamage(1000);
        }
    }
}
