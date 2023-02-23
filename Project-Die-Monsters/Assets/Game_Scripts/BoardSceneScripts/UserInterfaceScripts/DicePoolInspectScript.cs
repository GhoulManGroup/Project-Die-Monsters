using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class DicePoolInspectScript : MonoBehaviour
{
    public Die myDie;
    public int diceNumber;
    [SerializeField] GameObject dieImage;
    [SerializeField] GameObject crestImage;
    public List<Sprite> dieCrestList = new List<Sprite>();
    public List<Sprite> dieColorList = new List<Sprite>();

    // This script will now be repurposed to manage the redesigned die selection interface 

    public void setUp()
    {
        myDie.UpdateDetails();
        Die.RenameScriptableObject(myDie, myDie.dieID + " "+ myDie.dieCreature.name);
        
        switch (myDie.dieCreatureLevel.ToString())
        {
            case "one":
                crestImage.GetComponent<Image>().sprite = dieCrestList[0];
                break;
            case "two":
                crestImage.GetComponent<Image>().sprite = dieCrestList[1];
                break;
            case "three":
                crestImage.GetComponent<Image>().sprite = dieCrestList[2];
                break;
            case "four":
                crestImage.GetComponent<Image>().sprite = dieCrestList[3];
                break;
        }

        switch (myDie.dieColor.ToString())
        {
            case "Red":
                dieImage.GetComponent<Image>().sprite = dieColorList[0];
                break;
            case "Blue":
                dieImage.GetComponent<Image>().sprite = dieColorList[2];
                break;
            case "Yellow":
                dieImage.GetComponent<Image>().sprite = dieColorList[1];
                break;
            case "Green":
                dieImage.GetComponent<Image>().sprite = dieColorList[4];
                break;
            case "Purple":
                dieImage.GetComponent<Image>().sprite = dieColorList[5];
                break;
            case "Orange":
                dieImage.GetComponent<Image>().sprite = dieColorList[3];
                break;
            case "White":
                dieImage.GetComponent<Image>().sprite = dieColorList[6];
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

