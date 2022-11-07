using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{ // This class is our controller script for player related use of pathfinding in our project.

    GameObject levelController;
    GameObject chosenPiece;
    public string desiredAction = "Move";
    public GameObject startPosition; // The grid tile we start our calculation at.
    public GameObject desiredPosition;

    public void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag("LevelController");
    }

    // Move logic, declare a start position then > check movement crest pool & move cost of creature to determine how many tiles we can move.
    // Store all valid tiles that can be reached in that distance in a list?
    //Display that to the player.
    //Then allow on mouse down on the tile scripts to desginate a position to move to.
    //Have the piece on the board follow the optimal path between both points.

    public void DeclareConditions(GameObject creatureTokenPicked, string wantedAction)
    {
        switch (wantedAction)
        {
            case "Move":
                chosenPiece = creatureTokenPicked;
                startPosition = chosenPiece.GetComponent<CreatureToken>().myBoardLocation;
                establishPossibleMoves();
                break;
        }
    }

    void establishPossibleMoves()
    {
        int possibleMoveDistance;
        // do Maths.
        
    }

}
