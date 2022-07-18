using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    [Header("Opponent")]
    public Opponent levelOpponent;
    int life;
    public int handSize;
    public int boardCreatureCount = 0;

    [Header("AIResources")]
    int AImoveCrest = 1;
    int AIdefenceCrest = 1;
    int AIattackCrest = 1;
    int AIAbilityCrest = 1;

    [Header("AIDiceDeck")]
    public List<Die> AiDieDeck = new List<Die>();

    [Header("AIStateMachine")]
    public string AITurnState = "Idle";
    string AIActionTXT;
    public Text AIAction;

    // Start is called before the first frame update
    void Start()
    {
        setDetails();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setDetails()
    {
        for (int i = 0; i < levelOpponent.OpponentDeck.Count; i++) // set the game deck from the opponents.
        {
            AiDieDeck.Add(levelOpponent.OpponentDeck[i]);
        }

        life = levelOpponent.LifePoints;
        handSize = levelOpponent.handSize;
    }

    public void BeginTurn()
    {
        if (AiDieDeck.Count != 0)
        {
            Debug.Log("Dice");
            AITurnState = "Dice";
            this.GetComponent<AIDieController>().DrawDie();

        }else if (AiDieDeck.Count == 0)
        {
            Debug.Log("Board");
            AITurnState = "Board";
        }
    }


    IEnumerator WaitBetweenActionMain()
    {
        // StartCoroutine(WaitBetweenActionMain());
        yield return new WaitForSeconds(15);
        DetermineNextAction();
    }

    public void DetermineNextAction()
    {

    }
}
