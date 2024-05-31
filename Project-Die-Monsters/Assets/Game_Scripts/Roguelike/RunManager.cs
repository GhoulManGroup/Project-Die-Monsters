 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{

    [Header("Run Session")]
    public Deck runDeck;
    public int runProgress = 1; //1-5;

    private void Start()
    {
        // set deck.
       // runDeck = GameManagerScript.instance.deckManager.DeckSlots[GameManagerScript.instance.deckManager.decksInPlay[0]];
        runProgress = 1;
        //Spawn Level .. Spawner Scriptable Object //

        //When Player Selects Level Node > Hide Everything but keep it running I guess:>

    }


    // We need to remake this script
    //Store the deck that is being used in this run copied from what ever deck is used by the player from main menu, Its copied as me modify it here.
    // Track The Player Progress Through the Run
}

