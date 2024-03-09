 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;

public class LevelController : MonoBehaviour //This class oversees the setup turn management & game states of a simple 1v1 level
{
    #region Refrences
    [Header("Refrences & UI Objects")]
    //The projects game manager object which persists between scences.
    GameObject gameManager;
    public GameObject startTurnBTN;
    public GameObject endTurnBTN;
    public GameObject currentGameEndScreen;
    public List<GameObject> DungeonLordStartTiles = new List<GameObject>();

    [Header("Possible Participants")]
    // These two prefabs are the default player or AI opponent to be loaded into the level depending on who the opponent faces.
    public GameObject playerFab; 
    public GameObject opponent;
    //This list contains the opponent scriptable objects which contains the diffrent AI opponents the player can face, one will be added to the AI opponent object if present in the scene.
    public List<Opponent> opponentList = new List<Opponent>(); 

    [Header("Current Game Participants")]
    //If either prefab are instaciated they will be added to this list as "Player1" and "Player2".
    public List<GameObject> participants = new List<GameObject>();

    [Header("Turn Tracking")]
    public int turnCount = 1; // how many turns have passed.
    //which participant is the turn player.
    public GameObject turnPlayerObject;
    public int currentTurnParticipant = 0;

    [Header("Turn Player Constraints & Resources")] // Tweak constraints.
    public bool turnPlayerPerformingAction = false; //Is the player currently doing somthing.
    public bool ableToInteractWithBoard = false; // Player is allowed to interact with the pieces on the board, checked before raycast on piece ect.
    public string boardInteraction = "None"; //What action the player is taking
    
    [Header("Creature")]
    public bool placingCreature = false; // The player is placing a creature on the board
    public string creaturePlacedFrom = "None"; // DiceZone, Creature Pool, Other?

    public List<Text> turnPlayerUIDisplay = new List<Text>();
    #endregion

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
                        DungeonLordStartTiles[0].GetComponent<DungeonLordPiece>().myOwner = "0";
                        DungeonLordStartTiles[1].GetComponent<DungeonLordPiece>().myOwner = "1";

                        DungeonLordStartTiles[0].GetComponent<DungeonLordPiece>().SetDungeonLordTile();
                        DungeonLordStartTiles[1].GetComponent<DungeonLordPiece>().SetDungeonLordTile();            
                        break;

                    case "AI":
                        Debug.Log("Hello From AI Development");

                        GameObject player = Instantiate(playerFab);
                        participants.Add(player);

                        GameObject enemy = Instantiate(opponent);
                        enemy.GetComponent<AIOpponent>().myOpponent = opponentList[0];
                        participants.Add(enemy);
                        GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>().currentOpponent = enemy;

                        DungeonLordStartTiles[0].GetComponent<DungeonLordPiece>().myOwner = "0";
                        DungeonLordStartTiles[1].GetComponent<DungeonLordPiece>().myOwner = "1";

                        DungeonLordStartTiles[0].GetComponent<DungeonLordPiece>().SetDungeonLordTile();
                        DungeonLordStartTiles[1].GetComponent<DungeonLordPiece>().SetDungeonLordTile();

