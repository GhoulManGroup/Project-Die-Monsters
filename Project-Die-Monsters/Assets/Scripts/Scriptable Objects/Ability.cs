using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ability : ScriptableObject
{
    [Header("Ability Details")]
    public string nameOfAbility;
    public string abilityDescription;
    public int abilityCost;

    [Header("Ability Properties")]
    public int modifierValue;
    
    public ModifiedProperty statChanged;
    public StateReset whichState;
    public TriggeredBy myTrigger;
    public AbilityType abilityType; //The trigger condition of the ability.
    public enum AbilityType
    {
       Activated, Trigger, None
    }
    public enum TriggeredBy
    {
        attacked, attacking, defending, defended, damaged, death, moved, none
    }

    public enum MyEffect
    {
        stateChange, modifier, none
    }

    public enum ModifiedProperty
    {
        health, defence, attack, none
    }

    public enum StateReset
    {
        move, attack, useAbility, none
    }

}

