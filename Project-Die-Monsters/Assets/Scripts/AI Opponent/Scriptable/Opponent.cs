using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Opponent : ScriptableObject
{
    public List<Die> OpponentDeck = new List<Die>(); // opponent deck list.

    public Behaviour behaviour;

    public enum Behaviour
    {
        Aggro, Defence
    }


    public int handSize = 4;
    public int LifePoints = 3;
}
