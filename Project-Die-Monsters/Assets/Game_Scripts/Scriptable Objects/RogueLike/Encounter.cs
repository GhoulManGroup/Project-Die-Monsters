using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "RogueLike/Encounter")]
public class Encounter : ScriptableObject
{
    public Opponent myEncounterOpponent;

    public EncounterType encounterType;

    public enum EncounterType
    {
       Start, Shop, Mystery, Easy, Medium, Hard, Boss
    }

}
