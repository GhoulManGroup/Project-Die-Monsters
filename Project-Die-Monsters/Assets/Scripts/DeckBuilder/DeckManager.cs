using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour , IDataPersistence
{
    [Header("Player Collection Varibles")]
    public Collection dieLibary;
    public Collection playerCollection;
    public List<Deck> DeckSlots = new List<Deck>();

    [Header("Gameplay Deck Varibles")]
    public List<int> decksInPlay = new List<int>(); // when we choose a deck to use in freeplay or rogue like add it to the list so we can assign the contents to the approciate script once we reach the desired scene.



    public void setStartingCollection() // when starting a new game set the player collection to the first 15 items in the list.
    {
        for (int i = 0; i < 15; i++)
        {
            playerCollection.collectedDie.Add(dieLibary.collectedDie[i]);
        }
    }

    public void LoadData(GameData data)
    {
        playerCollection.collectedDie.Clear();
        DeckSlots[0].DeckDie.Clear();
        DeckSlots[1].DeckDie.Clear();
        DeckSlots[2].DeckDie.Clear();
        DeckSlots[3].DeckDie.Clear();
        DeckSlots[4].DeckDie.Clear();

        for (int i = 0; i < data.playerCollection.Count; i++)
        {
            playerCollection.collectedDie.Add(data.playerCollection[i]);
        }

        for (int i = 0; i < data.playerDeck01.Count; i++)
        {
            DeckSlots[0].DeckDie.Add(data.playerDeck01[i]);
        }

        for (int i = 0; i < data.playerDeck02.Count; i++)
        {
            DeckSlots[1].DeckDie.Add(data.playerDeck02[i]);
        }

        for (int i = 0; i < data.playerDeck03.Count; i++)
        {
            DeckSlots[2].DeckDie.Add(data.playerDeck03[i]);
        }

        for (int i = 0; i < data.playerDeck04.Count; i++)
        {
            DeckSlots[3].DeckDie.Add(data.playerDeck04[i]);
        }

        for (int i = 0; i < data.playerDeck05.Count; i++)
        {
            DeckSlots[4].DeckDie.Add(data.playerDeck05[i]);
        }

        if (data.newGameCheck == true)
        {
            setStartingCollection();
            data.newGameCheck = false;
            Debug.Log(data.newGameCheck);
        }
    }

    public void SaveData( ref GameData data)
    {
        // Clear all the die out of each of the 6 lists.
        data.playerCollection.Clear();
        data.playerDeck01.Clear();
        data.playerDeck02.Clear();
        data.playerDeck03.Clear();
        data.playerDeck04.Clear();
        data.playerDeck05.Clear();

        // then add the current die from the lists in the deck manager to be saved.
        for (int i = 0; i < playerCollection.collectedDie.Count; i++)
        {
            data.playerCollection.Add(playerCollection.collectedDie[i]);
        }

        for (int i = 0; i < DeckSlots[0].DeckDie.Count; i++)
        {
            data.playerDeck01.Add(DeckSlots[0].DeckDie[i]);
        }

        for (int i = 0; i < DeckSlots[1].DeckDie.Count; i++)
        {
            data.playerDeck02.Add(DeckSlots[1].DeckDie[i]);
        }

        for (int i = 0; i < DeckSlots[2].DeckDie.Count; i++)
        {
            data.playerDeck03.Add(DeckSlots[2].DeckDie[i]);
        }

        for (int i = 0; i < DeckSlots[3].DeckDie.Count; i++)
        {
            data.playerDeck04.Add(DeckSlots[3].DeckDie[i]);
        }
        for (int i = 0; i < DeckSlots[4].DeckDie.Count; i++)
        {
            data.playerDeck05.Add(DeckSlots[4].DeckDie[i]);
        }
    }

    

}
