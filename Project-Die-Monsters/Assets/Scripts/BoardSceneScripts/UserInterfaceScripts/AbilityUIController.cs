using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIController : MonoBehaviour
{
    public GameObject cancleBTN;
    public GameObject confirmBTN;
    public GameObject currentCreature;

    public string confirmBTNFunction = "Nothing";

    public void Awake()
    {
        HideInterface();
    }

    public void HideInterface()
    {
        cancleBTN.SetActive(false);
        confirmBTN.SetActive(false);    
    }

    public void ShowInterface()
    {
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
