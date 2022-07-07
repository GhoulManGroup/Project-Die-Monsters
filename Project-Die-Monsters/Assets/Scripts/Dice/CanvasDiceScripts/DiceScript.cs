using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceScript : MonoBehaviour //This script handles the UI Dice in the Dice management window.
{
    [Header("Die Components")]
    public List<Die> drawnDice = new List<Die>();
    public GameObject crestChildSprite;
    public GameObject inspectWindow;

    [Header("Dice Lists")]

    public List<Sprite> dieColorSpites = new List<Sprite>();
    public List<Sprite> crestSprites = new List<Sprite>();

    [Header("DiceState")]
    public bool amISeen = false;
    string landedCrest;
    string myColor;
    public string myResult;
    public bool CanBeChosen = false;
    public int WhichDiceAmI;
    

    public void Awake()
    {

    }

    public void diceDrawn() // this funciton is run when the dice are drawn so the player knows what creature level is in each dice.
    {
        amISeen = true;
        landedCrest = "Level";
        myStates();
    }

    public void myStates() // set our color based on the die scriptable objects color, then idenfity the crest rolled and have child display that.
    {
        myColor = drawnDice[0].dieColor.ToString();
        switch (myColor)
        {
            case "Blue":
                this.GetComponent<Image>().sprite = dieColorSpites[0];
                break;
            case "Red":
                this.GetComponent<Image>().sprite = dieColorSpites[1];
                break;
            case "Green":
                this.GetComponent<Image>().sprite = dieColorSpites[2];
                break;
            case "Yellow":
                this.GetComponent<Image>().sprite = dieColorSpites[3];
                break;
            case "White":
                this.GetComponent<Image>().sprite = dieColorSpites[4];
                break;
            case "Black":
                this.GetComponent<Image>().sprite = dieColorSpites[5];
                break;             
        }

        switch (landedCrest)
        {
            case "Level":
                switch (drawnDice[0].dieCreatureLevel.ToString())
                {
                    case "one":
                        crestChildSprite.GetComponent<Image>().sprite = crestSprites[4];
                        myResult = "L1C";
                        break;
                    case "two":
                        crestChildSprite.GetComponent<Image>().sprite = crestSprites[5];
                        myResult = "L2C";
                        break;
                    case "three":
                        crestChildSprite.GetComponent<Image>().sprite = crestSprites[6];
                        myResult = "L3C";
                        break;
                    case "four":
                        crestChildSprite.GetComponent<Image>().sprite = crestSprites[7];
                        myResult = "L4C";
                        break;
                }
                break;
            case "Move":
                crestChildSprite.GetComponent<Image>().sprite = crestSprites[0];
                myResult = "Move";
                break;
            case "Attack":
                crestChildSprite.GetComponent<Image>().sprite = crestSprites[1];
                myResult = "Attack";
                break;
            case "Defence":
                crestChildSprite.GetComponent<Image>().sprite = crestSprites[2];
                myResult = "Defence";
                break;
            case "AP":
                crestChildSprite.GetComponent<Image>().sprite = crestSprites[3];
                myResult = "AP";
                break;
        }
    }

    // land on crest idefity the crest, and display the level of the level one.
    public void RollMe()
    {
        int rollDie = Random.RandomRange(1, 6); // what face the die landed on.

        switch (rollDie)
        {
            case 1:
                landedCrest = drawnDice[0].firstCrest.ToString();
                break;
            case 2:
                landedCrest = drawnDice[0].secondCrest.ToString();
                break;
            case 3:
                landedCrest = drawnDice[0].thirdCrest.ToString();
                break;
            case 4:
                landedCrest = drawnDice[0].forthCrest.ToString();
                break;
            case 5:
                landedCrest = drawnDice[0].fifthCrest.ToString();
                break;
            case 6:
                landedCrest = drawnDice[0].sixthCrest.ToString();
                break;
        }
        myStates();
    }

    public void amIDisplayed()
    {
        switch (amISeen)
        {
            case true:
                this.GetComponent<Image>().enabled = true;
                crestChildSprite.GetComponent<Image>().enabled = true;
                break;
            case false:
                this.GetComponent<Image>().enabled = false;
                crestChildSprite.GetComponent<Image>().enabled = false;
                break;
        }
    }

    public void DieSelectedForInspect() // If this dice can be chosen as a creature target allow it to be clicked;
    {
        if (CanBeChosen == true)
        {
            inspectWindow.GetComponent<InspectTabScript>().usedFor = "DieInspect";
            inspectWindow.GetComponent<InspectTabScript>().ShowFunction();
            inspectWindow.GetComponent<InspectTabScript>().CurrentCreature = drawnDice[0].dieCreature;
            inspectWindow.GetComponent<InspectTabScript>().setDetails();
        }
    }
}
