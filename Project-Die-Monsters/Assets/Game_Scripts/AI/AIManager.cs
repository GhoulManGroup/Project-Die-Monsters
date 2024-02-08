using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{
    public static GameManagerScript instance { get; private set; }

    AIDungeonSpawner LvlDungeonSpawner = GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<AIDungeonSpawner>();
    LevelController LVLRef = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();

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
    public bool hasCreture = false;
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

        //Forgot to add back in the map dungeon call 

        //Check The Dungeon Can Expand 
        StartCoroutine(CheckDungeonCanExpand());
    }

    public IEnumerator CheckDungeonCanExpand()
    {
        LvlDungeonSpawner.tilesToCheck.Add(LvlDungeonSpawner.MyStartTile);

        LvlDungeonSpawner.AITileCheck("CheckSpawnLocations");

        while (PhaseDone == false)
        {
            yield return null;
        }

        PhaseDone = false;

        yield return LvlDungeonSpawner.StartCoroutine("CanAIDungeonExpand");

        while (PhaseDone == false)
        {
            yield return null;
        }

        StartCoroutine("DicePhase");
    }
    public IEnumerator DicePhase()
    {
        currentActionText.GetComponent<TextMeshProUGUI>().text = "AI Rolling Dice";

        this.GetComponent<AIRollManager>().SetUpAIDice();

        while (PhaseDone == false)
        {
            yield return null;
        }

        LVLRef.EndTurnFunction();

    }

    public IEnumerator tempSummonCreaturePhaseCall()
    {
        if (canPlaceCreature == true )
        {
            yield return LvlDungeonSpawner.StartCoroutine("PlaceDungeonAI");
            Debug.LogError("Spawning Creature can place creature");
        }
        else
        {
            Debug.LogError("Reseting Spawner cant place crearture");
            LvlDungeonSpawner.ResetSpawner();
        }

        canPlaceCreature = false;

        Debug.LogError("Path Check Done Start Dice");
    }


    IEnumerator CreatureActionPhase()
    {
        yield return null;
    }

    public void EndTurn()
    {

    }

    //Start AI Turn

    //Display AI Turn

    //Find if can expand dungeon

    //Display Dice Roll

    //Start AI Roll Manager
}
