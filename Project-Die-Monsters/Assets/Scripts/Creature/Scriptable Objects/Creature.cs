using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Creature : ScriptableObject
{
    // these are what artwork shows up on the inspect card window.
    public Sprite CardArt;
    public Material cardArt3D;
    public Sprite TribeSprite;
    public Sprite LevelSprite;
    public Sprite TypeSprite;

    // contains the creatures basic health and such on summon.
    public string CreatureName;

    public int Health;
    public int Attack;
    public int Defence;

    public CreatureType creatureType;

    public CreatureColor creatureColor;

    public CreatureLevel creaturelevel;

    public enum CreatureColor
    {
        Red, Blue, Yellow, Green, White, Black
    }

    public enum CreatureType
    {
        Normal, Flying,
    }
    
    public enum CreatureLevel
    {
        One, Two, Three, Four
    }

    public enum AbilityType
    {
        Activate, Combative, Passive, None
            //Active is an ability which is used when the creature is selected on the board, Combat is one only avalible during the combat step , and passive is always in affect.
    }
}
