using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOpponent : MonoBehaviour
{
    public Opponent myOpponent;
    [Header("Resources")]
    public int summmonCrestPoints = 0;
    public int moveCrestPoints = 0;
    public int attackCrestPoints = 0;
    public int defenceCrestPoints = 0;
    public int abiltyPowerCrestPoints = 0;

    public List<Creature> CreaturePool = new List<Creature>();
}
