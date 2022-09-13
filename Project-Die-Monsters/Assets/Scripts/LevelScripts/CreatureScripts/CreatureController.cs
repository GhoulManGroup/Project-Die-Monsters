using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreatureController : MonoBehaviour //  this script oversees piece movement on the board for all player participants.
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
    public GameObject ChosenCreature; 

    //What creature action BTN we have pressed.
    public string ChosenAction = "None";


    public void Start()
    {
        mRef = GameObject.FindGameObjectWithTag("GameController");
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
        lcScript = lvlRef.GetComponent<LevelController>();
        HideAndShowButtons();
    }
    // Update is called once per frame
    void Update()
    {
        MoveInput();
    }

    public void HideAndShowButtons()
    {
        if (ChosenCreature != null) // if the player has picked a creature token to select hide and display creature control UI buttons.
        {
            for (int i = 0; i < OrderBTNS.Count; i++)
            {
                OrderBTNS[i].GetComponent<Image>().enabled = true;
            }
            CheckPossibleActions();
        }

        else if (ChosenCreature == null) // close the UI.
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

        //If the player has enough move crests to pay the cost of moving this creature a tile then enable this button.
        if (turnPlayer.GetComponent<Player>().moveCrestPoints >= ChosenCreature.GetComponent<CreatureToken>().moveCost)
        {
            if (ChosenCreature.GetComponent<CreatureToken>().HasMovedThisTurn == false)
            {
                OrderBTNS[0].GetComponent<Button>().interactable = true;
            }
        }
        // Enable the attack button if the player has enough attack crests to pay the cost of an attack
        if (turnPlayer.GetComponent<Player>().attackCrestPoints >= ChosenCreature.GetComponent<CreatureToken>().attackCost)
        {
            //Check if the target creature has any nearby targets and hasn't already attacked this turn.
            if (ChosenCreature.GetComponent<CreatureToken>().targets.Count != 0 && ChosenCreature.GetComponent<CreatureToken>().HasAttackedThisTurn == false)
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
                if (ChosenCreature.GetComponent<CreatureToken>().HasMovedThisTurn == false)
                {
                    lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                    ChosenAction = "Move";
                }               
                break;

            case "Attack":
                //Declare attack action to be made open the inspect window for target selection.
                ChosenAction = "Attack";

                lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().currentCreatureToken = ChosenCreature;
                GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().OpenInspectWindow("AttackTargetSelection");
                CheckPossibleActions();
                HideAndShowButtons();
                break;

            case "Ability":
                ChosenAction = "Ability";
                lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                break;

            case "Cancle":
                CancleBTNFunction();
                break;
        }
        HideAndShowButtons();
    }

    public void CancleBTNFunction()
    {
        ChosenAction = "None";
        lcScript.ableToInteractWithBoard = true;
        ChosenCreature = null;
        HideAndShowButtons();
        lvlRef.GetComponent<CameraController>().ActiveCam = "Alt";
        lvlRef.GetComponent<CameraController>().switchCamera();
        lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false;
    }

    public void MoveInput()
    {
        
        if (ChosenAction == "Move")
        {
            if (Input.GetKeyDown("w"))
            {
                ChosenCreature.GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().desiredDir = "Up";
                ChosenCreature.GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().CheckIfMovePossible();
            }

            if (Input.GetKeyDown("s"))
            {
                ChosenCreature.GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().desiredDir = "Down";
                ChosenCreature.GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().CheckIfMovePossible();
            }

            if (Input.GetKeyDown("a"))
            {
                ChosenCreature.GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().desiredDir = "Left";
                ChosenCreature.GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().CheckIfMovePossible();
            }

            if (Input.GetKeyDown("d"))
            {
                ChosenCreature.GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().desiredDir = "Right";
                ChosenCreature.GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().CheckIfMovePossible();
            }

        }

    }


  public void subtractCrest() // remove the cost of action from the pool.
    {
        switch (ChosenAction)
        {
            case "Move":
                turnPlayer.GetComponent<Player>().moveCrestPoints -= ChosenCreature.GetComponent<CreatureToken>().moveCost;
                ChosenCreature.GetComponent<CreatureToken>().HasMovedThisTurn = true;

                // Check if player can afford to move creature again after this movement.
                if (turnPlayer.GetComponent<Player>().moveCrestPoints < ChosenCreature.GetComponent<CreatureToken>().moveCost)
                {
                    //Creature can't be moved again end the move state, and disable the move action button.
                    ChosenAction = "None";
                }

                ChosenCreature.GetComponent<CreatureToken>().CheckForAttackTarget();
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
