using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour //This script will oversee the use of a creatures ability when called by either the trigger and or creature caller.
{
    public Ability myAbility;
    CreatureToken  myCreature;
    LevelController lvlRef;
    PlayerCreatureController CCRef;
    AbilityUIController AbilityWindow;
    public GameObject TriggeringCreature;

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
        AbilityWindow = GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>();
        CCRef = GameObject.FindGameObjectWithTag("LevelController").GetComponent<PlayerCreatureController>();
        lvlRef = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
    }
    public void CheckTrigger(string trigger, GameObject triggeredBy = null)
    {
        if (myAbility.abilityActivatedHow == Ability.AbilityActivatedHow.Trigger)
        {
            if (trigger == myAbility.howTriggered.ToString())
            {
                //Debug.Log("Trigger Match" + trigger + myAbility.howTriggered);
                TriggeringCreature = triggeredBy;
                AbilityWindow.creaturesToTrigger.Add(this.gameObject);
            }
            else
            {
                //Debug.Log("Trigger Not Matched" + trigger + myAbility.howTriggered);
            }
        }
        else
        {
            //Debug.Log("No Trigger On Creature" + this.gameObject.name);
        }
    }

    public IEnumerator ActivatedEffect()
    {
        for (int i = 0; i < myAbility.abilityEffects.Count; i++)
        {
            checkingEffect = true;
            this.GetComponent<EffectManager>().effectToResolve = myAbility.abilityEffects[i];
            this.GetComponent<EffectManager>().StartCoroutine("PrepareAndCastEffect");

            while (checkingEffect == true)
            {
                yield return null;
            }
        }
        // Here We Are
        while (readyEffects != myAbility.abilityEffects.Count)
        {
            yield return null;
        }

        if (myAbility.abilityActivatedHow == Ability.AbilityActivatedHow.Trigger)
        {
            abilityCast = true;
        }
        else
        {
            AbilityWindow.ShowAndUpdateInterface("ConfirmCast");
            AbilityWindow.confirmBTNFunction = "CastAbility";
            AbilityWindow.confirmBTN.GetComponent<Button>().interactable = true;
        }

        while (effectsResolved != myAbility.abilityEffects.Count)
        {
            yield return null;
        }

        lvlRef.GetComponent<LevelController>().turnPlayerObject.GetComponent<Player>().abiltyPowerCrestPoints -= myAbility.abilityCost;
        this.GetComponent<CreatureToken>().hasUsedAbilityThisTurn = true;
        CancleAbilityCast();
        if (myAbility.abilityActivatedHow == Ability.AbilityActivatedHow.Trigger)
        {
            GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().waitForCast = false;
        }
        else
        {
            lvlRef.CheckForTriggersToResolve();

        }
        yield return null;
    }

    public void CancleAbilityCast()
    {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().boardInteraction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<PlayerCreatureController>().ChosenAction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<PlayerCreatureController>().OpenAndCloseControllerUI();
        lvlRef.turnPlayerPerformingAction = false;
        ResetAbilitySystem();
    }

    public void ResetAbilitySystem()
    {
        this.GetComponent<EffectManager>().ResetManager();
        this.GetComponent<TargetManager>().ResetManager();
        ResetManager();
    }

    public void ResetManager()
    {
        abilityCast = false;
        readyEffects = 0;
        effectsResolved = 0;

        checkingEffect = false;
        canBeCast = false;
        effectsCanBeDone = 0;
        StopAllCoroutines();
    }

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
        }

        if (effectsCanBeDone == myAbility.abilityEffects.Count)
        {
            canBeCast = true;

            Debug.Log("AbilityCanBeCast");

            if (lvlRef.turnPlayerObject.GetComponent<Player>() != null)
            {
                GameObject.FindGameObjectWithTag("LevelController").GetComponent<PlayerCreatureController>().CheckPossibleActions();
            }

        }else if (effectsCanBeDone != myAbility.abilityEffects.Count)
        {
            Debug.Log("AbilityCantBeCast");
            ResetManager();
        }

        yield return null;
    }
}
