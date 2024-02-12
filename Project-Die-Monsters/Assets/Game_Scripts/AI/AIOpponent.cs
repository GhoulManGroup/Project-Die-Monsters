using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOpponent : MonoBehaviour
{
    public Opponent myOpponent;
   
    [Header("Resources")]
    public int summmonCrestPoints = 0;
    public int moveCrestPoints = 0;
    public int attackCrestPoints = 0;
    public int defenceCrestPoints = 0;
    public int abiltyPowerCrestPoints = 0;

    //public List<Creature> CreaturePool = new List<Creature>();

    public List<Die> AIDiceDeck = new List<Die>();

    bool AssignedDice = false;

    public void SetUp()
    {
        if (AssignedDice == false)
        {
            //Debug.Log("Awake Add Dice To Die Deck");
            for (int i = 0; i < myOpponent.OpponentDeck.Count; i++)
            {
                AIDiceDeck.Add(myOpponent.OpponentDeck[i]);
            }
            AssignedDice = true;
        }

    }
}
