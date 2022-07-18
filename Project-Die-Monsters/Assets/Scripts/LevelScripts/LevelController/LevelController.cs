using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour //This class controls everything at the board scene level of the project.
{
    [Header("Refrences")]
    //The projects game manager object which persists between scences.
    GameObject gameManager;
    public GameObject startTurnBTN;
    public GameObject endTurnBTN;

    // The two grid tiles which are the dungeon lord starting positions.
    public List<GameObject> DungeonLord = new List<GameObject>();

    [Header("Game Participants")]
    // These two prefabs are the default player or AI opponent to be loaded into the level depending on who the opponent faces.
    public GameObject playerFab; 
    public GameObject opponent;

    //This list contains the opponent scriptable objects which contains the diffrent AI opponents the player can face, one will be added to the AI opponent object if present in the scene.
    public List<Opponent> opponentList = new List<Opponent>(); 

    //If either prefab are instaciated they will be added to this list as "Player1" and "Player2".
    public List<GameObject> participants = new List<GameObject>();

    [Header("Turn Tracking")]
    public int turnCount = 1; // how many turns have passed.

    //which participant is the turn player.
    public string whoseTurn = "P1";
    public int turnPlayer = 0;

    [Header("Turn Player Constraints")]
    public bool turnPlayerPerformingAction = false; // this bool will be what we check before being able to perform certain action.
    public bool ableToInteractWithBoard = false; // Player is allowed to interact with the board.

    public void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");

    }

    public void Start()
    {
        setUpGame();        
    }

    public void setUpGame() // this function is run first thing in order to control the rest of the scene.
    {
        // Check what Game Mode.
        switch (gameManager.GetComponent<GameManagerScript>().gameMode)
        {
            case "Freeplay":
                switch (gameManager.GetComponent<GameManagerScript>().desiredOpponent)
                {
                    case "Player":
                        // instanciate two playable objects
                        GameObject player1 = Instantiate(playerFab);
                        participants.Add(player1);
                        GameObject player2 = Instantiate(playerFab);
                        participants.Add(player2);

                        //Setup the Dungeon Lords
                        DungeonLord[0].GetComponent<DungeonLord>().myOwner = "Player0";
                        DungeonLord[1].GetComponent<DungeonLord>().myOwner = "Player1";

                        DungeonLord[0].GetComponent<DungeonLord>().SetDungeonLordTile();
                        DungeonLord[1].GetComponent<DungeonLord>().SetDungeonLordTile();

                        break;

                    case "AI":

                        break;
                }
                break;
        }

        //Set the player decks.
        setDeck();

        //Setup turn player.
        SetTurnPlayer();

        //Update starting players crest UI.
        //this.GetComponent<ResourceUIManager>().updateCrests(); 

    }


    public void setDeck() 
    {
        // Adds a copy of the two chosen decks stored in the deck manager to the player objects own die lists.
        for (int i = 0; i < participants.Count; i++)
        {
            if (participants[i].GetComponent<Player>() != null)
            {
                // add the chosen deck to that player object. ( We add each die to the player die list from the deckslot die list whose number is stored in int list decksinplay.
                for (int j = 0; j < gameManager.GetComponent<DeckManager>().DeckSlots[gameManager.GetComponent<DeckManager>().decksInPlay[i]].DeckDie.Count; j++)
                {
                    participants[i].GetComponent<Player>().diceDeck.Add(gameManager.GetComponent<DeckManager>().DeckSlots[gameManager.GetComponent<DeckManager>().decksInPlay[i]].DeckDie[j]);
                }
            }
        }

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
            this.GetComponent<LevelController>().turnPlayerPerformingAction = true; // Player is in dice window, action being performed.
            GetComponent<CameraController>().ActiveCam = "Dice";
            GetComponent<CameraController>().switchCamera();
            GetComponent<UIDiceController>().SetUp();
            
        }
                 
         else if (participants[turnPlayer].GetComponent<Player>().diceDeck.Count == 0) // if no dice in pool proceed to board phase.
         {
             this.GetComponent<CameraController>().ActiveCam = "Alt";
             this.GetComponent<CameraController>().switchCamera();
             ableToInteractWithBoard = true;
         }

        startTurnBTN.GetComponent<Image>().enabled = false;
     }


    public void SetTurnPlayer() // call this function after turn player changes to update each script.
    {
        this.GetComponent<CreatureController>().turnPlayer = participants[turnPlayer].gameObject;
        this.GetComponent<CreaturePoolController>().turnPlayer = participants[turnPlayer].gameObject;
        this.GetComponent<UIDiceController>().turnPlayer = participants[turnPlayer].gameObject;
        GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().turnPlayer = participants[turnPlayer].gameObject;
        whoseTurn = "Player" + turnPlayer.ToString();

    }


     public void EndTurnFunction()
     {
        if (this.GetComponent<LevelController>().turnPlayerPerformingAction == false) { // if current player isnt in the middle of an action eg moving a piece or combat end turn.

            //Player vs Player
            if (gameManager.GetComponent<GameManagerScript>().desiredOpponent == "Player")
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

                ResetFunction();
                startTurnBTN.GetComponent<Image>().enabled = true;
            }
            //Player VS AI
        }
    } 
    
    public void ResetFunction()
    {
        //Run cancle on end turn to close the control UI if it is still open.
        this.GetComponent<CreatureController>().CancleBTNFunction();

        //Reset the board tile dungeon connection state.
        GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().UpdateBoard();

        //Set the player turn UI to next turn player.
       // this.GetComponent<ResourceUIManager>().updateLifeText();

        //Reset the states of all creature piece on the board (Move, Attack, ect)
        this.GetComponent<CreatureController>().ResetCreatureStates();

        //Set the camera to the right board state.
        this.GetComponent<CameraController>().ActiveCam = "Alt";
        this.GetComponent<CameraController>().switchCamera();
    }
}
