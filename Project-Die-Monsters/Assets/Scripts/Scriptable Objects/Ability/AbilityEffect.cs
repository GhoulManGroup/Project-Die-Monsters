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
    public EffectTargeting abilityTarget;
    public AllowedTargets allowedTargets;

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

    public enum AllowedTargets
    {
        self, friendly , hostile, all
    }


}
