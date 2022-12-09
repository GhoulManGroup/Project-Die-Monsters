using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour //This script will oversee the use of a creatures ability when called by either the trigger and or creature caller.
{
    public Ability myAbility;
    CreatureToken  myCreature;
    public bool canBeCast = false;

    public List<GameObject> targetedCreatures = new List<GameObject>();

    public void Awake()
    {
        myCreature = this.gameObject.GetComponent<CreatureToken>();
        myAbility = myCreature.myCreature.myAbility;
    }

    public void DetermineIfAbilityCanBeCast()
    {// Like the pathfinding system before we allow the player the option to use their ability we must know if there would be a way for them to resolve that effect eg. 

    }

    //Identify what we are casting & its conditions
    //Apply those conditions & resolve its effect.
    public void ActivatedAbilityCast()
    {
        if (myAbility.howITarget == Ability.TargetType.Decleration)
        {

        }else if (myAbility.howITarget == Ability.TargetType.Random)
        {

        }else if (myAbility.howITarget == Ability.TargetType.AOE)
        {

        }
    }

    public void DeclareTargets()
    {

    }

    public void CheckForTrigger(string triggerType)
    {
        if (triggerType == myAbility.myTrigger.ToString())
        {
            Debug.Log("TriggerMatch, casting Ability");
        }
    }
}
