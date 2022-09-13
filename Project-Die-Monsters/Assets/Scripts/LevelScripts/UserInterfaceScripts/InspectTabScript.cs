using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InspectTabScript : MonoBehaviour // A UI display of a creature card used for checking what object is inside a die and picking a attack target.
{
    //The used for string(usedFor) within some functions is where we store why the inspect interface was opened and how we know what parts of the UI to enable / disable ect at any point.
    // "DrawDice", added a creature scriptable object to the dice to be rolled in the dice phase.
    // "Die Inspect, view what creature is inside of clicked dice that has been rolled.
    // "Pool Inspect", View what creature is currently stored within the creature pool UI we are hovering over.
    // "PieceInspect", view the details of the board piece the player is hovering over
    //AttackTargetSelection, display the details of the possible attack targets of chosen piece

    [Header("ScriptComponenets")]
    public GameObject CreatureInspectUI;
    public GameObject DungeonLordInspectUI;




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


    [Header("WindowContents")]
    //These two public objects are for assigning a die object to a 3D dice in the scene.
    public GameObject sceneDice;
    public GameObject turnPlayer;

    //What dice out of the player dice deck is currently displayed.
    int diceShown = 0;

    public Creature currentCreature; // the creature scriptableobject stored inside the die or creature pool.
    public DungeonLord currentDungeonLord;
    public GameObject currentCreatureToken; // the board piece that is being used for a combat action.
    
    int targetShown = 0; //Which of the creature chosens possible attack targets are being displayed.


    // Start is called before the first frame update
    void Start()
    {
        CloseInspectWindow();
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
    }

    #region displayAndHideUI
    public void CloseInspectWindow()
    {
        DungeonLordInspectUI.gameObject.SetActive(false);
        CreatureInspectUI.gameObject.SetActive(false);
    }

    public void OpenInspectWindow(string usedFor)
    {
        switch (usedFor)
        {
            case "DrawDice":
               // showCreatureDetails(myComponents.Count);
                DisplayCreatureDetails(usedFor);
                break;
            case "DieInspect":
                //componentsToDisplay = 11;
                DisplayCreatureDetails(usedFor);
                break;
            case "PoolInspect":
                //componentsToDisplay = 10;
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
                   //showCreatureDetails(myComponents.Count);
                    DisplayCreatureDetails(usedFor);

                }
                break;
        }
        // add check for state to choosecreature  button if not in dice window;
       
    }

    public void showCreatureDetails (int componentsToDisplay)
    {
        for (int i = 0; i < componentsToDisplay; i++)
        {
           // if (myComponents[i].GetComponent<Image>() != null)
            //{
           //     myComponents[i].GetComponent<Image>().enabled = true;
          //  }

          //  if (myComponents[i].GetComponent<Text>() != null)
           // {
          //      myComponents[i].GetComponent<Text>().enabled = true;
          //  }
        }
    }



    public void DisplayCreatureDetails(string usedFor) // sets the inspect window when displaying die contents either in manager or creature pool.
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
    #endregion

    #region inspectBTNInput

    public void ButtonPressed(string usedFor)
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
                    CloseInspectWindow();
                }

                if (usedFor == "DieInspect")
                {
                    AddCreatureToPool();
                    //When we end our dice management state change this boolean.
                    lvlRef.GetComponent<LevelController>().ableToInteractWithBoard = true;
                }

                if (usedFor == "PoolInspect")
                {
                    Debug.Log("Cant Be Possible");
                }

                if (usedFor == "AttackTargetSelection")
                {
                    if (currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                    {
                        DisplayDungeonLord();
                    }
                    else if (currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
                    {
                        CreatureIsAttackTarget();
                    }
                }
                break;

            case "ForwardBTN":

                if (usedFor == "DrawDice")
                {
                    if (diceShown < turnPlayer.GetComponent<Player>().diceDeck.Count)
                    {
                        diceShown += 1;
                        DisplayCreatureDetails(usedFor);
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
                            DisplayCreatureDetails(usedFor);
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
                        DisplayCreatureDetails(usedFor);
                    }
                }

                if (usedFor == "AttackTargetSelection")
                {
                    if (targetShown > 0)
                    {
                        targetShown -= 1;
                        if (currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                        {
                            DisplayDungeonLord();
                        }
                        else if (currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
                        {
                            DisplayCreatureDetails(usedFor);
                        }
                    }
                }
                break;

            case "CloseBTN":
                // go back to previous step.
                lvlRef.GetComponent<CreatureController>().ChosenAction = "Choosing";
                lvlRef.GetComponent<CreatureController>().CheckPossibleActions();
                lvlRef.GetComponent<CreatureController>().HideAndShowButtons();
                CloseInspectWindow();
                break;
        }
    }


    //This function is called when we want to show the inspect tab UI Object
  
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
        CloseInspectWindow();

        // switch to the board view.
        lvlRef.GetComponent<CameraController>().ActiveCam = "Alt";
        lvlRef.GetComponent<CameraController>().switchCamera();

        //Player not performing action can press end turn BTN.
        lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false;

    }
    #endregion

    
    //These two scripts go to the next step of combat if either a creature or dungeon lord is chosen
    public void CreatureIsAttackTarget()
    {
        //Declare creature in target list as attack target then send that information to creature vs creature combat script.
        GameObject CW = GameObject.FindGameObjectWithTag("CombatWindow");
        CW.GetComponent<AttackUIScript>().attacker = currentCreatureToken;
        CW.GetComponent<AttackUIScript>().defender = currentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].gameObject;
        CW.GetComponent<AttackUIScript>().Action = "Decide";
        CW.GetComponent<AttackUIScript>().displayAttackWindow();
        //Hide this Window for now
        CloseInspectWindow();
    }

    public void DungeonLordIsAttackTarget()
    {

    }
}
