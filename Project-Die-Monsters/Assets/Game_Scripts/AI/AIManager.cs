using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Resources")]
    public List<GameObject> myCreatures = new List<GameObject>();

    [Header("PossibleActions")]
    public int canAttack = 0;
    public int abiltiesCanBeCast;
    public void BeginTurn()
    {
        currentOpponent.GetComponent<AIOpponent>().SetUp();
        Debug.Log("Being Turn Function");
        currentAction.SetActive(true);
        currentActionText.GetComponent<TextMeshProUGUI>().text = "AI Turn Start";
        //Determine Priority of what to day :?

        this.GetComponent<AIRollManager>().SetUpAIDice();
    }

    //Start AI Turn

    //Display AI Turn

    //Display Dice Roll

    //Start AI Roll Manager
}
