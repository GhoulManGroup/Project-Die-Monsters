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

    public enum AbilityActivatedHow
    {
        Activated, Trigger, None
    }

    [Header("Ability Components")]
    public List<AbilityEffect> abilityEffects = new List<AbilityEffect>();
    //public List<Targeting> targeting;

}
