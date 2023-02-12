using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.IK;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour //This script will oversee the use of a creatures ability when called by either the trigger and or creature caller.
{
    public Ability myAbility;
    CreatureToken  myCreature;

    public bool abilityCast = false; // this is the check for if the ability has been used this turn already

    [Header("Ability Casting System")]
    public int readyEffects = 0; //effects prepared and ready for applicaiton to targets
    public int effectsResolved = 0; //resolved effect count


    [Header("Check Can Cast")]
    public bool checkingEffect = false; //This is inside for loop to make it wait untill the previous effect is checked before continung the loop.
    public bool canBeCast = false; // Can we cast the ability used to enable creature controller UI
    public int effectsCanBeDone = 0; //How many effects are able to be done.

    public void Awake()
    {
        myCreature = this.gameObject.GetComponent<CreatureToken>();
    }   

    public void abilityTrigger()
    {
        //if (myAbility.)
    }
    public IEnumerator TriggeredEffect()
    {
        yield return null;
    }

    public IEnumerator ActivatedEffect()
    {
        for (int i = 0; i < myAbility.abilityEffects.Count; i++)
        {
            this.GetComponent<EffectManager>().effectToResolve = myAbility.abilityEffects[i];
            this.GetComponent<EffectManager>().StartCoroutine("PrepareAndCastEffect");
            //Add while loop to stargger effect preperation.
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
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().CheckCreatureStates();
        CancleAbilityCast();
        yield return null;
    }

    public void CancleAbilityCast()
    {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenAction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().OpenAndCloseControllerUI();

        ResetAbilitySystem();
    }

    public void ResetAbilitySystem()
    {
        Debug.Log("Reseting Ability System");
        ResetManager();
        this.GetComponent<EffectManager>().ResetManager();
        this.GetComponent<TargetManager>().ResetManager();
    }

    public void ResetManager()
    {
        StopAllCoroutines();
        abilityCast = false;
        readyEffects = 0;
        effectsResolved= 0;

        checkingEffect = false;
        canBeCast = false;
        effectsCanBeDone = 0;
    }

    //Call this from creature controller.
    public IEnumerator CheckAbilityCanBeCast( )
    {
        for (int i = 0; i < myAbility.abilityEffects.Count; i++)
        {//Loop through each effect and check.
            checkingEffect = true;
            this.GetComponent<TargetManager>().currentEffect = myAbility.abilityEffects[i];
            this.GetComponent<EffectManager>().effectToResolve = myAbility.abilityEffects[i];
            this.GetComponent<EffectManager>().StartCoroutine("EffectChecking");
            while (checkingEffect == true)
            {
                yield return null;
            }
            Debug.Log("AM lOOP Check");
        }

        if (effectsCanBeDone == myAbility.abilityEffects.Count)
        {
            canBeCast = true;
            Debug.Log("AbilityCanBeCast");
            GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().CheckPossibleActions();
        }else if (effectsCanBeDone != myAbility.abilityEffects.Count)
        {
            Debug.Log("AbilityCantBeCast");
            ResetManager();
        }

        yield return null;
    }
}
