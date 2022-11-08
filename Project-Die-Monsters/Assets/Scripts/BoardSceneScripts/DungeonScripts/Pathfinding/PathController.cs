using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{ // This class is our controller script for player related use of pathfinding in our project.

    GameObject levelController;
    LevelController LCScript;
    GameObject chosenPiece;

    [Header("Movement Varibles")]
    public string desiredAction = "Move";
    public GameObject startPosition;
    public GameObject desiredPosition;

    List<GameObject> tilesToCheck = new List<GameObject>();
    List<GameObject> checkedTiles = new List<GameObject>();
    List<GameObject> reachableTiles = new List<GameObject>();
    public void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag("LevelController");
        LCScript = levelController.GetComponent<LevelController>();
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
        possibleMoveDistance = LCScript.participants[LCScript.turnPlayer].GetComponent<Player>().moveCrestPoints / chosenPiece.GetComponent<CreatureToken>().moveCost;
        Debug.Log(possibleMoveDistance);       
    }

}
