using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    //public void determineTa

    public AbilityEffect currentEffect;
    public List<GameObject> foundTargets = new List<GameObject>();
    public void FindTarget(string allowedTargets)
    {

        if (allowedTargets == "self")
        {
            Debug.Log("Self Targeting");
            foundTargets.Add(this.gameObject);
            this.GetComponent<EffectManager>().targetsChecked = true;
        }else
        {
            switch (currentEffect.abilityTarget)
            {
                case AbilityEffect.EffectTargeting.areaOfEffect:

                    break;
                case AbilityEffect.EffectTargeting.declared:

                    break;
            }
        }
    }


    public void ResetManager()
    {
        currentEffect = null;
        foundTargets.Clear();
    }
}
