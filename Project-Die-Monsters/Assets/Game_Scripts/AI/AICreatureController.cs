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

    [Header("Creature Actions")]
    bool canMove = false;
    bool canAttack = false;
    bool CanAbility = false;

    public void Start()
    {
        pathfinding = GameObject.FindGameObjectWithTag("LevelController").GetComponent<PathController>();
    }

    public IEnumerator ActionPhase()
    {
        for (int i = 0; i < myCreatures.Count; i++)
        {
            Debug.Log(i);

            creature = myCreatures[0].GetComponent<CreatureToken>();

            ability = myCreatures[0].GetComponent<AbilityManager>();

            yield return CheckPossibleActions();

            actionsDone = false;

            yield return PerformActions();

            while (actionsDone == false)
            {
                yield return null;
            }
        }

        yield return null;
    }

    int actionsToTake = 0;

    public IEnumerator CheckPossibleActions()
    {
        actionsToTake = 0;

        yield return pathfinding.StartCoroutine("DeclarePathfindingConditions", myCreatures[0]);
        if (creature.hasMovedThisTurn == false && pathfinding.possibleToMove == true)
        {
            canMove = true;
            Debug.Log("Can Move");
            actionsToTake += 1;
        }

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
    }

    IEnumerator PerformActions()
    {
        if (canAttack == true)
        {
            yield return StartCoroutine(AICreatureAttack());
        }

        if (CanAbility == true)
        {
            yield return StartCoroutine(AICreatureCastAbility());
        }

        if (canMove == true)
        {
            yield return StartCoroutine(AICreatureMove());
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
                Debug.Log("Found Closer Tile Move to this instead");
                tileChosen = pathfinding.reachableTiles[i];
            }
        }

        pathfinding.desiredPosition = tileChosen;
        pathfinding.tilesToCheck.Clear();
        pathfinding.tilesToCheck.Add(pathfinding.desiredPosition);
        pathfinding.EstablishPossibleMoves("FindPath");
 
        // Pick by declaring the tile that is in possible moves in path controller with the loqest tile value
        yield return null;
    }


}
