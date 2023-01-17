using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
   // THIS SCRIPT IS TO FIND ALL POSSIBLE TARGETS NOT FILTER IF THEY ARE SUTIABLE FOR THE EFFECT.

    public AbilityEffect currentEffect;
    public List<GameObject> foundTargets = new List<GameObject>();
    public void FindTarget()
    {

        switch (currentEffect.abilityTarget)
        {
            case AbilityEffect.EffectTargeting.areaOfEffect:

                break;
            case AbilityEffect.EffectTargeting.declared:

                switch (currentEffect.allowedTargets)
                {
                    case AbilityEffect.AllowedTargets.self:
                        foundTargets.Add(this.gameObject);
                        this.GetComponent<EffectManager>().targetsChecked = true;
                        break;

                    case AbilityEffect.AllowedTargets.friendly:

                        break;
                    case AbilityEffect.AllowedTargets.hostile:

                        break;
                    case AbilityEffect.AllowedTargets.all:

                        break;

                }
                break;
        }
    }

    IEnumerator DeclaringTargets()
    {
        bool hasDeclared = false;
        //Open Declare UI
        while (hasDeclared == false)
        {

            yield return null;
        }
        yield return null;
    }


    public void ResetManager()
    {
        currentEffect = null;
        foundTargets.Clear();
    }
}
