using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InspectTabScript : MonoBehaviour // A UI display of a creature card used for checking what object is inside a die and picking a attack target.
{
    [Header("Window Components")]
    public List<GameObject> myComponents = new List<GameObject>();

    public GameObject creatureArt;
    public GameObject creatureLevel;
    public GameObject creatureTribe;
    public GameObject creatureType;
    public Text creatureName;
    public Text attackValue;
    public Text defenceValue;
    public Text healthValue;

    GameObject lvlRef;

    [Header("Window States")]
    public string usedFor; // this tracks what we are opening the window four which will then control what it displays & what components are enabled.
    // DrawDice // DieInspect // Pool Inspect // AttackTargetSelection

    [Header("WindowContents")]
    //These two public objects are for assigning a die object to a 3D dice in the scene.
    public GameObject sceneDice;
    public GameObject turnPlayer;

    //What dice out of the player dice deck is currently displayed.
    int diceShown = 0;

    public Creature currentCreature; // the creature scriptableobject stored inside the die or creature pool.

    public GameObject currentCreatureToken; // the board piece that is being used for a combat action.
    
    int targetShown = 0; //Which of the creature chosens possible attack targets are being displayed.


    // Start is called before the first frame update
    void Start()
    {
        HideFunction();
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
    }

    public void ButtonPressed()
    {
        string inspectBTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (inspectBTNPressed)
        {
            case "SelectCreatureBTN":

                if (usedFor == "DrawDice")
                {
                    //Check if a dice is already in the object inside.
                    if (sceneDice.GetComponent<SceneDieScript>().myDie != null)
                    {
                        // if true then return that die object back to the player dice deck before we accidently remove it when we add the new dice to the object.
                        turnPlayer.GetComponent<Player>().diceDeck.Add(sceneDice.GetComponent<SceneDieScript>().myDie);
                    }

                    //Add that current dice from the deck to the dice object.
                    sceneDice.GetComponent<SceneDieScript>().myDie = turnPlayer.GetComponent<Player>().diceDeck[diceShown];

                    //Add this die scriptable object to our die object.
                    sceneDice.GetComponent<SceneDieScript>().setUpDie();

                    //Remove that dice from the dice deck list
                    turnPlayer.GetComponent<Player>().diceDeck.RemoveAt(diceShown);

                    //reset dice shown to 0
                    diceShown = 0;

                    //Close Window
                    HideFunction();
                }

                if (usedFor == "DieInspect")
                {
                    AddCreatureToPool();
                    //When we end our dice management state change this boolean.
                    lvlRef.GetComponent<LevelController>().ableToInteractWithBoard = true;
                }

                if (usedFor == "PoolInspect")
                {

                }

                if (usedFor == "AttackTargetSelection")
                {
                    CreatureIsAttackTarget();
                }
                break;

            case "ForwardBTN":

                if (usedFor == "DrawDice")
                {
                    if (diceShown < turnPlayer.GetComponent<Player>().diceDeck.Count)
                    {
                        diceShown += 1;
                        DisplayCreatureDetails();
                    }
                }

                if (usedFor == "AttackTargetSelection")
                {
                    if (targetShown < currentCreatureToken.GetComponent<CreatureToken>().targets.Count - 1)
                    {
                        targetShown += 1;
                        if (currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                        {
                            DisplayDungeonLord();
                        }else if (currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
                        {
                            DisplayCreatureDetails();
                        }

                    }
                }
                break;

            case "BackBTN":
                if (usedFor == "DrawDice")
                {
                    if (diceShown > 0)
                    {
                        diceShown -= 1;
                        DisplayCreatureDetails();
                    }
                }

                if (usedFor == "AttackTargetSelection")
                {
                    if (targetShown > 0)
                    {
                        targetShown -= 1;
                        DisplayCreatureDetails();
                        Debug.Log(targetShown);
                    }
                }
                break;

            case "CloseBTN":
                // go back to previous step.
                lvlRef.GetComponent<CreatureController>().ChosenAction = "Choosing";
                lvlRef.GetComponent<CreatureController>().CheckPossibleActions();
                lvlRef.GetComponent<CreatureController>().HideAndShowButtons();
                HideFunction();
                break;
        }
    }

    public void HideFunction()
    {
        for (int i = 0; i < myComponents.Count; i++)
        {
            if (myComponents[i].GetComponent<Image>() != null)
            {
                myComponents[i].GetComponent<Image>().enabled = false;
            }

            if (myComponents[i].GetComponent<Text>() != null)
            {
                myComponents[i].GetComponent<Text>().enabled = false;
            }
        }
    }

    //This function is called when we want to show the inspect tab UI Object
    public void ShowFunction()
    {
        int componentsToDisplay = 0;

        switch (usedFor)
        {
            case "DrawDice":
                componentsToDisplay = myComponents.Count;
                DisplayCreatureDetails();
                break;
            case "DieInspect":
                componentsToDisplay = 11;
                DisplayCreatureDetails();
                break;
            case "PoolInspect":
                componentsToDisplay = 10;
                break;
            case "AttackTargetSelection":

                if (currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                {
                    //if  current target is dungeon lord then display dungeon lord.
                    DisplayDungeonLord();
                }else if (currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
                {
                    //If current target is creature then display creature
                    currentCreature = currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>().myCreature;
                    DisplayCreatureDetails();
                    componentsToDisplay = myComponents.Count;
                }
                break;
        }
        // add check for state to choosecreature  button if not in dice window;
        for (int i = 0; i < componentsToDisplay; i++)
        {
            if (myComponents[i].GetComponent<Image>() != null)
            {
                myComponents[i].GetComponent<Image>().enabled = true;
            }

            if (myComponents[i].GetComponent<Text>() != null)
            {
                myComponents[i].GetComponent<Text>().enabled = true;
            }
        }
    }



    public void DisplayCreatureDetails() // sets the inspect window when displaying die contents either in manager or creature pool.
    {
        switch (usedFor)
        {
            case "DrawDice":
                //Set current creature to creature contained within the dice we are currently showing the player from the player deck.
                currentCreature = turnPlayer.GetComponent<Player>().diceDeck[diceShown].dieCreature;              
                break;

            case "DieInspect":
                // show the creature currently in the 3D dice object clicked on.
                currentCreature = sceneDice.GetComponent<SceneDieScript>().myDie.dieCreature;
                break;
        }
        creatureArt.GetComponent<Image>().sprite = currentCreature.CardArt;
        creatureLevel.GetComponent<Image>().sprite = currentCreature.LevelSprite;
        creatureTribe.GetComponent<Image>().sprite = currentCreature.TribeSprite;
        creatureType.GetComponent<Image>().sprite = currentCreature.TypeSprite;
        creatureName.GetComponent<Text>().text = currentCreature.creatureType.ToString();
        attackValue.GetComponent<Text>().text = "ATK" + currentCreature.Attack.ToString();
        defenceValue.GetComponent<Text>().text = "DEF" + currentCreature.Defence.ToString();
        healthValue.GetComponent<Text>().text = "HP" + currentCreature.Health.ToString();
    }

    public void DisplayDungeonLord()
    {

    }
  
    public void AddCreatureToPool() //Selected dice is discarded to add the creature inside to the player creature pool.
    {
        //add creature to pool
        lvlRef.GetComponent<CreaturePoolController>().turnPlayer.GetComponent<Player>().CreaturePool.Add(currentCreature);
        lvlRef.GetComponent<CreaturePoolController>().enableButtons();

        // remove the die object from the dice we are currently showing the contents of.
        sceneDice.GetComponent<SceneDieScript>().myDie = null;

        //Call the UIDICEController and run the reset function.
        lvlRef.GetComponent<UIDiceController>().resetFunction();

        // hide the inspect tab
        HideFunction();

        // switch to the board view.
        lvlRef.GetComponent<CameraController>().ActiveCam = "Alt";
        lvlRef.GetComponent<CameraController>().switchCamera();

        //Player not performing action can press end turn BTN.
        lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false;

    }
    
    public void CreatureIsAttackTarget()
    {
        //Check if dungeon lord or monster if dungeon lord do damage else go to attack window()
        GameObject CW = GameObject.FindGameObjectWithTag("CombatWindow");
        CW.GetComponent<AttackUIScript>().attacker = currentCreatureToken;
        CW.GetComponent<AttackUIScript>().defender = currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].gameObject;
        CW.GetComponent<AttackUIScript>().Action = "Decide";
        CW.GetComponent<AttackUIScript>().displayAttackWindow();
        //Hide this Window for now
        HideFunction();
    }
}
