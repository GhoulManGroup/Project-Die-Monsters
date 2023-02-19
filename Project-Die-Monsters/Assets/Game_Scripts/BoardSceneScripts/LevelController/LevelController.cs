using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager.UI;

public class LevelController : MonoBehaviour //This class controls everything at the board scene level of the project.
{
    [Header("Refrences")]
    //The projects game manager object which persists between scences.
    GameObject gameManager;
    public GameObject startTurnBTN;
    public GameObject endTurnBTN;
    public GameObject currentGameEndScreen;
    public List<GameObject> DungeonLordStartTiles = new List<GameObject>();

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
    public GameObject turnPlayerObject;
    public int turnPlayerSlot = 0;

    [Header("Turn Player Constraints & Resources")] // Tweak constraints.

    public bool turnPlayerPerformingAction = false; //Is the player currently doing somthing.
    public bool ableToInteractWithBoard = false; // Player is allowed to interact with the pieces on the board, checked before raycast on piece ect.
    public string boardInteraction = "None"; //What action the player is taking
    
    [Header("Creature  ")]
    public bool placingCreature = false; // The player is placing a creature on the board
    public string creaturePlacedFrom = "None"; // DiceZone, Creature Pool, Other?

    public List<Text> turnPlayerUIDisplay = new List<Text>();

    #region GameSetUp
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    private void Start()
    {
        setUpGame();        
    }

    public void setUpGame() // this function is run first thing in order to control the rest of the scene.
    {
        QualitySettings.vSyncCount = 1;
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
                        DungeonLordStartTiles[0].GetComponent<DungeonLordPiece>().myOwner = "Player0";
                        DungeonLordStartTiles[1].GetComponent<DungeonLordPiece>().myOwner = "Player1";

                        DungeonLordStartTiles[0].GetComponent<DungeonLordPiece>().SetDungeonLordTile();
                        DungeonLordStartTiles[1].GetComponent<DungeonLordPiece>().SetDungeonLordTile();

                    
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

    }

    private void setDeck()
    {
        // Adds a copy of the two chosen decks stored in the deck manager to the player objects own die lists to play with this match.
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

    #endregion

    #region TurnManagement
    public void SetTurnPlayer() // call this function after turn player changes to update each script.
    {
        this.GetComponent<CreatureController>().turnPlayer = participants[turnPlayerSlot].gameObject;
        this.GetComponent<CreaturePoolController>().turnPlayer = participants[turnPlayerSlot].gameObject;
        this.GetComponent<UIDiceController>().turnPlayer = participants[turnPlayerSlot].gameObject;
        GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().turnPlayer = participants[turnPlayerSlot].gameObject;
        turnPlayerObject = participants[turnPlayerSlot].gameObject;
        whoseTurn = "Player" + turnPlayerSlot.ToString();
    }

    public void BeginTurnFunction() // this function is only called when it is a Human Players turn not the ai,
     {
        // check if the player has any dice in the list 
        if (participants[turnPlayerSlot].GetComponent<Player>().diceDeck.Count != 0)
         {           
            this.GetComponent<LevelController>().turnPlayerPerformingAction = true; // Player is in dice window, action being performed.
            GetComponent<CameraController>().switchCamera("Dice");
            GetComponent<UIDiceController>().SetUp();       
        }
                 
         else if (participants[turnPlayerSlot].GetComponent<Player>().diceDeck.Count == 0) // if no dice in pool proceed to board phase.
         {
             this.GetComponent<CameraController>().switchCamera("Alt");
             ableToInteractWithBoard = true;
         }

        startTurnBTN.GetComponent<Image>().enabled = false;
        participants[turnPlayerSlot].GetComponent<Player>().moveCrestPoints += 1;
        UpdateTurnPlayerCrestDisplay();
     }

    public void UpdateTurnPlayerCrestDisplay()
    {
        turnPlayerUIDisplay[0].text = "Turn Player = " + turnPlayerSlot.ToString();
        turnPlayerUIDisplay[1].text = participants[turnPlayerSlot].GetComponent<Player>().attackCrestPoints.ToString();
        turnPlayerUIDisplay[2].text = participants[turnPlayerSlot].GetComponent<Player>().abiltyPowerCrestPoints.ToString();
        turnPlayerUIDisplay[3].text = participants[turnPlayerSlot].GetComponent<Player>().defenceCrestPoints.ToString();
        turnPlayerUIDisplay[4].text = participants[turnPlayerSlot].GetComponent<Player>().moveCrestPoints.ToString();
    }

    public void CheckForTriggersToResolve()
    {
        AbilityUIController AbilityWindow = GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>();
        if (AbilityWindow.creaturesToTrigger.Count != 0 && turnPlayerPerformingAction == false)
        {
            ableToInteractWithBoard = false;
            Debug.Log("Going To Trigger Abilties Now");
            AbilityWindow.StartCoroutine("StackManager");
        }
        else
        {
            Debug.Log("No Triggers To Resolve " + turnPlayerPerformingAction + " " + AbilityWindow.creaturesToTrigger.Count);
            GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().CheckCreatureStates();
        }
    }
    public void EndTurnFunction()
     {
        //Ensure that player isnt currently doing somthing.
        StartCoroutine("EndTurn");
     } 

    private IEnumerator EndTurn()
    {
        for (int i = 0; i < this.GetComponent<CreatureController>().CreaturesOnBoard.Count; i++)
        {
            if (this.GetComponent<CreatureController>().CreaturesOnBoard[i].GetComponent<AbilityManager>().myAbility.abilityActivatedHow == Ability.AbilityActivatedHow.Trigger)
            {
                this.GetComponent<CreatureController>().CreaturesOnBoard[i].GetComponent<AbilityManager>().CheckTrigger("OnEndTurn");
            }
        }

        CheckForTriggersToResolve();

        while (GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().creaturesToTrigger.Count > 0)
        {
            yield return null;
        }

        if (this.GetComponent<LevelController>().turnPlayerPerformingAction == false)
        { // if current player isnt in the middle of an action eg moving a piece or combat end turn.

            //Player vs Player
            if (gameManager.GetComponent<GameManagerScript>().desiredOpponent == "Player")
            {
                switch (whoseTurn)
                {
                    case "Player0":
                        whoseTurn = "Player1";
                        turnPlayerSlot = 1;
                        SetTurnPlayer();
                        break;

                    case "Player1":
                        whoseTurn = "Player0";
                        turnPlayerSlot = 0;
                        SetTurnPlayer();
                        break;
                }

                ResetFunction();
                startTurnBTN.GetComponent<Image>().enabled = true;
            }
        }
        yield return null;
    }
    
    public void ResetFunction()
    {
        //Run cancle on end turn to close the control UI if it is still open.
        this.GetComponent<CreatureController>().CancleBTNFunction();

        //Reset the board tile dungeon connection state.
        GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().UpdateBoard();

        //Set the player turn UI to next turn player.
        UpdateTurnPlayerCrestDisplay();

        //Reset the states of all creature piece on the board (Move, Attack, ect)
        this.GetComponent<CreatureController>().ResetCreatureStates(); //---- Need to Add Creatures we spawn to creature controller creature list in order to reset their states,.-------------------------------------------------------------------------------------------------------------------------------------

        //Set the camera to the right board state.
        this.GetComponent<CameraController>().switchCamera("Alt");
    }

    #endregion

    #region EndGameCode
    public void GameOverFunction()
    {
        //Load Main Menu Scene & Apply Locks.
        currentGameEndScreen.SetActive(true);
        ableToInteractWithBoard = false;
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    #endregion
}
