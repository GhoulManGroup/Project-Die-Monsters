using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttackUIScript : MonoBehaviour
{
    GameObject lvlRef;
    GameObject creatureTargets;

    [Header("Target Pick")]
    public List<GameObject> targetPick = new List<GameObject>();
    public List<Text> targetText = new List<Text>();

    public GameObject attacker;
    public GameObject defender;

    public void displayAttackWindow()
    {

    }

    public void hideAttackWindow()
    {

    }


}
