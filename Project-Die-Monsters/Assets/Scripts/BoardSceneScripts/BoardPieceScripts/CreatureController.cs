using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreatureController : MonoBehaviour //This script managers the UI panel which allows players to select the desired action they wish to take with the chosen piece.
{
    [Header ("List of all Creatures on Board")]
   public List <GameObject> CreaturesOnBoard = new List<GameObject>(); // this list will store all the creatures on the board so I can reset their has moved or has used ability booleans.

    [Header("Refrences")]
    public GameObject turnPlayer;
    GameObject mRef; //Game Manager
    GameObject lvlRef;
    LevelController lcScript;

    [Header("Order UI")]
    public List <GameObject> OrderBTNS = new List<GameObject>();

    // locks us from interacting with board piece when we aren't allowed.


    //What type of piece has the player picked.
    string PieceType = "None"; // creature // Dungeon Lord

    // the gameobject of the creature piece we have selected.
    public GameObject ChosenCreatureToken; 

    //What creature action BTN we have pressed.
    public string ChosenAction = "None";


    public void Start()
    {
        mRef = GameObject.FindGameObjectWithTag("GameController");
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
        lcScript = lvlRef.GetComponent<LevelController>();
        ChosenAction = "None";
        OpenAndCloseControllerUI();
    }

    public void OpenAndCloseControllerUI()
    {//Hide and Display this interface depending on if there is a creature selected.
        if (ChosenCreatureToken != null) 
        {
            for (int i = 0; i < OrderBTNS.Count; i++)
            {
                OrderBTNS[i].GetComponent<Image>().enabled = true;
            }
            CheckPossibleActions();
        }

        else if (ChosenCreatureToken == null) // close the UI.
        {
            for (int i = 0; i < OrderBTNS.Count; i++)
            {
                OrderBTNS[i].GetComponent<Image>().enabled = false;
            }
        }
        
    }

    public void CheckPossibleActions() // check what UI Buttons to enable;
    {
        for (int i = 0; i < OrderBTNS.Count; i++)
        {
            OrderBTNS[i].GetComponent<Button>().interactable = false;
        }
        //Add a condition first to see if there is even a vaild tile to move into before giving this options.
        //If the player has enough move crests to pay the cost of moving this creature a tile then enable this button.
        if (turnPlayer.GetComponent<Player>().moveCrestPoints >= ChosenCreatureToken.GetComponent<CreatureToken>().moveCost)
        {
            if (ChosenCreatureToken.GetComponent<CreatureToken>().HasMovedThisTurn == false)
            {
                OrderBTNS[0].GetComponent<Button>().interactable = true;
            }
        }
        // Enable the attack button if the player has enough attack crests to pay the cost of an attack
        if (turnPlayer.GetComponent<Player>().attackCrestPoints >= ChosenCreatureToken.GetComponent<CreatureToken>().attackCost)
        {
            //Check if the target creature has any nearby targets and hasn't already attacked this turn.
            if (ChosenCreatureToken.GetComponent<CreatureToken>().targets.Count != 0 && ChosenCreatureToken.GetComponent<CreatureToken>().HasAttackedThisTurn == false)
            {
                if (ChosenAction != "Attack")
                {
                    OrderBTNS[1].GetComponent<Button>().interactable = true;
                }
            }
        }

        //Add a check for ability later when we get into that step of development.
        if (turnPlayer.GetComponent<Player>().abiltyPowerCrestPoints != 0)
        {
            OrderBTNS[2].GetComponent<Button>().interactable = true;
        }

        // Enable the cancle button.
        OrderBTNS[3].GetComponent<Button>().interactable = true;

        
    } 

    public void DeclareAction()
    {
        string actionBTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (actionBTNPressed)
        {
            case "Move":
                if (ChosenCreatureToken.GetComponent<CreatureToken>().HasMovedThisTurn == false)
                {
                    lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                    ChosenAction = "Move";
                    lvlRef.GetComponent<PathController>().DeclareConditions(ChosenCreatureToken, "Move");
                }
                break;

            case "Attack":
                //Declare attack action to be made open the inspect window for target selection.
                ChosenAction = "Attack";

                lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().currentCreaturePiece = ChosenCreatureToken;
                GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().OpenInspectWindow("AttackTargetSelection");
                CheckPossibleActions();
                OpenAndCloseControllerUI();
                break;

            case "Ability":
                ChosenAction = "Ability";
                lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                break;

            case "Back":
                switch (ChosenAction)
                {
                    case "None":
                        //Close Creature Order UI.
                        Debug.Log("None");
                        CancleBTNFunction();
                        break;
                    case "Ability":
                        //Close ability UI?
                        break;
                    case "Attack":
                        //Go back from select attack target in inspect window.
                        break;
                    case "Move":
                        //Cancle Move
                        Debug.Log("MoveCancle");
                        lvlRef.GetComponent<PathController>().ResetBoard();
                        ChosenAction = "None";
                        break;
                }
                break;
        }

        OpenAndCloseControllerUI();
    }

    public void CancleBTNFunction()
    {
        ChosenAction = "None";
        lcScript.ableToInteractWithBoard = true;
        ChosenCreatureToken = null;
        OpenAndCloseControllerUI();
        lvlRef.GetComponent<CameraController>().switchCamera("Alt");
        lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false;
    }

    public void subtractCrest() // remove the cost of action from the pool.
    {
        switch (ChosenAction)
        {
            case "Move":
                turnPlayer.GetComponent<Player>().moveCrestPoints -= ChosenCreatureToken.GetComponent<CreatureToken>().moveCost;
                ChosenCreatureToken.GetComponent<CreatureToken>().HasMovedThisTurn = true;

                // Check if player can afford to move creature again after this movement.
                if (turnPlayer.GetComponent<Player>().moveCrestPoints < ChosenCreatureToken.GetComponent<CreatureToken>().moveCost)
                {
                    //Creature can't be moved again end the move state, and disable the move action button.
                    ChosenAction = "None";
                }

                ChosenCreatureToken.GetComponent<CreatureToken>().CheckForAttackTarget();
                CheckPossibleActions();
                break;
        }
    }

    public void ResetCreatureStates()
    {
        for (int i = 0; i < CreaturesOnBoard.Count; i++)
        {
            CreaturesOnBoard[i].GetComponent<CreatureToken>().HasMovedThisTurn = false;
            CreaturesOnBoard[i].GetComponent<CreatureToken>().HasAttackedThisTurn = false;
            CreaturesOnBoard[i].GetComponent<CreatureToken>().hasUsedAbilityThisTurn = false;
            lcScript.ableToInteractWithBoard = false;
        }
    }
}
