using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour //This script will oversee the use of a creatures ability when called by either the trigger and or creature caller.
{
    public Ability myAbility;

    public void castAbility()
    {
        Debug.Log("AP Cost Paid casting!" + myAbility.nameOfAbility.ToString());
    }

    public void checkForTrigger(string triggerType)
    {
        if (triggerType == myAbility.myTrigger.ToString())
        {
            Debug.Log("TriggerMatch, casting Ability");
        }
    }
}
