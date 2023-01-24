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

    public void CheckAbilityCanCast() // Check if ability can be cast by seeing if there are enough valid targets for its conditions
    {
        Debug.Log("Checking Ability");
        for (int i = 0; i < myAbility.abilityEffects.Count; i++)
        {
            this.GetComponent<EffectManager>().effectToResolve = myAbility.abilityEffects[i];
            this.GetComponent<EffectManager>().StartCoroutine("ResolveEffect");
        }
    }

    public IEnumerator ActivateEffect()
    {
        for (int i = 0; i < myAbility.abilityEffects.Count; i++)
        {
            this.GetComponent<EffectManager>().effectToResolve = myAbility.abilityEffects[i];
            this.GetComponent<EffectManager>().StartCoroutine("PrepareAndCastEffect");          
        }

        while (readyEffects != myAbility.abilityEffects.Count)
        {
            yield return null;
        }

        Debug.Log("Hello From effects ready!");
        //Enable Confirm Ability BTN in the ability UI & wait for outcome.
        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().ShowAndUpdateInterface("ConfirmCast");
        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().confirmBTNFunction = "CastAbility";
        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().confirmBTN.GetComponent<Button>().interactable = true;

        while (effectsResolved != myAbility.abilityEffects.Count)
        {
            yield return null;
        }

        Debug.Log("Ability Has Been Cast Finished");
        this.GetComponent<CreatureToken>().hasUsedAbilityThisTurn = true;
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenAction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().OpenAndCloseControllerUI();
        CancleAbilityCast();
        yield return null;
    }

    public void CancleAbilityCast()
    {
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
}
