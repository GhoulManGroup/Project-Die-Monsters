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
    GameObject mRef;
    GameObject lVLRef;

    [Header("Order UI")]
    public List <GameObject> OrderBTNS = new List<GameObject>();


    public bool PiecePicked = false;
    string PieceType = "None"; // creature // Dungeon Lord
    public GameObject ChosenCreature; // the creature piece we have selected.
    public string ChosenAction = "None";
    public bool attackWindowOpen = false;


    public void Start()
    {
        HideAndShowButtons();
        mRef = GameObject.FindGameObjectWithTag("GameController");
        lVLRef = GameObject.FindGameObjectWithTag("LevelController");
    }
    // Update is called once per frame
    void Update()
    {
        MoveInput();
    }

    public void HideAndShowButtons()
    {
        if (PiecePicked == true) // if the player has picked a creature token to select hide and display creature control UI buttons.
        {
            switch (ChosenAction)
            {
                case "Choosing":
                    for (int i = 0; i < OrderBTNS.Count; i++)
                    {
                        OrderBTNS[i].GetComponent<Image>().enabled = true;
                    }
                    break;

                case "Moving":
                    for (int i = 0; i < OrderBTNS.Count; i++)
                    {
                        OrderBTNS[i].GetComponent<Image>().enabled = true;
                    }
                    break;

                case "Attack":
                    for (int i = 0; i < OrderBTNS.Count; i++)
                    {
                        OrderBTNS[i].GetComponent<Image>().enabled = false;
                    }
                    break;
            }
           CheckPossibleActions();
        }

        else if (PiecePicked == false) // close the UI.
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
            if (attackWindowOpen == false)
            {
                if (ChosenCreature.GetComponent<CreatureToken>().HasMovedThisTurn == false)
                {
                    OrderBTNS[0].GetComponent<Button>().interactable = true;
                }
            }
        }
        // Enable the attack button if the player has enough attack crests to pay the cost of an attack, & the creature pieces target list.count isnt 0.
        if (turnPlayer.GetComponent<Player>().attackCrestPoints >= ChosenCreature.GetComponent<CreatureToken>().attackCost)
        {
            if (ChosenCreature.GetComponent<CreatureToken>().targets.Count != 0)
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
                    lVLRef.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                    ChosenAction = "Move";
                }               
                break;

            case "Attack":
                //Declare attack action to be made open the inspect window for target selection.
                ChosenAction = "Attack";
                lVLRef.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().usedFor = "AttackTargetSelection";
                GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().CurrentCreatureToken = ChosenCreature;
                GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().ShowFunction();
                CheckPossibleActions();
                HideAndShowButtons();
                break;

            case "Ability":
                ChosenAction = "Ability";
                lVLRef.GetComponent<LevelController>().turnPlayerPerformingAction = true;
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
        PiecePicked = false;
        attackWindowOpen = false;
        ChosenCreature = null;
        HideAndShowButtons();
        lVLRef.GetComponent<CameraController>().ActiveCam = "Alt";
        lVLRef.GetComponent<CameraController>().switchCamera();
        lVLRef.GetComponent<LevelController>().turnPlayerPerformingAction = false;
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
                lVLRef.GetComponent<ResourceUIManager>().updateCrests();
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
            case "Attack":

                break;
        }
    }

    public void ResetCreatureStates()
    {
        for (int i = 0; i < CreaturesOnBoard.Count; i++)
        {
            CreaturesOnBoard[i].GetComponent<CreatureToken>().HasMovedThisTurn = false;
        }
    }


}
