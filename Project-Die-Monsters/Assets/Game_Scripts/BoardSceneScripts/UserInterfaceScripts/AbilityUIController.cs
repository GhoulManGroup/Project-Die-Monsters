using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIController : MonoBehaviour
{
    public GameObject cancleBTN;
    public GameObject confirmBTN;
    public string confirmBTNFunction = "Nothing";

    public GameObject targetsPickedPanel;
    public Text targetText;

    public GameObject currentCreature;
    AbilityEffect currentEffect;

    [Header("Ability Stack")]
    bool startStack = false;
    public List<GameObject> creaturesToTrigger = new List<GameObject>();
    public bool waitForCast;

    #region Ability UI Display
    public void Awake()
    {
        HideInterface();
    }

    public void HideInterface()
    {
        targetsPickedPanel.SetActive(false);
        cancleBTN.SetActive(false);
        confirmBTN.SetActive(false);    
    }

    public void ShowAndUpdateInterface(string showedFor)
    {
        currentEffect = currentCreature.GetComponent<EffectManager>().effectToResolve;

        switch (showedFor)
        {
            case "Declare":
                cancleBTN.GetComponent<Button>().interactable = true;
                targetsPickedPanel.SetActive(true);
                targetText.text = "Targets Min " + currentEffect.requiredTargetCount + "Targets Max " + currentEffect.maximumTargetCount + "Tagets Picked " + currentCreature.GetComponent<TargetManager>().foundTargets.Count;
                break;

            case "AOE":
                targetsPickedPanel.SetActive(true);
                targetText.text = "Creatures Hit! " + currentCreature.GetComponent<TargetManager>().foundTargets.Count;
                confirmBTN.GetComponent<Button>().interactable = true;
                cancleBTN.GetComponent<Button>().interactable = true;
                break;

            case "ConfirmCast":
                targetsPickedPanel.SetActive(true);
                targetText.text = "Confirm Cast?";
                cancleBTN.GetComponent<Button>().interactable = true;
                confirmBTN.GetComponent<Button>().interactable = true;
                break;
        }

        cancleBTN.SetActive(true);
        confirmBTN.SetActive(true);
    }

    public void CancleAbility()
    {
        currentCreature.GetComponent<AbilityManager>().CancleAbilityCast();
        confirmBTN.GetComponent<Button>().interactable = false;
        cancleBTN.GetComponent<Button>().interactable = false;
        HideInterface();
    }

    public void ConfirmCast()
    {
        switch (confirmBTNFunction)
        {
            case "DeclareAOEPosition":
                currentCreature.GetComponent<TargetManager>().directionIndicated = true;
                break;
            case "DeclareTargets":
                currentCreature.GetComponent<TargetManager>().hasDeclared = true;
                GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().boardInteraction = "None";
                break;
            case "CastAbility":
                currentCreature.GetComponent<AbilityManager>().abilityCast = true;
                break;
        }
                confirmBTN.GetComponent<Button>().interactable = false;
                cancleBTN.GetComponent<Button>().interactable = false;
                HideInterface();
    }
    #endregion

    #region Trigger Ability Management
    public IEnumerator StackManager()
    {
        Debug.Log("Inside Stack Manager");
        targetsPickedPanel.SetActive(true);
        targetText.text = "Trigger Ability To Resolve";
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < creaturesToTrigger.Count; i++)
        {
            waitForCast = true;
            creaturesToTrigger[i].GetComponent<AbilityManager>().StartCoroutine("ActivatedEffect");
            while(waitForCast == true)
            {
                yield return null;
                //Wait here untill that creature is done casting its effect.
                //Add other creature to stack if triggered by another effect.
            }
            yield return new WaitForSeconds(1f);
        }
        //Loop Is Done
        HideInterface();
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().CheckCreatureStates();
        yield return null;
        Debug.Log("Howdy FROM STACK MANAGER BOYO");
    }
    #endregion
}
