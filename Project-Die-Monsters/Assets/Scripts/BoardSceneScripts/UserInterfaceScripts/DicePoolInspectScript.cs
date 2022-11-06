using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DicePoolInspectScript : MonoBehaviour
{
    public Die myDie;
    public int diceNumber;
    [SerializeField] GameObject dieImage;
    [SerializeField] GameObject crestImage;
    public List<Sprite> crestAndMaterialList = new List<Sprite>();

    // This script will now be repurposed to manage the redesigned die selection interface 

    public void setUp()
    {
        switch (myDie.dieCreatureLevel.ToString())
        {
            case "one":
                crestImage.GetComponent<Image>().sprite = crestAndMaterialList[0];
                break;
            case "two":
                crestImage.GetComponent<Image>().sprite = crestAndMaterialList[1];
                break;
            case "three":
                crestImage.GetComponent<Image>().sprite = crestAndMaterialList[2];
                break;
            case "four":
                crestImage.GetComponent<Image>().sprite = crestAndMaterialList[3];
                break;
        }

        switch (myDie.dieColor.ToString())
        {
            case "Red":
                dieImage.GetComponent<Image>().sprite = crestAndMaterialList[5];
                break;
            case "Blue":
                dieImage.GetComponent<Image>().sprite = crestAndMaterialList[6];
                break;
            case "Yellow":
                dieImage.GetComponent<Image>().sprite = crestAndMaterialList[8];
                break;
            case "Green":
                dieImage.GetComponent<Image>().sprite = crestAndMaterialList[7];
                break;
            case "White":
                dieImage.GetComponent<Image>().sprite = crestAndMaterialList[4];
                break;
        }
    }

    public void displayMyContents()
    {
        GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().diceShown = diceNumber;
        GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().OpenInspectWindow("DrawDice");
    }
    
    public void PickMe()
    {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<UIDiceController>().UIElements[3].gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().AssignDice();

    }

    
}

