using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    #region Public Variables
    public CharacterManager player;
    public float movementX, movementY;
    #endregion

    /*
     * Release Mobile input button
     */
    public void OnRelease()
    {
        player.horizontal = 0;
        player.vertical = 0;
    }

    /*
     * Press Mobile input button
     */
    public void OnPress()
    {
        player.usingMobile = true;
        player.horizontal = movementX;
        player.vertical = movementY;
    }

    /*
     * Pressed attack button
     */
    public void Attack()
    {
        if (!player.isAttacking)
        {
            player.isAttacking = true;
            StartCoroutine(player.PlayerAttack());
        }
    }
}
