using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiceHandManager : MonoBehaviour // this script oversees the roll dice objects 
{

    [Header("Refrences")]
    public GameObject turnPlayer;

    [Header("Dice Rolling System")]
    public List<GameObject> RolledDie = new List<GameObject>();
    int DiceNeeded; // a varible to track how many dice we need to actually show and enable based on how many dice we can draw from the pool.
    public string desiredDiceState;
    public int inspectedDie;

    public void Start()
    {
        desiredDiceState = "Hidden";
        diceShow();
    }

    public void diceShow() // this displays and hides the dice
    {
        for (int i = 0; i < RolledDie.Count; i++)
        {
            if (desiredDiceState == "Shown")
            {
                RolledDie[i].GetComponent<DiceScript>().amISeen = true;
                RolledDie[i].GetComponent<DiceScript>().amIDisplayed();
            }
            if (desiredDiceState == "Hidden")
            {
                RolledDie[i].GetComponent<DiceScript>().amISeen = false;
                RolledDie[i].GetComponent<DiceScript>().amIDisplayed();
            }
        }
    }

    public void drawDice() 
    {
        // Check if the dice pool has remaning dice equal to or greater than the hand size.
        // Add dice up to hand size to the
        if (turnPlayer.GetComponent<Player>().diceDeck.Count >= turnPlayer.GetComponent<Player>().diceHandSize)
        {           
            DiceNeeded = turnPlayer.GetComponent<Player>().diceHandSize;
            for (int i = 0; i < DiceNeeded; i++)
            {
                int diceDrawn = Random.RandomRange(0, turnPlayer.GetComponent<Player>().diceDeck.Count); // pick a random dice out of the dice deck.
                RolledDie[i].GetComponent<DiceScript>().drawnDice.Add(turnPlayer.GetComponent<Player>().diceDeck[diceDrawn]); // Assigne it to the drawn dice object.
                turnPlayer.GetComponent<Player>().diceDeck.RemoveAt(diceDrawn); // remove it from the dice deck as it is currently in play. 
                RolledDie[i].GetComponent<DiceScript>().WhichDiceAmI = i; // set the what dice am I to the number in the list so we can remove it from the dice pool if needed.
                RolledDie[i].GetComponent<DiceScript>().diceDrawn();
            }
        }else if (turnPlayer.GetComponent<Player>().diceDeck.Count < turnPlayer.GetComponent<Player>().diceHandSize)
        {
            // if dice pool is less than hand size set diceneeded to dicepool.count;
            DiceNeeded = turnPlayer.GetComponent<Player>().diceDeck.Count;
            for (int i = 0; i < DiceNeeded; i++)
            {
                int diceDrawn = Random.RandomRange(0, turnPlayer.GetComponent<Player>().diceDeck.Count);
                RolledDie[i].GetComponent<DiceScript>().drawnDice.Add(turnPlayer.GetComponent<Player>().diceDeck[diceDrawn]);
                turnPlayer.GetComponent<Player>().diceDeck.RemoveAt(diceDrawn); 
                RolledDie[i].GetComponent<DiceScript>().WhichDiceAmI = i;
                RolledDie[i].GetComponent<DiceScript>().diceDrawn();
            }
        }
       
    }

    public void mulliganDice() // to mulligan we readd the dice to the deck, then clear the list then reroll the drawdice fuction.
    {
        for (int i = 0; i < DiceNeeded; i++)
        {
            turnPlayer.GetComponent<Player>().diceDeck.Add(RolledDie[i].GetComponent<DiceScript>().drawnDice[0]);
            RolledDie[i].GetComponent<DiceScript>().drawnDice.RemoveAt(0);
        }
    }

    public void rollDice()
    {
        for (int i = 0; i < DiceNeeded; i++)
        {
            RolledDie[i].GetComponent<DiceScript>().RollMe();
        }

        checkRollResults();
    }

    public void checkRollResults()
    {
        int level1Crests = 0;
        int level2Crests = 0; 
        int level3Crests = 0; 
        int level4Crests = 0; 

        for (int i = 0; i < DiceNeeded; i++) // ASK EVERY dice what they are.
        {
            switch (RolledDie[i].GetComponent<DiceScript>().myResult) // Add 1 point to each non level crest resource for every landed crest.
            {
                case "Move":
                    turnPlayer.GetComponent<Player>().moveCrestPoints += 1;
                    break;
                case "Attack":
                    turnPlayer.GetComponent<Player>().attackCrestPoints += 1;
                    break;
                case "Defence":
                    turnPlayer.GetComponent<Player>().defenceCrestPoints += 1;
                    break;
                case "AP":
                    turnPlayer.GetComponent<Player>().abiltyPowerCrestPoints += 1;
                    break;
                case "L1C": // add any level crest to the respective varible to track how many of each crest rolled.
                    level1Crests += 1;
                    break;
                case "L2C":
                    level2Crests += 1;
                    break;
                case "L3C":
                    level3Crests += 1;
                    break;
                 case "L4C":
                    level4Crests += 1;
                    break;
            }           
        }

        // If two or more of the same level crest are rolled tell those dice to become interactable.
        
        if (level1Crests >= 2)
        {
            for (int i = 0; i < DiceNeeded; i++)
            {
                if (RolledDie[i].GetComponent<DiceScript>().myResult == "L1C")
                {
                    RolledDie[i].GetComponent<DiceScript>().CanBeChosen = true;
                }
            }
        }

        if (level2Crests >= 2)
        {
            for (int i = 0; i < DiceNeeded; i++)
            {
                if (RolledDie[i].GetComponent<DiceScript>().myResult == "L2C")
                {
                    RolledDie[i].GetComponent<DiceScript>().CanBeChosen = true;
                }
            }
        }

        if (level3Crests >= 3)
        {
            for (int i = 0; i < DiceNeeded; i++)
            {
                if (RolledDie[i].GetComponent<DiceScript>().myResult == "L3C")
                {
                    RolledDie[i].GetComponent<DiceScript>().CanBeChosen = true;
                }
            }
        }

        if (level4Crests >= 4)
        {
            for (int i = 0; i < DiceNeeded; i++)
            {
                if (RolledDie[i].GetComponent<DiceScript>().myResult == "L4C")
                {
                    RolledDie[i].GetComponent<DiceScript>().CanBeChosen = true;
                }
            }
        }
        this.GetComponent<ResourceUIManager>().updateCrests();
    }

    public void CreatureAddedToPool()
    {
        RolledDie[inspectedDie].GetComponent<DiceScript>().drawnDice.RemoveAt(0); // remove the dice whose creature was added to the pool from play.
        returnToPool(); // run the function that returns the rest.
    }

    public void returnToPool()
    {
        for (int i = 0; i < DiceNeeded; i++)
        {
            if (RolledDie[i].GetComponent<DiceScript>().drawnDice.Count != 0)
            {
                turnPlayer.GetComponent<Player>().diceDeck.Add(RolledDie[i].GetComponent<DiceScript>().drawnDice[0]);
                RolledDie[i].GetComponent<DiceScript>().drawnDice.RemoveAt(0);
            }
        }

        for (int i = 0; i < RolledDie.Count; i++) // reset the die states
        {
            RolledDie[i].GetComponent<DiceScript>().CanBeChosen = false;
            RolledDie[i].GetComponent<DiceScript>().amISeen = false;
        }
    }
    
}
