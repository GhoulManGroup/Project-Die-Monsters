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

    public AbilityType abilityType; //The trigger condition of the ability.

    [Header("Ability Properties")]
    public int modifierValue;
    public TriggeredBy myTrigger;
    public ModifiedProperty statChanged;
    public StateReset whichState;
    
    [Header("Ability Target System")]
    public TargetType howITarget;
    public AllowedTargets allowedTargets;
    public int numberOfTargets;
   


    [Header("AI Stuff")]
    public float SpellPriority = 0; // How much importance the AI should place on casting this determiend by highest priority
    public enum AbilityType
    {
       Activated, Trigger, None
    }

    public enum TargetType
    {//Decleration pick a target on the board,Aoe = all possible targets within 
        Decleration, AOE, Random
    }

    public enum AllowedTargets
    {
        Self, Friend, Enemy, Both
    }

    //Triggered by this condition else its activated or no ability.
    public enum TriggeredBy
    {
        attacked, attacking, defending, defended, damaged, death, moved, none
    }
    //What catagory of ability it is state change stat modifier ect
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

