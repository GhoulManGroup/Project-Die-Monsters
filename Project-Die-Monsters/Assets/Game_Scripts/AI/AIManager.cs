using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIManager : MonoBehaviour
{
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
        Debug.Log("Being Turn Function");
        currentAction.SetActive(true);
        currentActionText.GetComponent<TextMeshProUGUI>().text = "AI Turn Start";
        this.GetComponent<AIRollManager>().SetUpAIDice();
    }

    //Start AI Turn

    //Display AI Turn

    //Display Dice Roll

    //Start AI Roll Manager
}
