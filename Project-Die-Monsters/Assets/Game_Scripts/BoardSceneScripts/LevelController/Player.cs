using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Limits")]
    public int dicePool = 15; // how many dice in the player deck.
    public int diceHandSize = 3; // how many dice are rolled at once.
    public int playerLifeLimit = 3; // What health the player dungeon lord starts with

    [Header("Player Deck")] //Contains the remaning game dice in the players dice deck.
    public List<Die> diceDeck = new List<Die>();

    [Header("PlayerCreaturePool")] //Contains the current list of creatures in their pool to spawn.
    public List<Creature> CreaturePool = new List<Creature>();

    [Header("Player Resources")] // these are the resources the player has gained from rolling dice.
    public int summmonCrestPoints = 0;
    //public int moveCrestPoints = 0;
    public int attackCrestPoints = 0;
    public int defenceCrestPoints = 0;
    public int abiltyPowerCrestPoints = 0;

    [Header("Player ID")]
    public GameObject thisObject;
    public string myName;

    public void Awake()
    {
        thisObject = this.gameObject;

    }
}
