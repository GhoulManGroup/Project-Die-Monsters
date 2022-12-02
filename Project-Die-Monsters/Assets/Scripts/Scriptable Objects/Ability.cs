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
    public TriggerType myTrigger;

    public enum ModifiedProperty
    {
        health, defence, attack, none
    }

    public enum StateReset
    {
        move, attack, useAbility, none
    }

    public enum TriggerType
    {
        attacked, attacking, defending, defended, damaged, death, moved, none
    }
}

