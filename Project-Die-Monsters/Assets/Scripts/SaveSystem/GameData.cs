using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // https://www.youtube.com/watch?v=aUi9aijvpgs&ab_channel=TreverMock SOURCE CODE

    // Player Collection list.
    public List<Die> playerCollection = new List<Die>();

    //Player Die Deck Lists.
    public List<Die> playerDeck01 = new List<Die>();
    public List<Die> playerDeck02 = new List<Die>();
    public List<Die> playerDeck03 = new List<Die>();
    public List<Die> playerDeck04 = new List<Die>();
    public List<Die> playerDeck05 = new List<Die>();

    // as our loadfunction in deckmanager is called after new game is called in datapersistance this bool will act as a check to allow us to run the set player collection function after loading rather than start.
    public bool newGameCheck = false;

    // What is contained here will be the default values if no save data can be found.
    public GameData() 
    {
        newGameCheck = true;
    }
}
