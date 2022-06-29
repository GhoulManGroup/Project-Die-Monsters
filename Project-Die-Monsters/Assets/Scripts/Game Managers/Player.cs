using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Limits")]
    public int dicePool = 15; // how many dice in the player deck.
    public int diceHandSize = 3; // how many dice are rolled at once.
    public int playerLifeLimit = 3; // What health the player dungeon lord starts with

    [Header("Player Deck")]
    public List<Die> diceDeck = new List<Die>();

    [Header("PlayerCreaturePool")]
    public List<Creature> CreaturePool = new List<Creature>();

    [Header("Player Resources")] // these are the resources the player has gained from rolling dice.
    public int moveCrestPoints = 0;
    public int attackCrestPoints = 0;
    public int defenceCrestPoints = 0;
    public int abiltyPowerCrestPoints = 0;

}
