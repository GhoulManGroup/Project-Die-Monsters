using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InspectWindowController : MonoBehaviour //This script controls the various non combat in-Level UI windows that display the details of creatures, dungeonlords, items ect.
{
    //The used for string(usedFor) within some functions is where we store why the inspect interface was opened and how the script knows what parts of the UI to enable / disable ect at any point based on what is needed.
    // "DrawDice", Display the current turn players dice pool to allow them to assign it to the 3DDice fab to roll.
    // "DieInspect, view what creature is inside of clicked dice that has been rolled and can be summoned for choosing.
    // "PoolInspect", View what creature is currently stored within the creature pool UI object we are hovering over.
    // "PieceInspect", view the details of the board piece the player is hovering over
    //AttackTargetSelection, display the details of the possible attack targets of chosen piece.

    [Header("Interface UI Panels")] //The Game Objects that comprise the inspect UI.
    public GameObject CreatureInspectUI; //Parent of CIUI
    public GameObject DungeonLordInspectUI; //Parent of DLIUI
    public List<GameObject> InspectBTNList = new List<GameObject>();
    public GameObject CrestDisplay; //Shows what crests a dice in the pool has.

    [Space(10)]

    [Header("Interface UI Components")] //The Components that comprise the Inspect UI Elements eg BTN / Image / Text of the creature inspect window
    public List<GameObject> CreatureInspectionPanel = new List<GameObject>();
    public GameObject creatureArt;
    public GameObject creatureLevel;
    public GameObject creatureTribe;
    public GameObject creatureType;
    public Text creatureName;
    public Text attackValue;
    public Text defenceValue;
    public Text healthValue;
    public List<GameObject> DungeonLordInspectionPanel = new List<GameObject>();

 

    [Header("Window States")]


    [Header("DiceWindowRefrence")]
    //These two public objects are for assigning a die object to a 3D dice in the scene.
    public GameObject sceneDice;
    public GameObject turnPlayer;
    GameObject lvlRef;
    //What dice out of the player dice deck is currently displayed.
    int diceShown = 0;

    public Creature currentCreature; //The creature scriptable object contained with a game object, eg dice or board piece.
    public GameObject currentCreaturePiece; //The board piece game object.
    public DungeonLord currentDungeonLord;
    public GameObject currentDungeonLordPiece;
    
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

                if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                {
                    //if  current target is dungeon lord then display dungeon lord.
                    DisplayDungeonLord();
                }else if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
                {
                    //If current target is creature then display creature
                    currentCreature = currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>().myCreature;
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
            case "SelectCreatureBTN": // Only show this & toggle between BTN's when player has to select through the window and not other input.

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
                } //Display BTN

                if (usedFor == "AttackTargetSelection") //Display BTN
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
                    if (targetShown < currentCreaturePiece.GetComponent<CreatureToken>().targets.Count - 1)
                    {
                        targetShown += 1;
                        if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                        {
                            DisplayDungeonLord();
                        }else if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
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
                        if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<DungeonLordPiece>() != null)
                        {
                            DisplayDungeonLord();
                        }
                        else if (currentCreaturePiece.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>() != null)
                        {
                            DisplayCreatureDetails(usedFor);
                        }
                    }
                }
                break;

            case "CloseBTN": // Only for use in combat step.
                // go back to previous step.s
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
        lvlRef.GetComponent<CameraController>().switchCamera("Alt");

        //Player not performing action can press end turn BTN.
        lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false;

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
