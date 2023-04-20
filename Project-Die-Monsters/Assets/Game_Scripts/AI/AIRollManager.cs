using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class AIRollManager : MonoBehaviour
{
    [Header("3D Dice Rolling")]
    [SerializeField] GameObject diceFab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> DiceToRoll = new List<GameObject>();
    
    [SerializeField] GameObject LVLRef;
    [SerializeField] AIManager mRef;

    [Header("Dice Value Weight System")]
    int creature;
    int crest;
    Die diceToAdd;

    public void SetUpAIDice()
    {
        mRef = this.GetComponent<AIManager>();
        LVLRef = GameObject.FindGameObjectWithTag("LevelController");
        LVLRef.GetComponent<CameraController>().switchCamera("Dice");


        //Spawn a dice at the spawn points whose position in the list is equal to the number of dice already in the space.
        GameObject DiceSpawned;
        for (int i = 0; i < 3; i++)
        {
            DiceSpawned = Instantiate(diceFab, spawnPoints[DiceToRoll.Count].transform.position, Quaternion.identity);
            DiceToRoll.Add(DiceSpawned);
        }
        
        StartCoroutine("AssignCreatureToDice");
    }

    private IEnumerator AssignCreatureToDice()
    {
        for (int i = 0; i < DiceToRoll.Count; i++)
        {
            WhatDiceToAdd();
            DiceToRoll[i].GetComponent<SceneDieScript>().myDie = diceToAdd;
        }
        // Add dice that best meet that want to the pool
        yield return null;
    }

    public void WhatDiceToAdd()
    {
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
    }

    private IEnumerator RollDice()
    {
        yield return null;
    }

    private IEnumerator countCrests()
    {
        yield return null;
    }
}
