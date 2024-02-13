using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class AIRollManager : MonoBehaviour
{
    GameObject LVLRef;
    AIOpponent currentOpponent;
    AIManager mRef;

    [Header("3D Dice Rolling")]
    [SerializeField] GameObject diceFab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> DiceToRoll = new List<GameObject>();
    [HideInInspector] public Creature creaturePicked;
    
    [Header("Dice Value Weight System")]
    Die diceToAdd;

    [Header("DiceResults Tracking")]
    [HideInInspector] public int diceRolled = 0;
    [HideInInspector] public int crest1Count = 0;
    [HideInInspector] public int crest2Count = 0;
    [HideInInspector] public int crest3Count = 0;
    [HideInInspector] public int crest4Count = 0;
    [HideInInspector] public int summonCrestPool = 0;
    [HideInInspector] public int attackCrestPool = 0;
    [HideInInspector] public int defenceCrestPool = 0;
    [HideInInspector] public int abilityCrestPool = 0;

    public void Start()
    {
        mRef = this.GetComponent<AIManager>();
        LVLRef = GameObject.FindGameObjectWithTag("LevelController");
    }

    public IEnumerator SetUpAIDice()
    {

        LVLRef.GetComponent<CameraController>().switchCamera("Dice");
        currentOpponent = LVLRef.GetComponent<LevelController>().participants[LVLRef.GetComponent<LevelController>().currentTurnParticipant].GetComponent<AIOpponent>();

        mRef.PhaseDone = false;

        //Spawn a dice at the spawn points whose position in the list is equal to the number of dice already in the space.
        GameObject DiceSpawned;

        for (int i = 0; i < 3; i++)
        {
            DiceSpawned = Instantiate(diceFab, spawnPoints[DiceToRoll.Count].transform.position, Quaternion.identity);
            DiceToRoll.Add(DiceSpawned);
            WhatDiceToAdd();
            DiceSpawned.GetComponent<SceneDieScript>().myDie = diceToAdd;
            yield return DiceSpawned.GetComponent<SceneDieScript>().StartCoroutine("setUpDie");
        }

        yield return new WaitForSeconds(4f);
        
        StartCoroutine("RollDice");
    }

    public void WhatDiceToAdd()
    {
        diceToAdd = mRef.currentOpponent.GetComponent<AIOpponent>().AIDiceDeck[0];
        mRef.currentOpponent.GetComponent<AIOpponent>().AIDiceDeck.RemoveAt(0);
    }

    private IEnumerator RollDice()
    {

        for (int i = 0; i < DiceToRoll.Count; i++)
        {
            DiceToRoll[i].GetComponent<SceneDieScript>().spinDie();
        }

        while (diceRolled != 3)
        {
            yield return null;
        }

        Debug.Log("Dice Rolled And Results Checked");

        StartCoroutine(ResolveCrests());
    }

    private IEnumerator ResolveCrests()
    {

        //If AI can expand its dungeon check for summon crests //Check for Duplicates to summon for free //Check Can Summon via spending summonCrests //Store unused Crests

        if (mRef.canPlaceCreature == true)
        {
            for (int i = 0; i < DiceToRoll.Count; i++)
            {
                if (crest4Count >= 2 && DiceToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC4")
                {
                    creaturePicked = DiceToRoll[i].GetComponent<SceneDieScript>().myDie.dieCreature;
                    mRef.hasCreatureToPlace = true;
                    summonCrestPool = 0;
                    break;
                }
                else if (crest3Count >= 2 && DiceToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC3")
                {
                    creaturePicked = DiceToRoll[i].GetComponent<SceneDieScript>().myDie.dieCreature;
                    mRef.hasCreatureToPlace = true;
                    summonCrestPool = 0;
                    break;
                }
                else if (crest2Count >= 2 && DiceToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC2")
                {
                    creaturePicked = DiceToRoll[i].GetComponent<SceneDieScript>().myDie.dieCreature;
                    mRef.hasCreatureToPlace = true;
                    summonCrestPool = 0;
                    break;
                }
                else if (crest1Count >= 2 && DiceToRoll[i].GetComponent<SceneDieScript>().rollResult == "LC1")
                {
                    creaturePicked = DiceToRoll[i].GetComponent<SceneDieScript>().myDie.dieCreature;
                    mRef.hasCreatureToPlace = true;
                    summonCrestPool = 0;
                    break;
                }
                else if (DiceToRoll[i].GetComponent<SceneDieScript>().myDie.dieCreature.summonCost <= currentOpponent.summmonCrestPoints)
                {
                    creaturePicked = DiceToRoll[i].GetComponent<SceneDieScript>().myDie.dieCreature;
                    currentOpponent.summmonCrestPoints -= DiceToRoll[i].GetComponent<SceneDieScript>().myDie.dieCreature.summonCost;
                    mRef.hasCreatureToPlace = true;
                    summonCrestPool = 0;
                    break;
                }
            }
        }

        //Remove chosen dice from dice deck
        //Add unused dice back to dice deck for AI
        UpdateAICrests();
        ClearDice();

        yield return null;
        LVLRef.GetComponent<CameraController>().switchCamera("Board");
        mRef.PhaseDone = true;
    }

    void UpdateAICrests()
    {

        Debug.Log(summonCrestPool + "AIGains This many Summon Crests");
        currentOpponent.summmonCrestPoints += summonCrestPool;
        currentOpponent.attackCrestPoints += attackCrestPool;
        currentOpponent.abiltyPowerCrestPoints += abilityCrestPool;
        currentOpponent.defenceCrestPoints += defenceCrestPool;

        crest1Count = 0;
        crest2Count = 0;
        crest3Count = 0;
        crest4Count = 0;
        summonCrestPool = 0;
        attackCrestPool = 0;
        defenceCrestPool = 0;
        abilityCrestPool = 0;
    }

    void ClearDice()
    {


        foreach (var item in DiceToRoll)
        {
            if (item.GetComponent<SceneDieScript>().myDie.dieCreature != creaturePicked)
            {
                mRef.currentOpponent.GetComponent<AIOpponent>().AIDiceDeck.Add(item.GetComponent<SceneDieScript>().myDie);
            }
            Destroy(item.gameObject);
        }

        DiceToRoll.Clear();
        diceRolled = 0;

        Debug.Log(mRef.currentOpponent.GetComponent<AIOpponent>().AIDiceDeck.Count);
    }
}
