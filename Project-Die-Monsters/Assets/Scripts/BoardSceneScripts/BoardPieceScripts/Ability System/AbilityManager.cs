using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour //This script will oversee the use of a creatures ability when called by either the trigger and or creature caller.
{
    public Ability myAbility;
    CreatureToken  myCreature;

    public bool checkingCanCast = false;
    float vaildEffects = 0;

    public bool canBeCast = false;

    public bool effectResolved = false;

    public void Awake()
    {
        myCreature = this.gameObject.GetComponent<CreatureToken>();
    }   

    public void CheckAbilityCanCast() // Check if ability can be cast by seeing if there are enough valid targets for its conditions
    {
        Debug.Log("Checking Ability");
        for (int i = 0; i < myAbility.abilityEffects.Count; i++)
        {
            this.GetComponent<EffectManager>().effectToResolve = myAbility.abilityEffects[i];
            this.GetComponent<EffectManager>().StartCoroutine("ResolveEffect");
        }

        if (vaildEffects == myAbility.abilityEffects.Count)
        {
            Debug.Log("All effects have valid targets to meet conditions");
            canBeCast = true;
        }else if (vaildEffects != myAbility.abilityEffects.Count)
        {
            Debug.Log("Cant Cast Ability valid effect targets not found");
        }
    }

    public IEnumerator ActivateEffect()
    {
        for (int i = 0; i < myAbility.abilityEffects.Count; i++)
        {
            effectResolved = false;
            this.GetComponent<EffectManager>().effectToResolve = myAbility.abilityEffects[i];
            this.GetComponent<EffectManager>().StartCoroutine("ResolveEffect");
            while (effectResolved == false)
            {
                yield return null;
            }          
        }
        this.GetComponent<CreatureToken>().hasUsedAbilityThisTurn = true;
        this.GetComponent<EffectManager>().ResetManager();
        this.GetComponent<TargetManager>().ResetManager();
        canBeCast = false;
        vaildEffects = 0;
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenAction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().OpenAndCloseControllerUI();
        effectResolved = false;
        yield return null;
    }
}
