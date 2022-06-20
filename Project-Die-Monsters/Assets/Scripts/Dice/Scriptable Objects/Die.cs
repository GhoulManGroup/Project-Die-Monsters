using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Die : ScriptableObject
{
    [Header("Dice Properties")]
    public DieColor dieColor;
    public DieCreatureLevel dieCreatureLevel;
    public DiceContents diceContents;

    public FirstCrest firstCrest;
    public SecondCrest secondCrest;
    public ThirdCrest thirdCrest;
    public ForthCrest forthCrest;
    public FifthCrest fifthCrest;
    public SixthCrest sixthCrest;

    [Header("Dice Contents")]
    public Creature dieCreature;
    public int dieID; // which unique number each die has to check if it exists already in any list.

    [Header("Dice State")]
    public bool dieInDeck = false; // this boolean will prevent a die from being reused once it is in play.
    public Sprite Icon;
    



    public enum DieColor
    {
        Red, Blue, Yellow, Green, White, Black
    }

    public enum DieCreatureLevel
    {
        one, two, three, four // from this value we now know how many dice need to be each type of crest.
    }

    public enum DiceContents
    {
        creature, item,
    }

    public enum FirstCrest
    {
        Level, Move, Attack, Defence, AP
    }

    public enum SecondCrest
    {
        Level, Move, Attack, Defence, AP
    }

    public enum ThirdCrest
    {
        Level, Move, Attack, Defence, AP
    }

    public enum ForthCrest
    {
        Level, Move, Attack, Defence, AP
    }

    public enum FifthCrest
    {
        Level, Move, Attack, Defence, AP
    }

    public enum SixthCrest
    {
        Level, Move, Attack, Defence, AP
    }

}
