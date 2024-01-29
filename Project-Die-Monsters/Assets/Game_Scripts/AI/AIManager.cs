using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{
    public static GameManagerScript instance { get; private set; }

    //static AIManager Instance pro
    public GameObject currentOpponent;
    [SerializeField]
    GameObject currentAction;
    [SerializeField]
    GameObject currentActionText;

    [HideInInspector]
    public bool PhaseDone = false;

    [Header("Resources")]
    public List<GameObject> myCreatures = new List<GameObject>();

    [Header("PossibleActions")]
    public bool canPlaceCreature = false; //if dungeon can be expanded set to true
    public int canAttack = 0;
    public int abiltiesCanBeCast;

    public void BeginTurn()
    {
        //Set up AI Dice Deck
        currentOpponent.GetComponent<AIOpponent>().SetUp();
        
        //Enable Phase UI display
        currentAction.SetActive(true);

        //Set Text
        currentActionText.GetComponent<TextMeshProUGUI>().text = "AI Turn Start";

        //Add other prep when developed 

        //Start Dice Phase
        StartCoroutine("DicePhase");
        
    }

    public IEnumerator DicePhase()
    {
        AIDungeonSpawner LvlDungeonSpawner = GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<AIDungeonSpawner>();
        LevelController LVLRef = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();

        LvlDungeonSpawner.tilesToCheck.Add(LvlDungeonSpawner.MyStartTile);

        yield return LvlDungeonSpawner.StartCoroutine("AITileCheck", "CheckSpawnLocations");

        //Do Check
        LvlDungeonSpawner.StartCoroutine("CanAIDungeonExpand");

        while (PhaseDone == false)
        {
            yield return null;
        }

        //Do Thing here

        LvlDungeonSpawner.StartCoroutine("PlaceDungeonAI");

        Debug.LogError("Path Check Done Start Dice");

        LVLRef.EndTurnFunction();

        //this.GetComponent<AIRollManager>().SetUpAIDice();

    }

    //Start AI Turn

    //Display AI Turn

    //Find if can expand dungeon

    //Display Dice Roll

    //Start AI Roll Manager
}
