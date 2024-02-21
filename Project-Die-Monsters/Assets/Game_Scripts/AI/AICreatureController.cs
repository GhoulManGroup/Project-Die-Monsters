using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreatureController : MonoBehaviour
{
    public List<GameObject> myCreatures = new List<GameObject>();

    PathController pathfinding;
    CreatureToken creature;

    public bool creatureCheckDone = false;

    public void Start()
    {
        pathfinding = GameObject.FindGameObjectWithTag("LevelController").GetComponent<PathController>();
    }

    public IEnumerator PerformActions()
    {
        creature = myCreatures[0].GetComponent<CreatureToken>();
        pathfinding.StartCoroutine("DeclarePathfindingConditions", creature);

        //Check Creature can do any of the three actions // Then Do them Then Check Again till all actions are false

        // Add this check for all creatures in list so do a diffrent corutine I suppose.

        yield return StartCoroutine(AICreatureAttack());

        yield return StartCoroutine(AICreatureCastAbility());

        yield return StartCoroutine(AICreatureMove());
        

        yield return null;
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

    // for each creature check possible actions that can be taken

    //Attack

    //Move

    //Ability 

}
