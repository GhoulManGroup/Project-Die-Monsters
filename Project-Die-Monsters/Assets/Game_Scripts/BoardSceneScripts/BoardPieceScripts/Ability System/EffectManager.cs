using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EffectManager : MonoBehaviour
{ //This script handles the application of the effect to the target creatures.
    public AbilityEffect effectToResolve;
    public bool targetsChecked = false; //Has the target manager finished looking for the effects valid targets
    TargetManager targetFinder;

    public void Awake()
    {
        targetFinder = this.GetComponent<TargetManager>();
    }

    #region AbilityCastingCode
    public IEnumerator PrepareAndCastEffect()
    {
        targetsChecked = false;
        //Call target manager and have it find the effects targets then inform the ability manager the effect is ready!
        this.GetComponent<TargetManager>().currentEffect = effectToResolve;
        this.GetComponent<TargetManager>().FindTarget();

        while(targetsChecked == false)
        {
            yield return null;
        }
  
        // Store Local List Here untill All are effects have their targets and the ability is resolved or cancled.
        List<GameObject> EffectTargets = new List<GameObject>(targetFinder.foundTargets);
        this.GetComponent<AbilityManager>().readyEffects += 1;
 

        while (this.GetComponent<AbilityManager>().abilityCast == false)
        {
            yield return null;
        }
        ApplyWhichEffect(EffectTargets);
        this.GetComponent<AbilityManager>().effectsResolved += 1;
        yield return null;
    }

    //This is applying the effect to the targets
    public void ApplyWhichEffect(List<GameObject> targets)
    {
        switch (effectToResolve.effectType) 
        {
            case AbilityEffect.EffectType.stateChange:
                switch (effectToResolve.stateChanged)
                {
                    case AbilityEffect.StateReset.attack:
                        for (int i = 0; i < targets.Count; i++)
                        {
                            targets[i].GetComponent<CreatureToken>().hasAttackedThisTurn = false;
                        }
                        break;
                    case AbilityEffect.StateReset.move:
                        for (int i = 0; i < targets.Count; i++)
                        {
                            targets[i].GetComponent<CreatureToken>().hasMovedThisTurn = false;
                        }                     
                        break;
                    case AbilityEffect.StateReset.useAbility:
                        for (int i = 0; i < targets.Count; i++)
                        {
                            targets[i].GetComponent<CreatureToken>().hasUsedAbilityThisTurn = false;
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
    #endregion

    public void ResetManager()
    {
        targetsChecked = false;
        effectToResolve = null;
        StopAllCoroutines();
    }

    #region CheckCanBeCastCode
    public void EffectChecking()
    {

    }
    #endregion

}
