using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DieMonsters", menuName = "DieMonsters/AbilityEffect")]
public class AbilityEffect : ScriptableObject
{
    [Header("Ability Properties")]
    public EffectType effectType;
    public ModifiedProperty statChanged;
    public int modifierValue = 0;
    public StateReset stateChanged;

    [Header("Ability Target System")]
    public EffectTargeting howAbilityTarget;
    public AllowedTargets allowedTargets;
    public int requiredTargetCount;
    public int maximumTargetCount;
    [Space(10)]
    public AOEPosition AOEBoardPosition;
    public AOEDirections AOEDirection;
    public int distanceInDirection = 0;

    public enum EffectType
    {
        stateChange, modifier, none
    }

    public enum ModifiedProperty
    {
        healthHeal, healthDamage, defence, attack,  none
    }

    public enum StateReset
    {
        move, attack, useAbility, none
    }

    public enum EffectTargeting
    {
        areaOfEffect, declared, random
    }

    public enum AOEPosition
    {
        self, friendly, hostile, all, boardTile
    }

    public enum AOEDirections
    {
        front, frontSides, frontBack, sides, all
    }

    public enum AllowedTargets
    {
        self, friendly , hostile, all, otherTrigger
    }


}
