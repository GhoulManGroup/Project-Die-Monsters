using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public AbilityEffect effectToResolve;
    public bool targetsFound = false;

    // Start is called before the first frame update

    public IEnumerator ResolveEffect()
    {
        //Find target type & declare them here in a list.
        this.GetComponent<TargetManager>().currentEffect = effectToResolve;
        this.GetComponent<TargetManager>().FindTarget();

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

                        break;
                    case AbilityEffect.StateReset.move:
                       // switch (effectToResolve.abilityTarget)
                        
    
                        break;
                    case AbilityEffect.StateReset.useAbility:

                        break;
                }
                break;
            case AbilityEffect.EffectType.modifier:

                break;
            case AbilityEffect.EffectType.none:

                break;
        }

    }

}
