using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{ // This class is our controller script for player related use of pathfinding in our project.

    GameObject levelController;
    LevelController LCScript;
    GameObject chosenPiece; // the board piece we are moving or doing other things ect.

    [Header("Movement Varibles")]
    public string desiredAction = "Move";
    public GameObject startPosition;
    public GameObject desiredPosition;

    public List<GameObject> tilesToCheck = new List<GameObject>();
    public List<GameObject> checkedTiles = new List<GameObject>();
    public List<GameObject> reachableTiles = new List<GameObject>();

    public int possibleMoveDistance;

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
                //To get here we have the crests to move but we need to find if there is anywhere we can move.
                chosenPiece = creatureTokenPicked;
                startPosition = chosenPiece.GetComponent<CreatureToken>().myBoardLocation;
                tilesToCheck.Add(startPosition);
                possibleMoveDistance = LCScript.participants[LCScript.turnPlayer].GetComponent<Player>().moveCrestPoints / chosenPiece.GetComponent<CreatureToken>().moveCost;        
                establishPossibleMoves();
                break;
        }
    }

    public void establishPossibleMoves()
    {
        Debug.Log("EPM");
        if (tilesToCheck.Count != 0)
        {
            Debug.Log("TTC");
            tilesToCheck[0].GetComponent<GridScript>().SearchForMoveSpots();
            
        }else if (tilesToCheck.Count == 0)
        {
            Debug.Log("Done");
        }
    }

}
