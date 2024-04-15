using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DieMonsters", menuName = "DieMonsters/Encounter")]
public class Encounter : ScriptableObject
{
    public DungeonLord myEncounterOpponent;

    public EncounterType encounterType;

    public enum EncounterType
    {
        Shop, Mystery, Easy, Medium, Hard, Boss
    }

}
