using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Opponent : ScriptableObject
{// This class contains 
    [Header("Starting Settings")]
    public string nameOfDungeonLord = "AI Test";
    public int dungeonLordLife = 3;

    public List<Die> OpponentDeck = new List<Die>(); // opponent deck list.

    [Header("Personality")] // What weights are applied to decions
    public Behaviour behaviour;

    public enum Behaviour
    {
        Aggro, Defence
    }

}
