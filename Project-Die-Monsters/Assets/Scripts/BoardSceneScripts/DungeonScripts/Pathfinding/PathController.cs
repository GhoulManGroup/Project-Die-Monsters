using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{ // This class is our controller script for player related use of pathfinding in our project.
    [Header("PlaceHolder Constraints")]
    public bool quickMove = false;
    public bool allowedToMove = false;

    [Header("Declerations")]
    GameObject levelController;
    LevelController LCScript;
    public GameObject chosenPiece; // the board piece we are moving or doing other things ect.

    [Header("Movement Varibles")]
    public string desiredAction = "Move";
    public GameObject startPosition;
    public GameObject desiredPosition;

    public List<GameObject> tilesToCheck = new List<GameObject>();
    public List<GameObject> checkedTiles = new List<GameObject>();
    public List<GameObject> reachableTiles = new List<GameObject>();
    public List<GameObject> chosenPathTiles = new List<GameObject>();

    public int possibleMoveDistance;

    public void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag("LevelController");
        LCScript = levelController.GetComponent<LevelController>();
    }

    // Move logic, declare a start position then > check movement crest pool & move cost of creature to determine how many tiles we can move. (Done)
    // Store all valid tiles that can be reached in that distance in a list? (Done)
    //Display that to the player.(Done)
    //Then allow on mouse down on the tile scripts to desginate a position to move to.(Half Done)
    //Have the piece on the board follow the optimal path between both points.(Half Done)

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
                establishPossibleMoves("CheckPossibleMoves");
                break;
        }
    }

    public void establishPossibleMoves(string checkWhat)
    {

        if (checkWhat == "CheckPossibleMoves")
        {
            if (tilesToCheck.Count != 0)
            {
                tilesToCheck[0].GetComponent<GridScript>().SearchForMoveSpots();

            }
            else if (tilesToCheck.Count == 0)
            {
                if (reachableTiles.Count != 0)
                {
                    allowedToMove = true;                
                }
            }
        }
        else if (checkWhat == "FindPath")
        { //Rework movement
                       
            if (tilesToCheck.Count != 0)
            {
                Debug.Log("Tile To Check");
               
            }
            else if (tilesToCheck.Count == 0)
            {
                Debug.Log("No More Tiles to Check");
               StartCoroutine("MovePieceThroughPath");
            }
        }
       
    }

    
    IEnumerator MovePieceThroughPath()
    {
        //Pick first tile in list going from count down < to move piece towards.----------------------------------------------------------------
        int currentMoveTarget = tilesToCheck.Count - 1;
        GameObject desiredPos = tilesToCheck[currentMoveTarget];
        GameObject currentPos = chosenPiece.GetComponent<CreatureToken>().myBoardLocation;

        //DetermineRotation(desiredPos, currentPos);
        Debug.Log("Before");
        yield return StartCoroutine(FaceTowards(2f, 180f));

        yield return null;
    }

    IEnumerator FaceTowards(float direction , float desiredDirection)
    {
        Debug.Log("Hello");
       Vector3 directionToTurn = new Vector3 (0f, direction, 0f);
       while (chosenPiece.transform.rotation.y != chosenPiece.transform.rotation.y)
       {
            yield return new WaitForSeconds(0.1f);
            chosenPiece.transform.Rotate(directionToTurn);
       }
       Debug.Log("Done");
    }

    void DetermineRotation(GameObject desiredPos, GameObject currentPos)
    {
        //Replace with a simple yvalue check on chosenpiece vs the y rotation needed based on the positon of our next tile. eg up = 270.
        chosenPiece.GetComponent<CreatureToken>().CheckState();
        string currentDir = chosenPiece.GetComponent<CreatureToken>().facingDirection;
        string directionToFace = "Dir";

        if (desiredPos.transform.position.x > currentPos.transform.position.x)
        {
            directionToFace = "Up";
        }

        if (desiredPos.transform.position.x < currentPos.transform.position.x)
        {
            directionToFace = "Down";
        }

        if (desiredPos.transform.position.z > currentPos.transform.position.z)
        {
            directionToFace = "Right";
        }

        if (desiredPos.transform.position.z < currentPos.transform.position.z)
        {
            directionToFace = "Left";
        }

    }

    public void HasMoved()
    {// Reset all lists and reassign the start position & end the movement phase.
        chosenPiece.GetComponent<CreatureToken>().HasMovedThisTurn = true;
        chosenPiece.GetComponent<CreatureToken>().CheckForAttackTarget();
        ResetBoard();
        startPosition.GetComponent<GridScript>().myState = "DungeonTile";
        startPosition.GetComponent<GridScript>().TileContents = "Empty";
        startPosition = chosenPiece.GetComponent<CreatureToken>().myBoardLocation;
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenAction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().OpenAndCloseControllerUI();
    }


    public void ResetBoard()
    {// Go through every grid tile that has been interacted with and reset it to its default state.
        for (int i = 0; i < checkedTiles.Count; i++)
        {
            checkedTiles[i].GetComponent<GridScript>().ResetGridTile();
        }
        tilesToCheck.Clear();
        checkedTiles.Clear();
        reachableTiles.Clear();
    }
}
