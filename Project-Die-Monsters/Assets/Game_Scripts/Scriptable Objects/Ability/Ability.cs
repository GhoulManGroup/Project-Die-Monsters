using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DieMonsters", menuName = "DieMonsters/Ability")]
public class Ability : ScriptableObject
{
    [Header("Ability Details")]
    public string AbilityName;
    public string AbilityDescriptionText;
    public int abilityCost;
    public AbilityActivatedHow abilityActivatedHow;
    public TriggerCondition howTriggered;

    public enum AbilityActivatedHow
    {
        None, Activated, Trigger
    }

    public enum TriggerCondition
    {
        None, OnDeath, OnKill, OnHit, OnHitAbility, OnAttack, OnEndTurn
    }

    [Header("Ability Components")]
    public List<AbilityEffect> abilityEffects = new List<AbilityEffect>();

}

