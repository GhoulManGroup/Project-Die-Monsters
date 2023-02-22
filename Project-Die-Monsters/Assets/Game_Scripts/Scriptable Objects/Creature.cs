using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class Creature : ScriptableObject
{
    [Header("Crature Identifyers")]
    public int creatureID = 0;
    public string CreatureName;
    public CreatureType creatureType;
    public CreatureColor creatureColor;
    public CreatureLevel creatureLevel;

    [Header("Creature Assets&Materials")]
    public Sprite CardArt;
    public Material cardArt3D;
    public Sprite TribeSprite;
    public Sprite LevelSprite;
    public Sprite TypeSprite;

    [Header("Creature Properties")]
    public int summonCost;
    public int Health;
    public int Attack;
    public int Defence;

    public Ability myAbility;

    public enum CreatureColor
    {
        Red, Yellow, Blue, Orange, Green, Purple, White
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
