using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DieMonsters", menuName = "DieMonsters/DungeonLord")]
public class DungeonLord : ScriptableObject
{
    public Sprite CardArt;
    public string dungeonLordName;
    public int dungeonLordHealth;
}
