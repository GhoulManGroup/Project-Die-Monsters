using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InspectWindowController : MonoBehaviour //This script controls the various non combat in-Level UI windows that display the details of creatures, dungeonlords, items ect.
{
    //The used for string(usedFor) within some functions is where we store why the inspect interface was opened and how the script knows what parts of the UI to enable / disable ect at any point based on what is needed.
    // "DrawDice", Display the current turn players dice pool to allow them to assign it to the 3DDice fab to roll.
    // "DieInspect, view what creature is inside of clicked dice that has been rolled and can be summoned for choosing.
    // "PoolInspect", View what creature is currently stored within the creature pool UI object we are hovering over.
    // "PieceInspect", view the details of the board piece the player is hovering over
    //AttackTargetSelection, display the details of the possible attack targets of chosen piece.

    #region VariblesEct
    [Header("Interface UI Panels")] //The Game Objects that comprise the inspect UI.
    public GameObject creatureInspectUI; //Parent of CIUI
    public GameObject dungeonLordInspectUI; //Parent of DLIUI
    public GameObject crestDisplay; //Shows what crests a dice in the pool has.
    [SerializeField] CreatureInspectElements  creatureWindow = default;
    [SerializeField] DungeonLordInspectElements dungeonLordWindow = default;
    [SerializeField] InspectionWindowButtons inspectWindowButtons = default;

    #region SeralizedFields
    [Serializable]
    struct CreatureInspectElements
    {
        public GameObject creatureArt;
        public GameObject creatureLevel;
        public GameObject creatureTribe;
        public GameObject creatureType;
        public Text creatureName;
        public Text attackValue;
        public Text defenceValue;
        public Text healthValue;
    }
    [Serializable]
    struct DungeonLordInspectElements
    {
        public GameObject dungeonLordArt;
        public GameObject lifeIcon;
        public Text lifeText;
    }
    [Serializable]
    struct InspectionWindowButtons 
    {
        public GameObject declareBTN;
        public GameObject nextBTN;
        public GameObject backBTN;
    }

    #endregion

    [Header("DiceWindowRefrence")]
    //These two public objects are for assigning a die object to a 3D dice in the scene.
    public GameObject sceneDice;
    public GameObject turnPlayer;
    //What dice out of the player dice deck is currently displayed.
    int diceShown = 0;

    //Refrence to levelManager
    GameObject levelManager;

    //Refrence to game objects / scriptables
    public Creature currentCreature; //The creature scriptable object contained with a game object, eg dice or board piece.
    public GameObject currentCreaturePiece; //The board piece game object.
    public DungeonLord currentDungeonLord;
    public GameObject currentDungeonLordPiece;
    
    // what target out of what ever list is being accesssed is shown.
    int targetShown = 0; //Which of the creature chosens possible attack targets are being displayed.

    //Used to store the string value of openedFor, to tell the buttons what they need to do based on why the UI is open.
    string pressedFor;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CloseInspectWindow();
        levelManager = GameObject.FindGameObjectWithTag("LevelController");
    }

    #region displayAndHideUI
    public void CloseInspectWindow()
    {
        dungeonLordInspectUI.gameObject.SetActive(false);
        creatureInspectUI.gameObject.SetActive(false);
        inspectWindowButtons.declareBTN.SetActive(false);
        inspectWindowButtons.nextBTN.SetActive(false);
        inspectWindowButtons.backBTN.SetActive(false);
    }

    public void OpenInspectWindow(string openedFor)
    {
        pressedFor = openedFor;
        switch (openedFor)
        {
            case "DrawDice":
                DisplayCreatureDetails(openedFor);
                creatureInspectUI.SetActive(true);
                inspectWindowButtons.declareBTN.SetActive(true);
                inspectWindowButtons.nextBTN.SetActive(true);
                inspectWindowButtons.backBTN.SetActive(true);
                break;

            case "DieInspect":
                DisplayCreatureDetails(openedFor);
                creatureInspectUI.SetActive(true);
                break;

            case "PoolInspect":
                DisplayCreatureDetails(openedFor);
                creatureInspectUI.SetActive(true);
                break;

            case "PieceInspect":
                Debug.Log("Piece Inspect");
                DisplayCreatureDetails(openedFor);
                creatureInspectUI.SetActive(true);
                break;

            case "AttackTargetSelection":

                if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                {
                    //if  current target is dungeon lord then display dungeon lord.
                    DisplayDungeonLord();
                }else if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
                {
                    //If current target is creature then display creature
                    currentCreature = currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>().myCreature;
                    DisplayCreatureDetails(openedFor);
                }
                inspectWindowButtons.declareBTN.SetActive(true);
                inspectWindowButtons.nextBTN.SetActive(true);
                inspectWindowButtons.backBTN.SetActive(true);
                break;
        }  
    }

    public void DisplayCreatureDetails(string usedFor) //Set the inspect window components to display the details of current creatures
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
        creatureWindow.creatureArt.GetComponent<Image>().sprite = currentCreature.CardArt;
        creatureWindow.creatureLevel.GetComponent<Image>().sprite = currentCreature.LevelSprite;
        creatureWindow.creatureTribe.GetComponent<Image>().sprite = currentCreature.TribeSprite;
        creatureWindow.creatureType.GetComponent<Image>().sprite = currentCreature.TypeSprite;
        creatureWindow.creatureName.GetComponent<Text>().text = currentCreature.creatureType.ToString();
        creatureWindow.attackValue.GetComponent<Text>().text = "ATK" + currentCreature.Attack.ToString();
        creatureWindow.defenceValue.GetComponent<Text>().text = "DEF" + currentCreature.Defence.ToString();
        creatureWindow.healthValue.GetComponent<Text>().text = "HP" + currentCreature.Health.ToString();
    }

    public void DisplayDungeonLord()
    {
         
    }
    #endregion

    #region inspectBTNInput

    public void ButtonPressed()
    {
        string inspectBTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (inspectBTNPressed)
        {
            case "SelectCreatureBTN": // Only show this & toggle between BTN's when player has to select through the window and not other input.

                if (pressedFor == "DrawDice")
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
                } //Display BTN

                if (pressedFor == "AttackTargetSelection") //Display BTN
                {
                    if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                    {
                        DisplayDungeonLord();
                    }
                    else if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
                    {
                        CreatureIsAttackTarget();
                    }
                }
                break;

            case "ForwardBTN":

                if (pressedFor == "DrawDice")
                {
                    if (diceShown < turnPlayer.GetComponent<Player>().diceDeck.Count)
                    {
                        diceShown += 1;
                        DisplayCreatureDetails(pressedFor);
                    }
                }

                if (pressedFor == "AttackTargetSelection")
                {
                    if (targetShown < currentCreaturePiece.GetComponent<CreatureToken>().targets.Count - 1)
                    {
                        targetShown += 1;
                        if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                        {
                            DisplayDungeonLord();
                        }else if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
                        {
                            DisplayCreatureDetails(pressedFor);
                        }
                    }
                }
                break;

            case "BackBTN":
                if (pressedFor == "DrawDice")
                {
                    if (diceShown > 0)
                    {
                        diceShown -= 1;
                        DisplayCreatureDetails(pressedFor);
                    }
                }

                if (pressedFor == "AttackTargetSelection")
                {
                    if (targetShown > 0)
                    {
                        targetShown -= 1;
                        if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                        {
                            DisplayDungeonLord();
                        }
                        else if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
                        {
                            DisplayCreatureDetails(pressedFor);
                        }
                    }
                }
                break;

            case "CloseBTN": // Only for use in combat step.
                // go back to previous step.s
                levelManager.GetComponent<CreatureController>().ChosenAction = "Choosing";
                levelManager.GetComponent<CreatureController>().CheckPossibleActions();
                levelManager.GetComponent<CreatureController>().HideAndShowButtons();
                CloseInspectWindow();
                break;
        }
    }


    //This function is called when we want to show the inspect tab UI Object
  
    public void AddCreatureToPool() //Selected dice is discarded to add the creature inside to the player creature pool.
    {
        //add creature to pool
        levelManager.GetComponent<CreaturePoolController>().turnPlayer.GetComponent<Player>().CreaturePool.Add(currentCreature);
        levelManager.GetComponent<CreaturePoolController>().enableButtons();

        // remove the die object from the dice we are currently showing the contents of.
        sceneDice.GetComponent<SceneDieScript>().myDie = null;

        //Call the UIDICEController and run the reset function.
        levelManager.GetComponent<UIDiceController>().resetFunction();

        // hide the inspect tab
        CloseInspectWindow();

        // switch to the board view.
        levelManager.GetComponent<CameraController>().switchCamera("Alt");

        //Player not performing action can press end turn BTN.
        levelManager.GetComponent<LevelController>().turnPlayerPerformingAction = false;

    }
    #endregion

    
    //These two scripts go to the next step of combat if either a creature or dungeon lord is chosen
    public void CreatureIsAttackTarget()
    {
        //Declare creature in target list as attack target then send that information to creature vs creature combat script.
        GameObject CW = GameObject.FindGameObjectWithTag("CombatWindow");
        CW.GetComponent<AttackUIScript>().attacker = currentCreaturePiece;
        CW.GetComponent<AttackUIScript>().defender = currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].gameObject;
        CW.GetComponent<AttackUIScript>().Action = "Decide";
        CW.GetComponent<AttackUIScript>().displayAttackWindow();
        //Hide this Window for now
        CloseInspectWindow();
    }

    public void DungeonLordIsAttackTarget()
    {

    }
}
