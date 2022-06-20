using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [Header("Refrences")]
    GameObject mRef;
    public List<GameObject> DungeonLordTile = new List<GameObject>(); // the two grid tiles which 


    [Header("LevelCameras")]
    public Camera BoardCamera;
    public Camera FreeCamera;

    [Header("Game Participants")]
    // These two objects are prefabs for adding in the necessary number of player & or AI opponents to the game.
    public GameObject playerFab; 

    public GameObject opponent;

    //This list contains the opponent scriptable objects which contains the diffrent AI opponents the player can face, one will be added to the AI opponent object if present in the scene.
    public List<Opponent> opponentList = new List<Opponent>(); 

    //If either prefab are instaciated they will be added to this list as "Player1" and "Player2".
    public List<GameObject> participants = new List<GameObject>();

    [Header("Turn Manager")]
    public int turnCount = 1; // how many turns have passed.

    public string whoseTurn = "P1";

    public int turnPlayer = 0;

    public bool turnPlayerPerformingAction = false; // this bool will be what we check before being able to perform certain action.
                                                
    public void Awake()
    {
        mRef = GameObject.FindGameObjectWithTag("GameController");

    }

    public void Start()
    {
        setUpGame();        
    }

    public void Update()
    {
        
    }

    public void setUpGame() // this function is run first thing in order to control the rest of the scene.
    {
        // Check what Game Mode.
        switch (mRef.GetComponent<GameManagerScript>().gameMode)
        {
            case "Freeplay":
                switch (mRef.GetComponent<GameManagerScript>().desiredOpponent)
                {
                    case "Player":
                        // instanciate two playable objects
                        GameObject player1 = Instantiate(playerFab);
                        participants.Add(player1);
                        GameObject player2 = Instantiate(playerFab);
                        participants.Add(player2);
                        
                        break;

                    case "AI":

                        break;
                }
                break;
        }

        setDeck();
        SetTurnPlayer();
        this.GetComponent<ResourceUIManager>().updateCrests(); 

    }


    public void setDeck() 
    {
        for (int i = 0; i < participants.Count; i++)
        {
            if (participants[i].GetComponent<Player>() != null)
            {
                // add the chosen deck to that player object. ( We add each die to the player die list from the deckslot die list whose number is stored in int list decksinplay.
                for (int j = 0; j < mRef.GetComponent<DeckManager>().DeckSlots[mRef.GetComponent<DeckManager>().decksInPlay[i]].DeckDie.Count; j++)
                {
                    participants[i].GetComponent<Player>().diceDeck.Add(mRef.GetComponent<DeckManager>().DeckSlots[mRef.GetComponent<DeckManager>().decksInPlay[i]].DeckDie[j]);
                }
            }
        }
        this.GetComponent<ResourceUIManager>().BoardMang[0].GetComponent<Image>().enabled = true;
    }


    public void checkLevelState()
    {
        whoseTurn = "Player" + turnPlayer.ToString();
    }


     public void BeginTurnFunction() // this function is only called when it is a Human Players turn not the ai,
     {
        checkLevelState();
         // check if the player has any dice in the list 
         if (participants[turnPlayer].GetComponent<Player>().diceDeck.Count != 0)
         {
            // open the
            this.GetComponent<LevelController>().turnPlayerPerformingAction = true; // Player is in dice window, action being performed.
            this.GetComponent<ResourceUIManager>().whatUItoShow = "Dice";
            this.GetComponent<ResourceUIManager>().DiceWindow(); // display dice window.
            this.GetComponent<ResourceUIManager>().BoardMang[0].GetComponent<Image>().enabled = false; // hide start turn BTN
            this.GetComponent<DiceHandManager>().desiredDiceState = "Hidden";
            this.GetComponent<DiceHandManager>().diceShow();

            //Button Useability
            this.GetComponent<ResourceUIManager>().bugFix();
            this.GetComponent<ResourceUIManager>().DieMang[1].GetComponent<Button>().interactable = true;
            this.GetComponent<ResourceUIManager>().hasMulliganed = false;
         }
                 
         else if (participants[turnPlayer].GetComponent<Player>().diceDeck.Count != 0) // if no dice in pool proceed to board phase.
         {
             this.GetComponent<ResourceUIManager>().whatUItoShow = "Board";
             this.GetComponent<CameraController>().ActiveCam = "Alt";
             this.GetComponent<CameraController>().switchCamera();
         }
     }


    public void SetTurnPlayer() // call this function after turn player changes to update each script.
    {
        this.GetComponent<CreatureController>().turnPlayer = participants[turnPlayer].gameObject;
        this.GetComponent<DiceHandManager>().turnPlayer = participants[turnPlayer].gameObject;
        this.GetComponent<ResourceUIManager>().turnPlayer = participants[turnPlayer].gameObject;
        this.GetComponent<CreaturePoolController>().turnPlayer = participants[turnPlayer].gameObject;
        whoseTurn = "Player" + turnPlayer.ToString();
    }


     public void EndTurnFunction()
     {
        if (this.GetComponent<LevelController>().turnPlayerPerformingAction == false) { // if current player isnt in the middle of an action eg moving a piece or combat end turn.

            if (mRef.GetComponent<GameManagerScript>().desiredOpponent == "Player")
            {
                switch (whoseTurn)
                {
                    case "Player0":
                        whoseTurn = "Player1";
                        turnPlayer = 1;
                        SetTurnPlayer();
                        break;

                    case "Player1":
                        whoseTurn = "Player0";
                        turnPlayer = 0;
                        SetTurnPlayer();
                        break;
                }

                this.GetComponent<CreatureController>().ResetCreatureStates();
                this.GetComponent<CameraController>().ActiveCam = "Board";
                this.GetComponent<CameraController>().switchCamera();
                this.GetComponent<ResourceUIManager>().BoardMang[0].GetComponent<Image>().enabled = true;
            }
        }

        /* if (mRef.GetComponent<GameManagerScript>().desiredOpponent == "AI")
         {
             switch (whoseTurn)
             {
                 case "AI":
                     this.GetComponent<ResourceUIManager>().BoardMang[0].GetComponent<Image>().enabled = true;
                     break;

                 case "Player":
                     if (this.GetComponent<LevelController>().playerPerformingAction == false)
                     {

                         this.GetComponent<CreatureController>().ResetCreatureStates();
                         this.GetComponent<CameraController>().ActiveCam = "Board";
                         this.GetComponent<CameraController>().switchCamera();
                         whoseTurn = "AI";
                         GameObject.FindGameObjectWithTag("AIController").GetComponent<AIController>().BeginTurn();
                     }
                     break;
             }
         }*/
    }        
}
