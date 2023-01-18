using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIController : MonoBehaviour
{
    public GameObject cancleBTN;
    public GameObject confirmBTN;
    public GameObject currentCreature;

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
        currentCreature.GetComponent<AbilityManager>().abilityCast = true;
        confirmBTN.GetComponent<Button>().interactable = false;
        cancleBTN.GetComponent<Button>().interactable = false;
        HideInterface();
    }
}
