using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreatureAbilitys", menuName = "CreatureAbilityComponents/Ability")]
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
        Activated, Trigger, None
    }

    public enum TriggerCondition
    {
        onDeath, onKill, onHit, onHitAbility, onAttack, onEndTurn
    }

    [Header("Ability Components")]
    public List<AbilityEffect> abilityEffects = new List<AbilityEffect>();
    //public List<Targeting> targeting;

}

