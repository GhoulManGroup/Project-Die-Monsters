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
            this.GetComponent<EffectManager>().targetsFound = true;
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

        /*
        switch (currentEffect.allowedTargets)
        {
            case AbilityEffect.AllowedTargets.self:

                break;
            case AbilityEffect.AllowedTargets.friendly:

                break;
            case AbilityEffect.AllowedTargets.hostile:

                break;
            case AbilityEffect.AllowedTargets.all:

                break;
        }
        */
    }


    public void SendTargets()
    {
        
    }

    public void ResetManager()
    {
        currentEffect = null;
        foundTargets.Clear();
    }
}
