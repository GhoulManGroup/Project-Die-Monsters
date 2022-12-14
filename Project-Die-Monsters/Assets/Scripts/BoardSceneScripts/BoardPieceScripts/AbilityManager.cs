using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour //This script will oversee the use of a creatures ability when called by either the trigger and or creature caller.
{
    public Ability myAbility;
    CreatureToken  myCreature;
    public bool canBeCast = false;


    public List<GameObject> targetedCreatures = new List<GameObject>();

    public void Awake()
    {
        myCreature = this.gameObject.GetComponent<CreatureToken>();
        myAbility = myCreature.myCreature.myAbility;
    }

    public void ActivatedAbilityCast()
    {
        //What do we want to do
        //What do we do it to
        //How will we do it.
        //Do it.?
        MyEffect();
    }

    public void MyEffect()
    {
        if (myAbility.myEffect == Ability.MyEffect.stateChange)
        {
            if (myAbility.whichState == Ability.StateReset.move)
            {

            }else if (myAbility.whichState == Ability.StateReset.attack)
            {
                myTargets("AttackReset");
            }else if (myAbility.whichState == Ability.StateReset.useAbility)
            {

            }
        }
    }

    public void myTargets(string myEffect)
    {
        if (myAbility.allowedTargets == Ability.AllowedTargets.Self)
        {
            switch (myEffect)
            {
                case "AttackReset":

                    break;
            }
        }
    }

}
