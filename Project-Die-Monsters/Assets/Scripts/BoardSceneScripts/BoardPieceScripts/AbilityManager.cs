using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour //This script will oversee the use of a creatures ability when called by either the trigger and or creature caller.
{
    public Ability myAbility;
    CreatureToken  myCreature;
    public void Awake()
    {
        myCreature = this.gameObject.GetComponent<CreatureToken>();
    }

    public void CheckAbility(string whyChecked) //Check the conditions of the attached creatures ability then resolve it.
    {
        if (myAbility != null)
        {
            if (myAbility.abilityType == Ability.AbilityType.Activated)
            {
               
            }else if (myAbility.abilityType == Ability.AbilityType.Trigger)
            {

            }else if (myAbility.abilityType == Ability.AbilityType.None)
            {
                Debug.Log("Error Why is ability type None!");
            }
        }
        else
        {
            Debug.Log("No Ability Found are you expecting one?");
        }
    }

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
