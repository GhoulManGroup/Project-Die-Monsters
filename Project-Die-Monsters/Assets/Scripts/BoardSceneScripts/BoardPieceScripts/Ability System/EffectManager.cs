using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public AbilityEffect effectToResolve;
    public bool targetsFound = false;
    TargetManager targetFinder;
    // Start is called before the first frame update

    public void Awake()
    {
        targetFinder = this.GetComponent<TargetManager>();
    }
    public IEnumerator ResolveEffect()
    {
        //Find target type & declare them here in a list.
        Debug.Log("fIND tARGETS");
        this.GetComponent<TargetManager>().currentEffect = effectToResolve;
        this.GetComponent<TargetManager>().FindTarget(effectToResolve.allowedTargets.ToString());

        while(targetsFound == false)
        {
            yield return null;
        }
        Debug.Log("Finding Effect");
        WhatEffect();
        this.GetComponent<AbilityManager>().effectResolved = true;
        yield return null;
    }



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
        targetsFound = false;
        effectToResolve = null;
    }



}