                        //Once we have declared dungeon lord tile positions map out each tiles distance from the player position
                        GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<AIDungeonSpawner>().DungeonSizeSetup();
                        break;
                }
                break;

            case "Multiplayer":
                participants.AddRange(GameObject.FindGameObjectsWithTag("Player"));
                break;
        }

        //Set the player decks.
        setDeck();

        //Setup turn player.
        SetTurnPlayer();

    }

    private void setDeck()
    {
        // Copies the the players deck in the deck list slot [0] to the spawned player prefabs will need to be changed once we get more gameplay modes set up 
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
        this.GetComponent<PlayerCreatureController>().turnPlayer = participants[currentTurnParticipant].gameObject;
        //this.GetComponent<CreaturePoolController>().turnPlayer = participants[currentTurnParticipant].gameObject;
        this.GetComponent<UIDiceController>().turnPlayer = participants[currentTurnParticipant].gameObject;
        GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().turnPlayer = participants[currentTurnParticipant].gameObject;
        turnPlayerObject = participants[currentTurnParticipant].gameObject;
    }

    public void BeginTurnFunction() // this function is only called when it is a Human Players turn not the ai,
     {
        // check if the player has any dice in the list 
        if (participants[currentTurnParticipant].GetComponent<Player>().diceDeck.Count != 0)
        {
            this.GetComponent<LevelController>().turnPlayerPerformingAction = true; // Player is in dice window, action being performed.

            GetComponent<CameraController>().switchCamera("Dice");
            GetComponent<UIDiceController>().SetUp();       
        }
                
         else if (participants[currentTurnParticipant].GetComponent<Player>().diceDeck.Count == 0) // if no dice in pool proceed to board phase.
         {
            Debug.Log("Turn Player Has No Dice");
             this.GetComponent<CameraController>().switchCamera("Alt");
             ableToInteractWithBoard = true;
         }

        startTurnBTN.SetActive(false);
        endTurnBTN.SetActive(true);
        //participants[currentTurnParticipant].GetComponent<Player>().moveCrestPoints += 1; pity crest gain removed as movement changed
        UpdateTurnPlayerCrestDisplay();
     }

    public void UpdateTurnPlayerCrestDisplay()
    {
        turnPlayerUIDisplay[0].text = "Turn Player = " + currentTurnParticipant.ToString();
        if (turnPlayerObject.GetComponent<Player>() != null)
        {
            turnPlayerUIDisplay[1].text = participants[currentTurnParticipant].GetComponent<Player>().attackCrestPoints.ToString();
            turnPlayerUIDisplay[2].text = participants[currentTurnParticipant].GetComponent<Player>().abiltyPowerCrestPoints.ToString();
            turnPlayerUIDisplay[3].text = participants[currentTurnParticipant].GetComponent<Player>().defenceCrestPoints.ToString();
            //turnPlayerUIDisplay[4].text = participants[currentTurnParticipant].GetComponent<Player>().moveCrestPoints.ToString(); no longer active UI element so disable
            turnPlayerUIDisplay[5].text = participants[currentTurnParticipant].GetComponent<Player>().summmonCrestPoints.ToString();

        }else if (turnPlayerObject.GetComponent<AIOpponent>() != null)
        {
            turnPlayerUIDisplay[1].text = participants[currentTurnParticipant].GetComponent<AIOpponent>().attackCrestPoints.ToString();
            turnPlayerUIDisplay[2].text = participants[currentTurnParticipant].GetComponent<AIOpponent>().abiltyPowerCrestPoints.ToString();
            turnPlayerUIDisplay[3].text = participants[currentTurnParticipant].GetComponent<AIOpponent>().defenceCrestPoints.ToString();
            //turnPlayerUIDisplay[4].text = participants[currentTurnParticipant].GetComponent<AIOpponent>().moveCrestPoints.ToString(); no longer active UI element so disable
            turnPlayerUIDisplay[5].text = participants[currentTurnParticipant].GetComponent<AIOpponent>().summmonCrestPoints.ToString();
        }
    }

    public void CheckForTriggersToResolve()
    {
        AbilityUIController AbilityWindow = GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>();
        if (AbilityWindow.creaturesToTrigger.Count != 0 && turnPlayerPerformingAction == false)
        {
            ableToInteractWithBoard = false;
            //Debug.Log("Going To Trigger Abilties Now");
            AbilityWindow.StartCoroutine("StackManager");
        }
        else
        {
            //Debug.Log("No Triggers To Resolve " + turnPlayerPerformingAction + " " + AbilityWindow.creaturesToTrigger.Count);
            GameObject.FindGameObjectWithTag("LevelController").GetComponent<PlayerCreatureController>().CheckCreatureStates();
        }
      
    }

    public void CanEndTurn()
    {
        if (turnPlayerPerformingAction == false)
        {
            endTurnBTN.GetComponent<Button>().interactable = true;
        }else if (turnPlayerPerformingAction == true)
        {
            endTurnBTN.GetComponent<Button>().interactable = false;
        }
    }

    public void EndTurnFunction()
     {
        //Ensure that player isnt currently doing somthing.
        StartCoroutine("EndTurn");
     }

    private IEnumerator EndTurn()
    {

        //If human player is current turn user then ensure all current qued actions are resolved, then perform an end of turn abiltiy trigger check and wait untill all are resolved before proceeding.
        if (turnPlayerObject.GetComponent<Player>() != null)
        {
            if (turnPlayerPerformingAction == true)
            {
                Debug.Log("Cant End Turn Yet Player");
                yield break;
            }
            if (turnPlayerPerformingAction == false)
            {
                for (int i = 0; i < this.GetComponent<PlayerCreatureController>().CreaturesOnBoard.Count; i++)
                {
                    if (this.GetComponent<PlayerCreatureController>().CreaturesOnBoard[i].GetComponent<AbilityManager>().myAbility.abilityActivatedHow == Ability.AbilityActivatedHow.Trigger)
                    {
                        this.GetComponent<PlayerCreatureController>().CreaturesOnBoard[i].GetComponent<AbilityManager>().CheckTrigger("OnEndTurn");
                    }
                }

                CheckForTriggersToResolve();

                while (GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().creaturesToTrigger.Count > 0)
                {
                    yield return null;
                }

            }
        }else if (turnPlayerObject.GetComponent<AIOpponent>() != null)
        {
            Debug.Log("Inside AI End Turn Condtion " + currentTurnParticipant);

        }

        StartCoroutine("ResetFunction");

        yield return new WaitForSeconds(1f);

        SwitchTurnPlayer();
    }

    public void SwitchTurnPlayer()
    {
        switch (currentTurnParticipant)
        {
            case 0:

                currentTurnParticipant = 1;

                SetTurnPlayer();

                if (turnPlayerObject.GetComponent<Player>() != null)
                {
                    startTurnBTN.SetActive(true);
                }
                else if (turnPlayerObject.GetComponent<AIOpponent>() != null)
                {
                    GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>().BeginTurn();
                }
                break;

            case 1: //0 participant is always a player so don't check for component
                currentTurnParticipant = 0;
                SetTurnPlayer();
                startTurnBTN.SetActive(true);
                break;

        }
    }
    
    public IEnumerator ResetFunction()
    {
        if (gameManager.GetComponent<GameManagerScript>().desiredOpponent == "Player")
        {
            Debug.Log("Test");
        }
        endTurnBTN.SetActive(false);

        //this.GetComponent<CreaturePoolController>().enableButtons();

        //Run cancle on end turn to close the control UI if it is still open.
        this.GetComponent<PlayerCreatureController>().CancleBTNFunction();

        //Reset the board tile dungeon connection state.
        GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().UpdateBoard();

        //Set the player turn UI to next turn player.
        UpdateTurnPlayerCrestDisplay();

        //Reset the states of all creature piece on the board (Move, Attack, ect)
        this.GetComponent<PlayerCreatureController>().ResetCreatureStates();
        GameObject.FindGameObjectWithTag("AIController").GetComponent<AICreatureController>().ResetCreatures();

        //Set the camera to the right board state.
        yield return new WaitForSeconds(1f);

        if (gameManager.GetComponent<GameManagerScript>().desiredOpponent == "AI")
        {//When the AI turn begins switch camera to board scene untill Ai decides to move to dice.
            if (turnPlayerObject.GetComponent<AIOpponent>() != null)
            {
                this.GetComponent<CameraController>().switchCamera("Alt");
            }
        }
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
