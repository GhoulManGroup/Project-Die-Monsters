using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour //This script will oversee the use of a creatures ability when called by either the trigger and or creature caller.
{
    public Ability myAbility;
    CreatureToken  myCreature;

    public bool canBeCast = false; // Can we cast the ability used to enable creature controller UI

    // how many effects in the list of effects are ready to cast. if == count of effect list ability ready to cast.
    public int readyEffects = 0;

    public bool abilityCast = false; // Has the player actually confirmed the casting.
    //how many effects of our pending effects have actualy resolved used to determine when the effect is done.
    public int effectsResolved = 0;


    public void Awake()
    {
        myCreature = this.gameObject.GetComponent<CreatureToken>();
    }   

    public IEnumerator ActivateEffect()
    {
        for (int i = 0; i < myAbility.abilityEffects.Count; i++)
        {
            this.GetComponent<EffectManager>().effectToResolve = myAbility.abilityEffects[i];
            this.GetComponent<EffectManager>().StartCoroutine("PrepareAndCastEffect"); 
            //Add a Wait here untill the effect is done as user input is required.
        }

        while (readyEffects != myAbility.abilityEffects.Count)
        {
            yield return null;
        }

        Debug.Log("Hello From effects ready!");
        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().ShowAndUpdateInterface("ConfirmCast");
        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().confirmBTNFunction = "CastAbility";
        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().confirmBTN.GetComponent<Button>().interactable = true;

        while (effectsResolved != myAbility.abilityEffects.Count)
        {
            yield return null;
        }
        //Add subtraction of ability crest to this step, rather than when the ability is first pressed as it makes sence to only remove once its done casting.
        Debug.Log("Ability Has Been Cast Finished");
        this.GetComponent<CreatureToken>().hasUsedAbilityThisTurn = true;
        CancleAbilityCast();
        yield return null;
    }

    public void CancleAbilityCast()
    {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenAction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().OpenAndCloseControllerUI();
        ResetManager();
        this.GetComponent<EffectManager>().ResetManager();
        this.GetComponent<TargetManager>().ResetManager();
        //by runing the reset funcitons all all 3.
    }

    public void ResetManager()
    {
        StopAllCoroutines();
        readyEffects = 0;
        abilityCast = false;
        effectsResolved= 0;
    }

    public IEnumerator CheckAbilityCanBeCast(int checkedEffects, int castableEffects, bool hasChecked )
    {
        for (int i = 0; i < myAbility.abilityEffects.Count; i++)
        {

        }
        while (hasChecked == false)
        {
            if (checkedEffects == myAbility.abilityEffects.Count)
            {
                hasChecked = true;
            }
            yield return null;
        }

        if (castableEffects == myAbility.abilityEffects.Count)
        {
            canBeCast = true;
            Debug.Log("cANcASTiVElOoked");
        }else
        {
            canBeCast = false;
            Debug.Log("CANT cAST IVE CHECKED");
        }

        yield return null;
    }
}
