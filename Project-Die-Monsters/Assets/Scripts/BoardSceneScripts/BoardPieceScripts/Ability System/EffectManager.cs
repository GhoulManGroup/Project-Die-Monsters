using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public AbilityEffect effectToResolve;
    // Start is called before the first frame update
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
                        switch (effectToResolve.abilityTarget)
                        {
                            case AbilityEffect.EffectTarget.self:

                                break;
                        }
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
