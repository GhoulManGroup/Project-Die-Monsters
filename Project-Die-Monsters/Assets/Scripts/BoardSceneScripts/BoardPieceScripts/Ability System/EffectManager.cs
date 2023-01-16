using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public AbilityEffect effectToResolve;
    public bool targetsChecked = false; //Has the target manager finished looking for the effects valid targets
    TargetManager targetFinder;
    // Start is called before the first frame update

    public void Awake()
    {
        targetFinder = this.GetComponent<TargetManager>();
    }
    public IEnumerator ResolveEffect()
    {
        //Find target type & declare them here in a list.
        this.GetComponent<TargetManager>().currentEffect = effectToResolve;
        this.GetComponent<TargetManager>().FindTarget(effectToResolve.allowedTargets.ToString());

        while(targetsChecked == false)
        {
            yield return null;
        }
        //If we are checking if the ability can be cast check for the last criteria to be met, else 
        if (this.GetComponent<AbilityManager>().checkingCanCast == true)
        {
            //checkCondition();
        }else if(this.GetComponent<AbilityManager>().checkingCanCast == false)
        {
            WhatEffect();
            this.GetComponent<AbilityManager>().effectResolved = true;
        }
        yield return null;
    }

    //This is applying the effect to the targets
    public void WhatEffect()
    {
        switch (effectToResolve.effectType) 
        {
            case AbilityEffect.EffectType.stateChange:
                switch (effectToResolve.stateChanged)
                {
                    case AbilityEffect.StateReset.attack:
                        for (int i = 0; i < targetFinder.foundTargets.Count; i++)
                        {
                            targetFinder.foundTargets[i].GetComponent<CreatureToken>().hasAttackedThisTurn = false;
                        }
                        break;
                    case AbilityEffect.StateReset.move:
                        for (int i = 0; i < targetFinder.foundTargets.Count; i++)
                        {
                            targetFinder.foundTargets[i].GetComponent<CreatureToken>().hasMovedThisTurn = false;
                        }                     
                        break;
                    case AbilityEffect.StateReset.useAbility:
                        for (int i = 0; i < targetFinder.foundTargets.Count; i++)
                        {
                            targetFinder.foundTargets[i].GetComponent<CreatureToken>().hasUsedAbilityThisTurn = false;
                        }
                        break;
                }
                break;
            case AbilityEffect.EffectType.modifier:

                break;
            case AbilityEffect.EffectType.none:

                break;
        }

    }

    public void ResetManager()
    {
        targetsChecked = false;
        effectToResolve = null;
    }



}
