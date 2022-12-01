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

    public CreatureLevel creatureLevel;

    public Ability creatureAbility;

    //https://www.youtube.com/watch?v=bvRKfLPqQ0Q&t=551s Use this as a basis for an abiltiy system 2BD

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
}
