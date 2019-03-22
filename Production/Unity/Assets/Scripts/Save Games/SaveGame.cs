
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGame
{
    public string title;

    public float health;

    public int coins;
    public int maxHealth;
    public int damage;
    public int progress;

    public bool finishedLvl1;
    public bool finisehedLvl2;
    public bool finishedLvl3;
    public bool finishedBoss;
}
