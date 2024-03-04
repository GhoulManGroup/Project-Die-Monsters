using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{ 
   
    AIDungeonSpawner LvlDungeonSpawner;
    LevelController LVLRef;

    //static AIManager Instance pro
    public GameObject currentOpponent;
    [SerializeField]
    GameObject currentAction;
    [SerializeField]
    GameObject currentActionText;

    //[HideInInspector]
    public bool PhaseDone = false;

    [Header("PossibleActions")]
    public bool canPlaceCreature = false; //if dungeon can be expanded set to true
    public bool hasCreatureToPlace = false; // did dice roll result in summonable creature
    [HideInInspector] public bool hasCreature = false; // If false skip creature action phase 

    public void Start()
    {
        LvlDungeonSpawner = GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<AIDungeonSpawner>();
        LVLRef = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
    }

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
        LvlDungeonSpawner.tilesToCheck.Add(LvlDungeonSpawner.MyStartTile.gameObject);

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

        this.GetComponent<AIRollManager>().StartCoroutine("SetUpAIDice");

        while (PhaseDone == false)
        {
            yield return null;
        }

        LVLRef.UpdateTurnPlayerCrestDisplay();
        StartCoroutine(TempSummonCreaturePhaseCall());
    }

    public IEnumerator TempSummonCreaturePhaseCall()
    {
        if (canPlaceCreature == true && hasCreatureToPlace == true)
        {
            PhaseDone = false;

            yield return LvlDungeonSpawner.StartCoroutine("PlaceDungeonAI");

            while (PhaseDone == false)
            {
                yield return null;
            }
        }
        else
        {
            Debug.LogError("Either Dungeon Cant Expand " + canPlaceCreature +" Or No Creature to Spawn " + hasCreatureToPlace);
            LvlDungeonSpawner.ResetSpawner();
        }

        canPlaceCreature = false;
        hasCreatureToPlace = false;

        yield return new WaitForSeconds(3f);

        StartCoroutine(CreatureActionPhase());
    }


    IEnumerator CreatureActionPhase()
    {
        PhaseDone = false;

        if (this.GetComponent<AICreatureController>().myCreatures.Count != 0)
        {
            this.GetComponent<AICreatureController>().StartCoroutine("ActionPhase");

            while(PhaseDone == false)
            {
                yield return null;
            }
        }
        else
        {
            Debug.Log("No Creature To Act");
        }

        yield return new WaitForSeconds(3f);

        EndTurn();
    }

    public void EndTurn()
    {
        LVLRef.EndTurnFunction();
    }

    //Start AI Turn

    //Display AI Turn

    //Find if can expand dungeon

    //Display Dice Roll

    //Start AI Roll Manager

    // check for creature

    // expand dungeon

    // give creature orders

    // end turn
}
