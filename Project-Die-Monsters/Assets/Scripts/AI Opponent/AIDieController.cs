using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIDieController : MonoBehaviour
{
    public List <Die> AIDicePool = new List<Die>();

    public List<GameObject> AIDisplay = new List<GameObject>();

    IEnumerator WaitBetweenActionDice()
    {
        yield return new WaitForSeconds(15);
        Debug.Log("Wait");
    }

    public void DrawDie()
    {
        this.GetComponent<AIController>().AIAction.text = "Drawing Dice";
        WaitBetweenActionDice();

        // Draw dice from deck.
        for (int i = 0; i < this.GetComponent<AIController>().handSize; i++)
        {
            int drawMe = Random.RandomRange(0, this.GetComponent<AIController>().AiDieDeck.Count);
            AIDicePool.Add(this.GetComponent<AIController>().AiDieDeck[drawMe]);
            this.GetComponent<AIController>().AiDieDeck.RemoveAt(drawMe);
        }

        DisplayDice();
    }

    public void DisplayDice()
    {            
        for (int i = 0; i < AIDisplay.Count; i++)
        {
            Debug.Log("DD");
            AIDisplay[i].GetComponent<AIDieScript>().myDie = AIDicePool[i];
            AIDisplay[i].GetComponent<AIDieScript>().showUI();
            AIDisplay[i].GetComponent<AIDieScript>().SetImage();
            WaitBetweenActionDice();
            WhatDoIWant();
        }
    }


    public void WhatDoIWant()
    {
        Debug.Log("wdiw");
    }

    public void MulliganDie()
    {
        Debug.Log("MD");
        this.GetComponent<AIController>().AIAction.text = "Mulliganing";


        DrawDie();
    }

    public void RollDice()
    {
        Debug.Log("RD");
    }

    public void SelectDice()
    {
        Debug.Log("SD");
    }


}
