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
    public AbilityActivatedHow abilityActivatedHow; //The trigger condition of the ability.
    public TriggeredBy myTrigger;

    [Header("Effect Values")]
    public AbilityEffect[] myEffects;
    public int[] modifierValue;



    [Header("Ability Target System")]
    public TargetType howITarget;
    public TargetPoint myTargetPoint;
    public TargetAmmount targetAmmount;
    public AllowedTargets allowedTargets;
    public int numberOfTargets;
   
    [Header("AI Stuff")]
    public float SpellPriority = 0; // How much importance the AI should place on casting this determiend by highest priority

    public enum AbilityActivatedHow
    {
       Activated, Trigger, None
    }

    public enum TriggeredBy
    {
        attacked, attacking, defending, defended, damaged, death, moved, none
    }

    public enum TargetType
    {//Decleration pick a target on the board,Aoe = all possible targets within 
        Decleration, AOE, Random
    }

    public enum TargetPoint
    {//Where AOE effects occur.
        self, otherCreature, tile
    }

    public enum TargetAmmount
    {//How many targets up to numberOFTargetInt we must effect.
        some, all
    }

    public enum AllowedTargets
    {
        Self, Friend, Enemy, Both
    }

    //Triggered by this condition else its activated or no ability.

    //What catagory of ability it is state change stat modifier ect






}

