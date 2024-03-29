using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDiceController : MonoBehaviour // This class controls the In game dice selection and roll system and its UI elements involved in that process.
{
    [Header ("States & Checks")]
    //How many objects are set up to roll
    public int readyDice = 0;
    public bool hasRolledDice = false;

    public int dicechecked; // how many dice have stopped rolling and resolved thier results.
    bool canCloseUI = false; // stop the player from closing the UI early.

    [Header("Object Refrences")]
    GameObject lvlRef;
    GameObject inspectWindow;
    public GameObject turnPlayer;
    Player player;

    [Header("Objects to Interact With")]
    public List<GameObject> UIElements = new List<GameObject>();
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> dieToRoll = new List<GameObject>();
    public List<GameObject> UIDice = new List<GameObject>(); // The UI objects representing the dice choices


    [Header("Objects to Spawn")]
    public GameObject diceFab;

    [Header("Summon Crest Tracking")]
    // These ints track how many of each summon crest we just rolled.We need 2 or more of one crest before we can declare those dice that match that crest to be playable on the board.
    public int lvl1Crest;
    public int lvl2Crest;
    public int lvl3Crest;
    public int lvl4Crest;
    public int summonCrestPool = 0;
    bool hasSummoned = false;

    public void Start()
    {
        UIElements[0].SetActive(false);
        UIElements[1].SetActive(false);
        UIElements[2].SetActive(false);
        UIElements[3].SetActive(false);
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
        inspectWindow = GameObject.FindGameObjectWithTag("InspectWindow");
    }

    public void SetUp()
    {
        player = turnPlayer.GetComponent<Player>();
        //Check how many dice to roll.
        for (int diceToSpawn = 0; diceToSpawn < player.diceHandSize; diceToSpawn++)
        {
            if (diceToSpawn <= player.diceDeck.Count)
            {
                SpawnDice();
            }
        }
        UIElements[0].SetActive(true);
        UIElements[4].GetComponent<TextMeshProUGUI>().text = "Click Each Dice To Assign Which Die To Roll";
    }

    public void SpawnDice()
    {
        GameObject DiceSpawned;
        //Spawn a dice at the spawn points whose position in the list is equal to the number of dice already in the space.
         DiceSpawned = Instantiate(diceFab, spawnPoints[dieToRoll.Count].transform.position, Quaternion.identity);
        dieToRoll.Add(DiceSpawned);
    }

    public void DiceSelectWindow()
    {
        UIElements[3].SetActive(true);
        // For each dice in the player dice pool activate and assign a dice object in the UI 1 dice then have it display a sprite to represent its contents.
        for (int i = 0; i < player.GetComponent<Player>().dicePool; i++)
        {
            if (i < player.GetComponent<Player>().diceDeck.Count)
            {
                UIDice[i].gameObject.SetActive(true);
                UIDice[i].GetComponent<DicePoolInspectScript>().diceNumber = i;
                UIDice[i].GetComponent<DicePoolInspectScript>().myDie = player.GetComponent<Player>().diceDeck[i];
                UIDice[i].GetComponent<DicePoolInspectScript>().setUp();
            }
            else
            {
                UIDice[i].gameObject.SetActive(false);
            }
        }
    }

    public void CheckAllDiceSetUp()
    {
        int diceReady = 0;
        for (int i = 0; i < dieToRoll.Count; i++)
        {
            if (dieToRoll[i].GetComponent<SceneDieScript>().diceSetUp == true)
            {
                diceReady += 1;
            }
        }

        if (diceReady == dieToRoll.Count)
        { 
            UIElements[1].SetActive(true);
            UIElements[4].GetComponent<TextMeshProUGUI>().text = "Roll Your Dice!";
        }
    }

    public void CheckCanSummonCreature()
    {
        if (player.diceDeck.Count > 3)
        {
            UIElements[1].SetActive(false);
            UIElements[2].SetActive(true);
            UIElements[4].GetComponent<TextMeshProUGUI>().text = "Summon a creature if able? or return to the board";
            //check if each int is 2+ in value then tell any dice thats result string matches that intcrestname to change their can be played state to true.
            if (lvl1Crest >= 2)
            {
                for (int i = 0; i < dieToRoll.Count; i++)
                {
                    if (dieToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC1" && dieToRoll[i].GetComponent<SceneDieScript>().isFree == false)
                    {
                        dieToRoll[i].GetComponent<SceneDieScript>().isFree = true;
                    }
                }
            }

            if (lvl2Crest >= 2)
            {
                for (int i = 0; i < dieToRoll.Count; i++)
                {
                    if (dieToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC2" && dieToRoll[i].GetComponent<SceneDieScript>().isFree == false)
                    {
                        dieToRoll[i].GetComponent<SceneDieScript>().isFree = true;
                    }
                }
            }

            if (lvl3Crest >= 2)
            {
                for (int i = 0; i < dieToRoll.Count; i++)
                {
                    if (dieToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC3" && dieToRoll[i].GetComponent<SceneDieScript>().isFree == false)
                    {
                        dieToRoll[i].GetComponent<SceneDieScript>().isFree = true;
                    }
                }
            }

            if (lvl4Crest >= 2)
            {
                for (int i = 0; i < dieToRoll.Count; i++)
                {
                    if (dieToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC4" && dieToRoll[i].GetComponent<SceneDieScript>().isFree == false)
                    {
                        dieToRoll[i].GetComponent<SceneDieScript>().isFree = true;
                    }
                }
            }

            if (dicechecked == 3)
            {

                for (int i = 0; i < dieToRoll.Count; i++)
                {// Add if rolled levle crest. Need to rewrite the script a bit more.
                    if (dieToRoll[i].GetComponent<SceneDieScript>().myDie.dieCreature.summonCost <= turnPlayer.GetComponent<Player>().summmonCrestPoints && dieToRoll[i].GetComponent<SceneDieScript>().isFree == false && dieToRoll[i].GetComponent<SceneDieScript>().rolledLevelCrest == true)
                    {
                        dieToRoll[i].GetComponent<SceneDieScript>().canBeChosen = true;
                    }
                }
                canCloseUI = true;
            }
        }else if (player.diceDeck.Count< 3)
        {
            Debug.Log("Insufficent Dice In Pool To Summon");
            canCloseUI = true;
        }
    }

    public void ButtonPressed()
    {
        string BTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (BTNPressed)
        {
            case "RollDiceBTN":
                //Check if dice are set up;
                for (int i = 0; i < dieToRoll.Count; i++)
                {
                    if (dieToRoll[i].GetComponent<SceneDieScript>().diceSetUp == true && hasRolledDice == false)
                    {
                        readyDice += 1;
                    }
                }

                if (readyDice == 3 && hasRolledDice == false)
                {
                    inspectWindow.GetComponent<InspectWindowController>().CloseInspectWindow();
                    for (int i = 0; i <dieToRoll.Count; i++)
                    {
                        dieToRoll[i].GetComponent<SceneDieScript>().spinDie();
                    }
                    hasRolledDice = true;
                    UIElements[1].SetActive(false);

                }
                else if (readyDice != 3 && hasRolledDice == false)
                {
                    readyDice = 0;
                }
                break;

            case "CloseBTN":
                if (canCloseUI == true)
                {
                    //Player not performing action can press end turn BTN.
                    lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false;
                    lvlRef.GetComponent<LevelController>().ableToInteractWithBoard = true;
                    turnPlayer.GetComponent<Player>().summmonCrestPoints += summonCrestPool;
                    summonCrestPool = 0;
                    inspectWindow.GetComponent<InspectWindowController>().CloseInspectWindow();
                    //Close window and delete the die.
                    ResetFunction();
                    lvlRef.GetComponent<CameraController>().switchCamera("Alt");
                }
                break;

                case "SummonBTN":
                {
                    lvlRef.GetComponent<LevelController>().placingCreature = true;
                    inspectWindow.GetComponent<InspectWindowController>().sceneDice.GetComponent<SceneDieScript>().myDie = null;
                    inspectWindow.GetComponent<InspectWindowController>().CloseInspectWindow();
                    GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().HideandShow();
                    lvlRef.GetComponent<CameraController>().switchCamera("Board");
                    lvlRef.GetComponent<LevelController>().creaturePlacedFrom = "DiceBoard";
                    hasSummoned = true;
                    ResetFunction();
                }
                break;
                /*
                case "PoolBTN":
                    inspectWindow.GetComponent<InspectWindowController>().AddCreatureToPool();
                    lvlRef.GetComponent<LevelController>().ableToInteractWithBoard = true;
                    hasSummoned = true;
                    ResetFunction();
                break;
                */

        }
    }

    public void ResetFunction()
    {
        lvlRef.GetComponent<LevelController>().UpdateTurnPlayerCrestDisplay();
        lvlRef.GetComponent<LevelController>().CanEndTurn();
        //Add the dice objects on each dice if not null back to the player list.(The Unchosen ones)
        for (int i = 0; i < readyDice; i++)
        {
            if (dieToRoll[i].GetComponent<SceneDieScript>().myDie != null)
            {
                turnPlayer.GetComponent<Player>().diceDeck.Add(dieToRoll[i].GetComponent<SceneDieScript>().myDie);
            }
            GameObject.Destroy(dieToRoll[i]);
        }

        //Empty list of missing objects.
        dieToRoll.Clear();

        //Reset States
        dicechecked = 0;
        canCloseUI = false;
        hasRolledDice = false;
        readyDice = 0;

        //Then hide the UI elements as the camera has changed view.
        UIElements[0].SetActive(false);
        UIElements[2].SetActive(false);
        UIElements[3].SetActive(false);
        UIElements[5].GetComponent<Button>().interactable = false;
        UIElements[6].GetComponent<Button>().interactable = false;


        //Reset dice results.
        lvl1Crest = 0;
        lvl2Crest = 0;
        lvl3Crest = 0;
        lvl4Crest = 0;
    }

}
