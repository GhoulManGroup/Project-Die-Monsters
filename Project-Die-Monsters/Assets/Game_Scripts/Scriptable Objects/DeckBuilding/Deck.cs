using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckCollection", menuName = "DeckCollection/DiceDeck")]
public class Deck : ScriptableObject
{

    public enum mySlot
    {
        one, two, three, four, five
    }

    public List<Die> DeckDie = new List<Die>();

    public int DeckSizeMin = 12;
    public int DeckSizeCap = 15;
}
