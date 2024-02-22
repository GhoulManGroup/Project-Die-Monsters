using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreatureController : MonoBehaviour
{
    public List<GameObject> myCreatures = new List<GameObject>();

    PathController pathfinding;
    CreatureToken creature;
    AbilityManager ability;
    bool actionsDone = false;

    public bool creatureCheckDone = false;

    public void Start()
    {
        pathfinding = GameObject.FindGameObjectWithTag("LevelController").GetComponent<PathController>();
    }

    public IEnumerator ActionPhase()
    {
        for (int i = 0; i < myCreatures.Count; i++)
        {
            creature = myCreatures[0].GetComponent<CreatureToken>();
            ability = myCreatures[0].GetComponent<AbilityManager>();
            CheckPossibleActions();
            actionsDone = false;
            yield return PerformActions();

            while(actionsDone == false)
            {
                yield return null;
            }
        }

        yield return null;
    }

    public void CheckPossibleActions()
    {
        pathfinding.StartCoroutine("DeclarePathfindingConditions", myCreatures[0]);
    }

    IEnumerator PerformActions()
    {
        if (creature.canReachTarget == true)
        {

        }

        if (ability.canBeCast == true)
        {

        }

        if (pathfinding.possibleToMove == true)
        {

        }

        //Check Creature can do any of the three actions // Then Do them Then Check Again till all actions are false

        // Add this check for all creatures in list so do a diffrent corutine I suppose.

        yield return StartCoroutine(AICreatureAttack());

        yield return StartCoroutine(AICreatureCastAbility());

        yield return StartCoroutine(AICreatureMove());
        
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
