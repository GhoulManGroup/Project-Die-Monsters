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
    public float moveSpeed = 0.1f;
    float wantedDir;
    float directionToTurn;

    public Vector3 positionToMove;
    string WhereNextTile = "null";

    public void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag("LevelController");
        LCScript = levelController.GetComponent<LevelController>();
    }

    // Move logic, declare a start position then > check movement crest pool & move cost of creature to determine how many tiles we can move. (Done)
    // Store all valid tiles that can be reached in that distance in a list? (Done)
    //Display that to the player.(Done)
    //Then allow on mouse down on the tile scripts to desginate a position to move to.(Done);
    //Rotate to face the direction we need to walk.
    //Have the piece on the board follow the optimal path between both points.(Done)

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
                tilesToCheck[0].GetComponent<GridScript>().FindPossibleMovements();

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
        {                   
            if (tilesToCheck.Count != 0)
            {
                tilesToCheck[0].GetComponent<GridScript>().FindPossiblePathToStart();
               
            }
            else if (tilesToCheck.Count == 0)
            {
                ResetBoard("ShowOnlyPath");
               StartCoroutine("MovePieceThroughPath");
            }
        }
       
    }

    
    IEnumerator MovePieceThroughPath() //This is soruce of broken code?
    {
        Debug.Log("Start of Move Function");
        // Check the size of chosenPath, if its 0 then we are next to desired position else  we pick the tile closest to start being the last one added so listName.count
        GameObject currentPos = chosenPiece.GetComponent<CreatureToken>().myBoardLocation;
        GameObject desiredPos = null;
        if (chosenPathTiles.Count == 0)
        {
            desiredPos = desiredPosition;
            positionToMove = new Vector3(desiredPos.transform.position.x, chosenPiece.transform.position.y, desiredPos.transform.position.z);
        }
        else if (chosenPathTiles.Count > 0)
        {
            desiredPos = chosenPathTiles[chosenPathTiles.Count - 1];
            positionToMove = new Vector3(desiredPos.transform.position.x , chosenPiece.transform.position.y, desiredPos.transform.position.z);
        }

        //Find where the next tile in the path is in relation to us.
        NextTileLocation(desiredPos, currentPos);
        //Rotate to Face it.
        //yield return StartCoroutine("Rotation");

        //Move Towards It
        yield return StartCoroutine("WalkToTile");
  
        if (chosenPiece.GetComponent<CreatureToken>().myBoardLocation == desiredPosition)
        {
            HasMoved();
        }
        else if (chosenPiece.GetComponent<CreatureToken>().myBoardLocation != desiredPosition)
        {
            //Debug.Log("Next Tile Up");
            chosenPathTiles.Remove(chosenPathTiles[chosenPathTiles.Count -1]);
            StartCoroutine("MovePieceThroughPath");
        }

    }

    IEnumerator Rotation()
    {
       
        while(chosenPiece.transform.eulerAngles.y != wantedDir)
        {
            chosenPiece.transform.Rotate(new Vector3(0f, moveSpeed, 0f) * Time.deltaTime, Space.World);
            yield return null;
        }
    }

    void NextTileLocation(GameObject desiredPos, GameObject currentPos)
    {
        //Where we want to face
        if (desiredPos.transform.position.x > currentPos.transform.position.x)
        {
            wantedDir = 90;
            WhereNextTile = "Down";
        }

        if (desiredPos.transform.position.x < currentPos.transform.position.x)
        {
            wantedDir = 270;
            WhereNextTile = "Up";
        }

        if (desiredPos.transform.position.z > currentPos.transform.position.z)
        {
            wantedDir = 0f;
            WhereNextTile = "Right";
        }

        if (desiredPos.transform.position.z < currentPos.transform.position.z)
        {
            wantedDir = 180;
            WhereNextTile = "Left";
        }
        //Where we currenly face & how we get to face wantedir;
       
        switch (chosenPiece.transform.eulerAngles.y)
        {
            case 0:
                //WhereNextTile = "Right";
                switch (wantedDir)
                {
                    case 90:
                        directionToTurn = 15;
                        break;
                    case 270:
                        directionToTurn = -15;
                        break;
                    case 180:
                        directionToTurn = 15;
                        break;
                }
                break;
            case 90:
                //WhereNextTile = "Down";
                switch (wantedDir)
                {
                    case 0:
                        directionToTurn = -15;
                        break;
                    case 270:
                        directionToTurn = -15;
                        break;
                    case 180:
                        directionToTurn = 15;
                        break;
                }
                break;
            case 180:
               // WhereNextTile = "Left";
                switch (wantedDir)
                {
                    case 90:
                        directionToTurn = -15;
                        break;
                    case 270:
                        directionToTurn = 15;
                        break;
                    case 0:
                        directionToTurn = 15;
                        break;
                }
                break;
            case 270:
                //WhereNextTile = "Up";
                switch (wantedDir)
                {
                    case 90:
                        directionToTurn = 15;
                        break;
                    case 0:
                        directionToTurn = -15;
                        break;
                    case 180:
                        directionToTurn = 15;
                        break;
                }
                break;
        }

    }

    IEnumerator WalkToTile()
    {
        while (chosenPiece.transform.position != (positionToMove))
        {
            chosenPiece.transform.position = Vector3.MoveTowards(chosenPiece.transform.position, positionToMove, moveSpeed* Time.deltaTime);
            yield return null;
        }
            GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenCreatureToken.GetComponent<CreatureToken>().declareTile("Move");
            yield return null;
    }

    public void HasMoved()
    {
        chosenPiece.GetComponent<CreatureToken>().HasMovedThisTurn = true;
        chosenPiece.GetComponent<CreatureToken>().CheckForAttackTarget();
        ResetBoard("Reset");
        startPosition.GetComponent<GridScript>().TileContents = "Empty";
        startPosition = chosenPiece.GetComponent<CreatureToken>().myBoardLocation;
        startPosition.GetComponent<GridScript>().TileContents = "Creature";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenAction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().OpenAndCloseControllerUI();
    }


    public void ResetBoard(string why)
    {// Go through every grid tile that has been interacted with and reset it to its default state.
        if (why == "Reset")
        {
            for (int i = 0; i < checkedTiles.Count; i++)
            {
                checkedTiles[i].GetComponent<GridScript>().ResetGridTile();
            }
            tilesToCheck.Clear();
            checkedTiles.Clear();
            reachableTiles.Clear();
        }
        else if (why == "ShowOnlyPath")
        {
            for (int i = 0; i < checkedTiles.Count; i++)
            {
                if (chosenPathTiles.Contains(chosenPathTiles[i].gameObject))
                {
                }else
                {

                    checkedTiles[i].GetComponent<GridScript>().ResetGridTile();
                }
            }
        }
    }
}