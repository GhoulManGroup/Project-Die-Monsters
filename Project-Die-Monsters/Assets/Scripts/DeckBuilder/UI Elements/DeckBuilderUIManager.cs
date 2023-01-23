using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DeckBuilderUIManager : MonoBehaviour
{
    [Header ("Deck Builder States")]
    public string UIStage;

    [Header("UI Elements Containers Canvas")]
    public List<GameObject> DeckSelectUI = new List<GameObject>();
    public List<GameObject> DeckBuildUI = new List<GameObject>();

    [Header("Deck Display Varibles  ")]
    public Image DeckSizeTab;
    public Text DeckSizeText;

    public List<Die> tempDeck = new List<Die>(); // this list is a clone of the list in the chosen deckslot which will overwrite the list on the scriptbale object once I saved changes.
    public List<GameObject> Creaturelist = new List<GameObject>();
    public Deck chosenDeck;
    int chosenDeckNumber;

    [Header("Collection Dispaly Varibles")]
    public List<GameObject> DieBTN = new List<GameObject>();
    public string colorFilter = "None";
    public string levelFilter = "None";
    int dieRangeStart; // these two varible int  control where the list starts to check eg the value of i and what it is < than. just like my color game menu this will allow me to on btn press.
    int dieRangeFinish; //move the range of the list one lot of 7 down or up allowing me to view in future above 42 die in the interface as if I was shifting pages.


    [Header("Refrences")]
    GameObject mRef;

    // Start is called before the first frame update
    void Start()
    {
        mRef = GameObject.FindGameObjectWithTag("GameController");
        UIStage = "DeckSelect";
        UISetUp();
        
    }

    // Update is called once per frame
    void Update()
    {
        //CheckBTNcanbePressed();
    }

    public void SetDeck()
    {
        for (int i = 0; i < mRef.GetComponent<DeckManager>().DeckSlots[chosenDeckNumber].DeckDie.Count; i++)
        {
            tempDeck.Add(chosenDeck.DeckDie[i]);
        }

        UIStage = "DeckBuilder";
        UISetUp();
        DisplayContents();
    }

    public void UISetUp()
    {
       switch (UIStage)
        {
            case "DeckSelect":
                for (int i = 0; i < DeckSelectUI.Count; i++)
                {
                    DeckSelectUI[i].GetComponent<Image>().enabled = true;
                }

                for (int i = 0; i < DeckBuildUI.Count; i++)
                {
                    DeckBuildUI[i].GetComponent<Image>().enabled = false;
                }

                for (int i = 0; i < Creaturelist.Count; i++)
                {
                    Creaturelist[i].GetComponent<CreatureSlotScript>().HideMe();
                }

                for (int i = 0; i < DieBTN.Count; i++)
                {
                    DieBTN[i].GetComponent<BuilderDie>().HideMe();
                }

                DeckSizeTab.enabled = false;
                DeckSizeText.enabled = false;
                break;

            case "DeckBuilder":
                for (int i = 0; i < DeckSelectUI.Count; i++)
                {
                    DeckSelectUI[i].GetComponent<Image>().enabled = false;
                }

                for (int i = 0; i < DeckBuildUI.Count; i++)
                {
                    DeckBuildUI[i].GetComponent<Image>().enabled = true;
                }

                for (int i = 0; i < DieBTN.Count; i++)
                {
                    DieBTN[i].GetComponent<BuilderDie>().ShowMe();
                }
            
                DeckSizeTab.enabled = true;
                DeckSizeText.enabled = true;               
                break;
        }
    }

    public void ButtonPressed()
    {
        string BTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();
        if (UIStage == "DeckSelect")
        {
            switch (BTNPressed)
            {
                case "Deck 1":
                    chosenDeck = mRef.GetComponent<DeckManager>().DeckSlots[0];
                    chosenDeckNumber = 0;
                    SetDeck();
                    break;
                case "Deck 2":
                    chosenDeck = mRef.GetComponent<DeckManager>().DeckSlots[1];
                    chosenDeckNumber = 1;
                    SetDeck();             
                    break;
                case "Deck 3":
                    chosenDeck = mRef.GetComponent<DeckManager>().DeckSlots[2];
                    chosenDeckNumber = 2;
                    SetDeck();
                    break;
                case "Deck 4":
                    chosenDeck = mRef.GetComponent<DeckManager>().DeckSlots[3];
                    chosenDeckNumber = 3;
                    SetDeck();
                    break;
                case "Deck 5":
                    chosenDeck = mRef.GetComponent<DeckManager>().DeckSlots[4];
                    chosenDeckNumber = 4;
                    SetDeck();
                    break;
                case "Back":
                    SceneManager.LoadScene("MainMenuScene");
                    break;
            }
        }
        if (UIStage == "DeckBuilder")
        {
            switch (BTNPressed)
            {
                case "SaveBTN":
                    CheckCanSave();
                    break;
                case "DownRow":

                    break;
                case "UpRow":

                    break;

                case "Back":
                    tempDeck.Clear();
                    UIStage = "DeckSelect";
                    UISetUp();
                    break;

                case "Red":
                    switch (colorFilter == "Red")
                    {
                        case false:
                            colorFilter = "Red";
                            break;
                        case true:
                            colorFilter = "None";
                            break;
                    }
                    DisplayDice();
                    break;
                case "Blue":
                    switch (colorFilter == "Blue")
                    {
                        case false:
                            colorFilter = "Blue";
                            break;
                        case true:
                            colorFilter = "None";
                            break;
                    }
                    DisplayDice();
                    break;
                case "Green":
                    switch (colorFilter == "Green")
                    {
                        case false:
                            colorFilter = "Green";
                            break;
                        case true:
                            colorFilter = "None";
                            break;
                    }
                    DisplayDice();
                    break;
                case "Yellow":
                    switch (colorFilter == "Yellow")
                    {
                        case false:
                            colorFilter = "Yellow";
                            break;
                        case true:
                            colorFilter = "None";
                            break;
                    }
                    DisplayDice();
                    break;
                case "White":
                    switch (colorFilter == "White")
                    {
                        case false:
                            colorFilter = "White";
                            break;
                        case true:
                            colorFilter = "None";
                            break;
                    }
                    DisplayDice();
                    break;
                case "Black":
                    switch (colorFilter == "Black")
                    {
                        case false:
                            colorFilter = "Black";
                            break;
                        case true:
                            colorFilter = "None";
                            break;
                    }
                    DisplayDice();
                    break;
                case "LVL1":
                    switch (levelFilter == "one")
                    {
                        case false:
                            levelFilter = "one";
                            break;
                        case true:
                            levelFilter = "None";
                            break;
                    }
                    DisplayDice();
                    break;
                case "LVL2":
                    switch (levelFilter == "two")
                    {
                        case false:
                            levelFilter = "two";
                            break;
                        case true:
                            levelFilter = "None";
                            break;
                    }
                    DisplayDice();
                    break;
                case "LVL3":
                    switch (levelFilter == "three")
                    {
                        case false:
                            levelFilter = "three";
                            break;
                        case true:
                            levelFilter = "None";
                            break;
                    }
                    DisplayDice();
                    break;
                case "LVL4":
                    switch (levelFilter == "four")
                    {
                        case false:
                            levelFilter = "four";
                            break;
                        case true:
                            levelFilter = "None";
                            break;
                    }
                    DisplayDice();
                    break;
            }
        }
    }

    public void DisplayContents()
    {     
        for (int i = 0; i < Creaturelist.Count; i++)
        {
            Creaturelist[i].GetComponent<CreatureSlotScript>().HideMe();
        }

        for (int i = 0; i < tempDeck.Count; i++)
        {          
            Creaturelist[i].GetComponent<CreatureSlotScript>().myDie = tempDeck[i];
            Creaturelist[i].GetComponent<CreatureSlotScript>().ShowMe();
            Creaturelist[i].GetComponent<CreatureSlotScript>().mySlot = i;
        }

        DeckSizeText.text = "Deck: " + chosenDeckNumber.ToString()+ " " + tempDeck.Count.ToString() + " / 15";
        DisplayDice();
    } // displays a creature slot for every die in the deck in the slot UI section

    public void DisplayDice() // check the filters and display the appropirate dice from the collection. 
    {
        for (int i = 0; i < DieBTN.Count; i++) // clear all diebtns first of previous state.
        {
            DieBTN[i].GetComponent<BuilderDie>().myState = "Empty";
            DieBTN[i].GetComponent<BuilderDie>().SetDetails();
        }

        if (colorFilter == "None" && levelFilter == "None")
        {
            for (int i = 0; i < mRef.GetComponent<DeckManager>().playerCollection.collectedDie.Count; i++)
            {

                DieBTN[i].GetComponent<BuilderDie>().myDie = mRef.GetComponent<DeckManager>().playerCollection.collectedDie[i];
                DieBTN[i].GetComponent<BuilderDie>().myState = "Used";
                DieBTN[i].GetComponent<BuilderDie>().SetDetails();
                
            }
        }

        else if (colorFilter == "None" && levelFilter != "None")
        {
            int freeSlot = 0;
            for (int i = 0; i < mRef.GetComponent<DeckManager>().playerCollection.collectedDie.Count; i++)
            {
                if (mRef.GetComponent<DeckManager>().playerCollection.collectedDie[i].dieCreatureLevel.ToString() == levelFilter)
                {
                    Debug.Log("LevelMatch");
                    DieBTN[freeSlot].GetComponent<BuilderDie>().myDie = mRef.GetComponent<DeckManager>().playerCollection.collectedDie[i];
                    DieBTN[freeSlot].GetComponent<BuilderDie>().myState = "Used";
                    DieBTN[freeSlot].GetComponent<BuilderDie>().SetDetails();
                    freeSlot += 1;
                }
            }
        }

        else if (colorFilter != "None" && levelFilter == "None")
        {
            int freeSlot = 0;
            for (int i = 0; i < mRef.GetComponent<DeckManager>().playerCollection.collectedDie.Count; i++)
            {
                if (mRef.GetComponent<DeckManager>().playerCollection.collectedDie[i].dieColor.ToString() == colorFilter)
                {
                    Debug.Log("Color Match");
                    DieBTN[freeSlot].GetComponent<BuilderDie>().myDie = mRef.GetComponent<DeckManager>().playerCollection.collectedDie[i];
                    DieBTN[freeSlot].GetComponent<BuilderDie>().myState = "Used";
                    DieBTN[freeSlot].GetComponent<BuilderDie>().SetDetails();
                    freeSlot += 1;
                }
            }
        }

        else if (colorFilter != "None" && levelFilter != "None")
        {
            int freeSlot = 0;
            for (int i = 0; i < mRef.GetComponent<DeckManager>().playerCollection.collectedDie.Count; i++)
            {
                if (mRef.GetComponent<DeckManager>().playerCollection.collectedDie[i].dieColor.ToString() == colorFilter && mRef.GetComponent<DeckManager>().playerCollection.collectedDie[i].dieCreatureLevel.ToString() == levelFilter)
                {
                    
                    DieBTN[freeSlot].GetComponent<BuilderDie>().myDie = mRef.GetComponent<DeckManager>().playerCollection.collectedDie[i];
                    DieBTN[freeSlot].GetComponent<BuilderDie>().myState = "Used";
                    DieBTN[freeSlot].GetComponent<BuilderDie>().SetDetails();
                    freeSlot += 1;
                }
            }
        }

    }

    public void CheckCanSave()
    {
        if (tempDeck.Count >= 12)
        {
            Debug.Log("Deck has Needed Die Count");
            chosenDeck.DeckDie.Clear();
            for (int i = 0; i < tempDeck.Count; i++)
            {
                chosenDeck.DeckDie.Add(tempDeck[i]);
            }
        }

        else if (tempDeck.Count < 12)
        {
            Debug.Log("Not Enough Dice");
        }
    }

    public void CheckBTNcanbePressed()
    {

    }
}      

