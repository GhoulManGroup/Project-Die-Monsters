using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreatureController : MonoBehaviour
{
    public List<GameObject> myCreatures = new List<GameObject>();

    PathController pathfinding;
    public CreatureToken creature;
    AbilityManager ability;

    public bool actionsDone = false;
    public bool actionDone = false;

    [Header("Creature Actions")]
    [SerializeField] bool canMove = false;
    [SerializeField] bool wantToMove = true;
    [SerializeField] bool canAttack = false;
    [SerializeField] bool CanAbility = false;

    public void Start()
    {
        pathfinding = GameObject.FindGameObjectWithTag("LevelController").GetComponent<PathController>();
    }

    public IEnumerator ActionPhase()
    {
        for (int i = 0; i < myCreatures.Count; i++)
        {
            creature = myCreatures[i].GetComponent<CreatureToken>();

            ability = myCreatures[i].GetComponent<AbilityManager>();

            Debug.Log(i + creature.name);

            yield return CheckPossibleActions();

            actionsDone = false;

            yield return PerformActions();

            while (actionsDone == false)
            {
                yield return null;
            }
        }

       this.GetComponent<AIManager>().PhaseDone= true;
    }

    private void ResetToDefault()
    {
        actionsDone = false;
        actionDone = false;
    }

    [SerializeField]  int actionsToTake = 0;

    public IEnumerator CheckPossibleActions()
    {
        Debug.Log("CheckAction");
        actionsToTake = 0;

        yield return  ability.StartCoroutine("CheckAbilityCanBeCast");
        if (ability.canBeCast == true && creature.hasUsedAbilityThisTurn == false && creature.abilityCost <= this.GetComponent<AIManager>().currentOpponent.GetComponent<AIOpponent>().abiltyPowerCrestPoints)
        {
            CanAbility = true;
            Debug.Log("Can Cast Ability");
            actionsToTake += 1;
        }

        creature.CheckForAttackTarget();
        if (creature.canReachTarget == true && creature.hasAttackedThisTurn == false)
        {
            canAttack = true;
            Debug.Log("Can Attack");
            actionsToTake += 1;
        }

        yield return pathfinding.StartCoroutine("DeclarePathfindingConditions", creature.gameObject);
        if (creature.hasMovedThisTurn == false && pathfinding.possibleToMove == true)
        {
            if (canAttack == true)
            {
                wantToMove = false;
            }else
            {
                wantToMove = true;
                canMove = true;
                actionsToTake += 1;
            }
        }
    }

    IEnumerator PerformActions()
    {
        Debug.Log("StartingPerformAction");

        if (canAttack == true)
        {
            yield return StartCoroutine(AICreatureAttack());
        }

        if (canMove == true && wantToMove == true)
        {
            yield return StartCoroutine(AICreatureMove());
        }

        if (CanAbility == true)
        {
            yield return StartCoroutine(AICreatureCastAbility());
        }

        yield return CheckPossibleActions();

        if (actionsToTake != 0)
        {
            Debug.Log("Still Actions Can Be Taken By Creature" + creature.name);
            PerformActions();
        }
        else
        {
            Debug.Log("No More Actions Any MOre");
            actionsDone = true;
        }   
    }

    IEnumerator AICreatureAttack()
    {
        yield return null;
    }

    IEnumerator AICreatureCastAbility()
    {
        yield return null;
    }

    IEnumerator AICreatureMove()
    {
        GameObject tileChosen = pathfinding.reachableTiles[0];

        for (int i = 0; i < pathfinding.reachableTiles.Count; i++)
        {
            if (pathfinding.reachableTiles[i].GetComponent<GridScript>().distanceFromPlayerDungeonLord < tileChosen.GetComponent<GridScript>().distanceFromPlayerDungeonLord)
            {
                Debug.Log("Found Closer Tile Move to this instead" + tileChosen.gameObject.name);
                tileChosen = pathfinding.reachableTiles[i];
            }
        }

        pathfinding.desiredPosition = tileChosen;
        pathfinding.tilesToCheck.Add(pathfinding.desiredPosition);
        pathfinding.EstablishPossibleMoves("FindPath");

        while (actionDone == false)
        {
            yield return null;
        }

        actionDone = false;


        Debug.Log("Move Action Done For Creature");
    }

    public void ResetCreatures()
    {
        for (int i = 0; i < myCreatures.Count; i++)
        {
            myCreatures[i].GetComponent<CreatureToken>().CreatureResetTurnEnd();
        }
    }


}
