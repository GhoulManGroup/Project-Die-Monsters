using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    //public void determineTa

    public AbilityEffect currentEffect;
    public List<GameObject> foundTargets = new List<GameObject>();
    public void FindTarget()
    {
        switch (currentEffect.allowedTargets)
        {
            case AbilityEffect.AllowedTargets.self:
                Debug.Log("Self Targeting");
                foundTargets.Add(this.gameObject);
                this.GetComponent<EffectManager>().targetsFound = true;
                break;
        }
    }


    public void SendTargets()
    {
        
    }

    public void ResetManager()
    {
        foundTargets.Clear();
    }
}
