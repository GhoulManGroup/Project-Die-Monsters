using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDiceController : MonoBehaviour //This class is replacing the old resource UI Manager script 
{
    [Header ("States & Checks")]
    //How many objects are set up to roll
    public int readyDice = 0;
    public bool hasRolledDice = false;

    [Header("Object Refrences")]
    GameObject lvlRef;
    public GameObject turnPlayer;
    Player player;

    [Header("Objects to Interact With")]
    public List<GameObject> UIElements = new List<GameObject>();
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> dieToRoll = new List<GameObject>();


    [Header("Objects to Spawn")]
    public GameObject diceFab;

    [Header("Summon Crest Tracking")]
    // These ints track how many of each summon crest we just rolled.We need 2 or more of one crest before we can declare those dice that match that crest to be playable on the board.
    public int lvl1Crest;
    public int lvl2Crest;
    public int lvl3Crest;
    public int lvl4Crest;

    public void Start()
    {
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
        UIElements[0].SetActive(false);
        UIElements[1].SetActive(false);
    }

    //Set up the games code
    public void SetUp()
    {
        Debug.Log("Setup Reached");
        player = turnPlayer.GetComponent<Player>();

        //Enable our UI BTNS.
        UIElements[0].SetActive(true);
        UIElements[1].SetActive(true);

        //Check how many dice to roll.
        for (int diceToSpawn = 0; diceToSpawn < player.diceHandSize; diceToSpawn++)
        {
            if (diceToSpawn <= player.diceDeck.Count)
            {
                //Spawn those dice into the arena.
                spawnDice();
            }
        }
        //Proceed to dice allocation step.
    }

    public void spawnDice()
    {
        GameObject DiceSpawned;

        //Spawn a dice at the spawn points whose position in the list is equal to the number of dice already in the space.
         DiceSpawned = Instantiate(diceFab, spawnPoints[dieToRoll.Count].transform.position, Quaternion.identity);
        dieToRoll.Add(DiceSpawned);
    }

    public void matchingSummonCrestCheck()
    {
        //check if each int is 2+ in value then tell any dice thats result string matches that intcrestname to change their can be played state to true.
        if (lvl1Crest >= 2)
        {
            for (int i = 0; i < dieToRoll.Count; i++)
            {
                if (dieToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC1")
                {
                    dieToRoll[i].GetComponent<SceneDieScript>().canBeChosen = true;
                }
            }
        }

        if (lvl2Crest >= 2)
        {
            for (int i = 0; i < dieToRoll.Count; i++)
            {
                if (dieToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC2")
                {
                    dieToRoll[i].GetComponent<SceneDieScript>().canBeChosen = true;
                }
            }
        }

        if (lvl3Crest >= 2)
        {
            for (int i = 0; i < dieToRoll.Count; i++)
            {
                if (dieToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC3")
                {
                    dieToRoll[i].GetComponent<SceneDieScript>().canBeChosen = true;
                }
            }
        }

        if (lvl4Crest >= 2)
        {
            for (int i = 0; i < dieToRoll.Count; i++)
            {
                if (dieToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC4")
                {
                    dieToRoll[i].GetComponent<SceneDieScript>().canBeChosen = true;
                }
            }
        }
    }

    public void resetFunction()
    {
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

        //Then hide the UI elements as the camera has changed view.
        UIElements[0].SetActive(false);
        UIElements[1].SetActive(false);
        hasRolledDice = false;

        //Reset dice results.
        lvl1Crest = 0;
        lvl2Crest = 0;
        lvl3Crest = 0;
        lvl4Crest = 0;
    }

    public void buttonPressed()
    {
        string BTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (BTNPressed)
        {
            case "RollDiceBTN":
                //If all 3 die are set up then tell all die to spin
                if (readyDice == 3 && hasRolledDice == false)
                {
                    Debug.Log("3 Dice Set Up");
                    for (int i = 0; i <dieToRoll.Count; i++)
                    {
                        dieToRoll[i].GetComponent<SceneDieScript>().spinDie();
                    }
                    hasRolledDice = true;
                }
                break;

            case "CloseBTN":
                //Close window and delete the die.
                resetFunction();              
                readyDice = 0;
                hasRolledDice = false;

                lvlRef.GetComponent<CameraController>().ActiveCam = "Board";
                lvlRef.GetComponent<CameraController>().switchCamera();

                //Player not performing action can press end turn BTN.
                lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false;
                break;
        }
    }

}
