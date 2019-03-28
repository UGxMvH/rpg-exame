using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public CharacterManager player;
    public float movementX, movementY;

    public void OnRelease()
    {
        player.horizontal = 0;
        player.vertical = 0;
    }

    public void OnPress()
    {
        player.usingMobile = true;
        player.horizontal = movementX;
        player.vertical = movementY;
    }

    public void Attack()
    {
        if (!player.isAttacking)
        {
            player.isAttacking = true;
            StartCoroutine(player.PlayerAttack());
        }
    }
}
