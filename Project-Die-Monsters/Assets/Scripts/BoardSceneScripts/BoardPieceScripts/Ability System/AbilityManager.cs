using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour //This script will oversee the use of a creatures ability when called by either the trigger and or creature caller.
{
    public Ability myAbility;
    CreatureToken  myCreature;
    public bool canBeCast = false;
    public bool effectResolved = false;

    public void Awake()
    {
        myCreature = this.gameObject.GetComponent<CreatureToken>();
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
            Debug.Log("Effect Resolved");
            
        }
        Debug.Log("Ability Finished Resolving");
        this.GetComponent<CreatureToken>().hasUsedAbilityThisTurn = true;
        this.GetComponent<EffectManager>().ResetManager();
        this.GetComponent<TargetManager>().ResetManager();
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenAction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().OpenAndCloseControllerUI();
        effectResolved = false;
        yield return null;
    }
}
