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

    // the gameobject of the creature piece we have selected.
    public GameObject ChosenCreatureToken; 

    //What creature action BTN we have pressed.
    public string ChosenAction = "None";
    public bool ableToMove = false; //Checks if there is a possible tile to move into should we have the crests to afford to move.


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
                OrderBTNS[i].GetComponent<Button>().interactable = false;
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

    public void CheckPossibleActions() 
    {
        if (ChosenAction == "None")
        {
            //Check if the creature can be moved & if the player can pay its cost.
            if (ChosenCreatureToken.GetComponent<CreatureToken>().hasMovedThisTurn == false)
            {
                if (ableToMove == true && turnPlayer.GetComponent<Player>().moveCrestPoints >= ChosenCreatureToken.GetComponent<CreatureToken>().moveCost)
                {
                    OrderBTNS[0].GetComponent<Button>().interactable = true;
                }
            }
            else if (ChosenCreatureToken.GetComponent<CreatureToken>().hasMovedThisTurn == true)
            {
                OrderBTNS[0].GetComponent<Button>().interactable = false;
            }

            //Check if the creature can attack & if the player can pay its cost.
            if (ChosenCreatureToken.GetComponent<CreatureToken>().targets.Count != 0 && ChosenCreatureToken.GetComponent<CreatureToken>().hasAttackedThisTurn == false)
            {
                Debug.Log("Has Target");
                if (turnPlayer.GetComponent<Player>().attackCrestPoints >= ChosenCreatureToken.GetComponent<CreatureToken>().attackCost)
                {
                    Debug.Log("Has Attack Crests");
                    OrderBTNS[1].GetComponent<Button>().interactable = true;
                }
            }
            else if (ChosenCreatureToken.GetComponent<CreatureToken>().targets.Count == 0 || ChosenCreatureToken.GetComponent<CreatureToken>().hasAttackedThisTurn == true)
            {
                OrderBTNS[1].GetComponent<Button>().interactable = false;
            }

            // check if the creature has an ability the player can use & can pay the ability crest cost.
            Ability myAbility = ChosenCreatureToken.GetComponent<CreatureToken>().myCreature.myAbility;
            if (myAbility != null)
            {
                if (myAbility.abilityActivatedHow == Ability.AbilityActivatedHow.Activated)
                {
                    if (turnPlayer.GetComponent<Player>().abiltyPowerCrestPoints >= myAbility.abilityCost && ChosenCreatureToken.GetComponent<CreatureToken>().hasUsedAbilityThisTurn == false)
                    {
                        //ChosenCreatureToken.GetComponent<AbilityManager>().checkingCanCast = true;
                        //ChosenCreatureToken.GetComponent<AbilityManager>().CheckAbilityCanCast();
                        if (ChosenCreatureToken.GetComponent<AbilityManager>().canBeCast == true)
                        {
                            //ChosenCreatureToken.GetComponent<AbilityManager>().checkingCanCast = false;
                            OrderBTNS[2].GetComponent<Button>().interactable = true;
                        }
                    }
                    else if (turnPlayer.GetComponent<Player>().abiltyPowerCrestPoints < myAbility.abilityCost || ChosenCreatureToken.GetComponent<CreatureToken>().hasUsedAbilityThisTurn == true || ChosenCreatureToken.GetComponent<AbilityManager>().canBeCast == false)
                    {
                        Debug.Log("Cant Afford To Cast Or has cast this turn, or cant be cast");
                        OrderBTNS[2].GetComponent<Button>().interactable = false;
                    }
                }
                else if (myAbility.abilityActivatedHow != Ability.AbilityActivatedHow.Activated)
                {
                    Debug.Log("Cant Use it not activatable");
                    OrderBTNS[2].GetComponent<Button>().interactable = false;
                }
            }
            // Enable the cancle button.
            OrderBTNS[3].GetComponent<Button>().interactable = true;
        }
        else
        {
            OrderBTNS[0].GetComponent<Button>().interactable = false;
            OrderBTNS[1].GetComponent<Button>().interactable = false;
            OrderBTNS[2].GetComponent<Button>().interactable = false;
            OrderBTNS[3].GetComponent<Button>().interactable = true;
        }
    } 

    public void DeclareAction()
    {
        string actionBTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (actionBTNPressed)
        {
            case "Move":
                if (ChosenCreatureToken.GetComponent<CreatureToken>().hasMovedThisTurn == false)
                {
                    lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                    ChosenAction = "Move";
                    lvlRef.GetComponent<PathController>().DeclarePathfindingConditions(ChosenCreatureToken, "Move");
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
                CheckPossibleActions();
                OpenAndCloseControllerUI();
                GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().currentCreature = ChosenCreatureToken;
                GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().ShowAndUpdateInterface("Start");
                ChosenCreatureToken.GetComponent<AbilityManager>().StartCoroutine("ActivateEffect");
                // Call Ability manager & Do things.
                break;

            case "Back":
                switch (ChosenAction)
                {
                    case "None":
                        //Close Creature Order UI.
                        CancleBTNFunction();
                        break;
                    case "Ability":
                        ChosenAction = "None";
                        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().HideInterface();
                        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().CancleAbility();
                        break;
                    case "Attack":
                        //Go back from select attack target in inspect window.
                        break;
                    case "Move":
                        //Cancle Move
                        lvlRef.GetComponent<PathController>().ResetBoard("Reset");
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

    public void ResetCreatureStates()
    {
        for (int i = 0; i < CreaturesOnBoard.Count; i++)
        {
            CreaturesOnBoard[i].GetComponent<CreatureToken>().hasMovedThisTurn = false;
            CreaturesOnBoard[i].GetComponent<CreatureToken>().hasAttackedThisTurn = false;
            CreaturesOnBoard[i].GetComponent<CreatureToken>().hasUsedAbilityThisTurn = false;
            lcScript.ableToInteractWithBoard = false;
        }
    }
}
