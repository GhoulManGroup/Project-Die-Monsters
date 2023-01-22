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
    public EffectTargeting abilityTarget;
    public AOEDirections AOEDirection;
    public AllowedTargets allowedTargets;

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
        areaOfEffect,   declared
    }

    public enum AOEDirections
    {
        forward, frontSides, frontBack, all
    }

    public enum AllowedTargets
    {
        self, friendly , hostile, all
    }


}
