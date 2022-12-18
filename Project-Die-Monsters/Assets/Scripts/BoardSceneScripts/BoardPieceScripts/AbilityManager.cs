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
    }

    public void checkCanCastAbility()
    {
        if (canBeCast == false)
        {
            MyEffect();
        }
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
                myTargets("MoveReset");
            }
            else if (myAbility.whichState == Ability.StateReset.attack)
            {
                myTargets("AttackReset");
            }else if (myAbility.whichState == Ability.StateReset.useAbility)
            {

            }
        }

        if (myAbility.myEffect == Ability.MyEffect.modifier)
        {
            if (myAbility.statChanged == Ability.ModifiedProperty.attack)
            {
                myTargets("ModifyAttack");
            }
            else if (myAbility.statChanged == Ability.ModifiedProperty.defence)
            {
                myTargets("ModifyDefence");
            }
            else if (myAbility.statChanged == Ability.ModifiedProperty.health)
            {
                myTargets("ModifyHealth");
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
                    if (myCreature.GetComponent<CreatureToken>().HasAttackedThisTurn == true)
                    {
                        if (canBeCast == true)
                        {
                            myCreature.GetComponent<CreatureToken>().HasAttackedThisTurn = false;
                            myCreature.GetComponent<CreatureToken>().hasUsedAbilityThisTurn = true;
                            Debug.Log("Casting Ability");
                        }
                        else
                        {
                            canBeCast = true;
                            Debug.Log("Can Cast Ability");
                        }
                    }
                    else
                    {
                        Debug.Log("Cant do that havent attacked first already");
                    }
                    break;
                case "MoveReset":
                    if (myCreature.GetComponent<CreatureToken>().HasMovedThisTurn == true)
                    {
                        if (canBeCast == true)
                        {
                            myCreature.GetComponent<CreatureToken>().HasAttackedThisTurn = false;
                            myCreature.GetComponent<CreatureToken>().hasUsedAbilityThisTurn = true;
                            Debug.Log("Casting Ability");
                        }
                        else
                        {
                            canBeCast = true;
                            Debug.Log("Can Cast Ability");
                        }
                    }
                    else
                    {
                        Debug.Log("Cant do that havent moved first already");
                    }
                    break;
            }
        }
    }

}
