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

    public bool creatureCheckDone = false;

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

            while(actionsDone == false)
            {
                yield return null;
            }
        }

        yield return null;
    }
    int actionsToTake = 0;

    public IEnumerator CheckPossibleActions()
    {
        yield return pathfinding.StartCoroutine("DeclarePathfindingConditions", myCreatures[0]);
        if (creature.canReachTarget == true && creature.hasAttackedThisTurn == false)
        {
            actionsToTake += 1;
        }

        yield return  ability.StartCoroutine("CheckAbilityCanBeCast");
        if (ability.canBeCast == true && creature.hasUsedAbilityThisTurn == false && ability.)
        {
            actionsToTake += 1;
        }

        creature.CheckForAttackTarget();
        if (creature.canReachTarget == true)
        {
            actionsToTake += 1;
        }

        Debug.Log(actionsToTake);
    }

    IEnumerator PerformActions()
    {

        {
            yield return StartCoroutine(AICreatureAttack());
        }

        if (ability.canBeCast == true && )
        {
            yield return StartCoroutine(AICreatureCastAbility());
        }

        if (pathfinding.possibleToMove == true && creature.hasMovedThisTurn == false)
        {
            yield return StartCoroutine(AICreatureMove());
        }

        if (actionsToTake != 0)
        {

        }
        else
        {

        }


        //Check Creature can do any of the three actions // Then Do them Then Check Again till all actions are false

        // Add this check for all creatures in list so do a diffrent corutine I suppose.
        
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
