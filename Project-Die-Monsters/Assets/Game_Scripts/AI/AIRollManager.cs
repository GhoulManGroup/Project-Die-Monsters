using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class AIRollManager : MonoBehaviour
{
    GameObject LVLRef;
    AIManager mRef;

    [Header("3D Dice Rolling")]
    [SerializeField] GameObject diceFab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> DiceToRoll = new List<GameObject>();
    


    [Header("Dice Value Weight System")]
    Die diceToAdd;

    [Header("DiceResults Tracking")]
    [HideInInspector] public int diceRolled = 0;
    [HideInInspector] public int crest1Count = 0;
    [HideInInspector] public int crest2Count = 0;
    [HideInInspector] public int crest3Count = 0;
    [HideInInspector] public int Crest4Count = 0;
    [HideInInspector] public int summonCrestPool = 0;
    [HideInInspector] public int attackCrestPool = 0;
    [HideInInspector] public int defenceCrestPool = 0;
    [HideInInspector] public int abilityCrestPool = 0;


    public IEnumerator SetUpAIDice()
    {
        mRef = this.GetComponent<AIManager>();
        LVLRef = GameObject.FindGameObjectWithTag("LevelController");
        LVLRef.GetComponent<CameraController>().switchCamera("Dice");

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
        diceToAdd = mRef.currentOpponent.GetComponent<AIOpponent>().myOpponent.OpponentDeck[0];
        mRef.currentOpponent.GetComponent<AIOpponent>().myOpponent.OpponentDeck.RemoveAt(0);

        /*
        int lvl1CreatureCount = 0;
        int lvl2CreatureCount = 0;
        int lvl3CreatureCount = 0;
        int lvl4CreatureCount = 0;

        for (int i = 0; i < mRef.currentOpponent.GetComponent<AIOpponent>().myOpponent.OpponentDeck.Count; i++)
        {
            if (mRef.currentOpponent.GetComponent<AIOpponent>().myOpponent.OpponentDeck[i].dieCreatureLevel == Die.DieCreatureLevel.one)
            {

            }else if (mRef.currentOpponent.GetComponent<AIOpponent>().myOpponent.OpponentDeck[i].dieCreatureLevel == Die.DieCreatureLevel.two)
            {

            }else if (mRef.currentOpponent.GetComponent<AIOpponent>().myOpponent.OpponentDeck[i].dieCreatureLevel == Die.DieCreatureLevel.three)
            {

            }else if (mRef.currentOpponent.GetComponent<AIOpponent>().myOpponent.OpponentDeck[i].dieCreatureLevel == Die.DieCreatureLevel.four)
            {

            }
        }
        */
    }

    private IEnumerator RollDice()
    {

        Debug.LogError("Inisde THe Menbrane");

        for (int i = 0; i < DiceToRoll.Count; i++)
        {
            DiceToRoll[i].GetComponent<SceneDieScript>().spinDie();
        }

        while (diceRolled != 3)
        {
            yield return null;
        }

        Debug.Log("Dice Rolled And Results Checked");

    }

    private IEnumerator countCrests()
    {
        yield return null;
    }
}
