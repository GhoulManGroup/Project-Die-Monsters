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
                targetsPickedPanel.SetActive(true);
                targetText.text = "Targets Min " + currentEffect.requiredTargetCount + "Targets Max " + currentEffect.maximumTargetCount + "Tagets Picked " + currentCreature.GetComponent<TargetManager>().foundTargets.Count;
                break;

            case "AOE":
                confirmBTN.GetComponent<Button>().interactable = true;
                cancleBTN.GetComponent<Button>().interactable = true;
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
                break;
            case "CastAbility":
                currentCreature.GetComponent<AbilityManager>().abilityCast = true;
                break;
        }
                confirmBTN.GetComponent<Button>().interactable = false;
                cancleBTN.GetComponent<Button>().interactable = false;
                HideInterface();
    }
}
