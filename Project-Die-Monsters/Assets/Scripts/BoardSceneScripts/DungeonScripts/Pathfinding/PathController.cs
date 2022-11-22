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

    [Header("Pathfinding Varibles")]
    public string desiredAction = "Move";
    public GameObject startPosition;
    public GameObject desiredPosition;

    public List<GameObject> tilesToCheck = new List<GameObject>();
    public List<GameObject> checkedTiles = new List<GameObject>();
    public List<GameObject> reachableTiles = new List<GameObject>();
    public List<GameObject> chosenPathTiles = new List<GameObject>();

    public int possibleMoveDistance;
    float wantedDir;
    float directionToTurn;

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
                //We will always have 1 gameobject in this list untill our desired tile has found its path back to start position.
                tilesToCheck[0].GetComponent<GridScript>().SearchForPath();
               
            }
            else if (tilesToCheck.Count == 0)
            {
                //transform.LookAt();
                Debug.Log("No More Tiles to Check");
               StartCoroutine("MovePieceThroughPath");
            }
        }
       
    }

    
    IEnumerator MovePieceThroughPath()
    {
        // Check the size of chosenPath, if its 0 then we are next to desired position else  we pick the tile closest to start being the last one added so listName.count
        GameObject currentPos = chosenPiece.GetComponent<CreatureToken>().myBoardLocation;
        GameObject desiredPos = null;
        if (chosenPathTiles.Count == 0)
        {
            desiredPos = desiredPosition;

        }else if (chosenPathTiles.Count > 0)
        {
            desiredPos = chosenPathTiles[chosenPathTiles.Count - 1];
        }

        DetermineRotation(desiredPos, currentPos);
        StartCoroutine("Rotation");
        Debug.Log("Before");

        yield return null;
    }

    IEnumerator Rotation()
    {
        chosenPiece.transform.Rotate(0f, directionToTurn, 0f);
        yield return new WaitForFixedUpdate();
        //Debug.Log(chosenPiece.transform.eulerAngles.y);

        if (chosenPiece.transform.eulerAngles.y != wantedDir)
        {
            StartCoroutine("Rotation");
        }
        else if (chosenPiece.transform.eulerAngles.y == wantedDir)
        {
            Debug.Log("EscaPEd");
            
        }
    }

    void DetermineRotation(GameObject desiredPos, GameObject currentPos)
    {
        //Where we want to face
        if (desiredPos.transform.position.x > currentPos.transform.position.x)
        {
            wantedDir = 90;
        }

        if (desiredPos.transform.position.x < currentPos.transform.position.x)
        {
            wantedDir = 270;
        }

        if (desiredPos.transform.position.z > currentPos.transform.position.z)
        {
            wantedDir = 0f;
        }

        if (desiredPos.transform.position.z < currentPos.transform.position.z)
        {
            wantedDir = 180;
        }
        //Where we currenly face & how we get to face wantedir;
        string currentDir = "null";
        switch (chosenPiece.transform.eulerAngles.y)
        {
            case 0:
                currentDir = "Right";
                switch (wantedDir)
                {
                    case 90:

                        break;
                    case 270:

                        break;
                    case 0:

                        break;
                    case 180:

                        break;
                }
                break;
            case 90:
                currentDir = "Down";
                break;
            case 180:
                currentDir = "Left";
                break;
            case 270:
                currentDir = "Up";
                break;
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
