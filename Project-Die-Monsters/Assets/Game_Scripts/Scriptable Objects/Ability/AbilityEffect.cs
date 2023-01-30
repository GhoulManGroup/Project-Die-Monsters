using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureAbilitys", menuName = "CreatureAbilityComponents/AbilityEffects")]
public class AbilityEffect : ScriptableObject
{
    [Header("Ability Properties")]
    public EffectType effectType;
    public ModifiedProperty statChanged;
    public int modifierValue = 0;
    public StateReset stateChanged;

    [Header("Ability Target System")]
    public EffectTargeting howAbilityTarget;
    public AOEDirections AOEDirection;
    public AOEPosition AOEBoardPosition;
    public AllowedTargets allowedTargets;
    public int distanceInDirection = 0;

    public int requiredTargetCount;
    public int maximumTargetCount;

    public enum EffectType
    {
        stateChange, modifier, none
    }

    public enum ModifiedProperty
    {
        health, defence, attack,  none
    }

    public enum StateReset
    {
        move, attack, useAbility, none
    }

    public enum EffectTargeting
    {
        areaOfEffect,   declared , random
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
        self, friendly , hostile, all
    }


}
