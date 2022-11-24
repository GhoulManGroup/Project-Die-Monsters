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
            positionToMove = new Vector3(desiredPos.transform.position.x, chosenPiece.transform.position.y, desiredPos.transform.position.z);
            //StopCoroutine("MovePieceThroughPath");

        }
        else if (chosenPathTiles.Count > 0)
        {
            desiredPos = chosenPathTiles[chosenPathTiles.Count - 1];
            positionToMove = new Vector3(desiredPos.transform.position.x , chosenPiece.transform.position.y, desiredPos.transform.position.z);
        }

        //Find where the next tile in the path is in relation to us.
        NextTileLocation(desiredPos, currentPos);
        //Rotate to Face it.
        //StartCoroutine("Rotation");
        //Move Towards It
        yield return StartCoroutine("WalkToTile");
        //Then if we are in desired pos end coroutine.
        if (chosenPiece.GetComponent<CreatureToken>().myBoardLocation == desiredPosition)
        {
            Debug.Log("Reached End");
            HasMoved();
        }
        else
        {
            Debug.Log("Hello");
            chosenPathTiles.Remove(chosenPathTiles[chosenPathTiles.Count -1]);
            StartCoroutine("MovePieceThroughPath");
        }

    }

    IEnumerator Rotation()
    {
        
        //Find direciton to face. 
        ///Find our rotation.y
        ///Find the direction our tile is in.
        //Find shortest rotation to get there
        ///Determine which way 
        //Rotate that way
        while(chosenPiece.transform.eulerAngles.y != wantedDir)
        {
            chosenPiece.transform.Rotate(0f, directionToTurn, 0f);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitUntil(() => chosenPiece.transform.eulerAngles.y == wantedDir);
        //Debug.Log(chosenPiece.transform.eulerAngles.y);

        /*
        if (chosenPiece.transform.eulerAngles.y != wantedDir)
        {
            StartCoroutine("Rotation");
        }
        else if (chosenPiece.transform.eulerAngles.y == wantedDir)
        {
            Debug.Log("EscaPEd");
            //chosenPiece.transform.eulerAngles = Vector3 (0f, wantedDir, 0f);


        }
        */
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
            switch (WhereNextTile)
            {
                case "Up":
                    chosenPiece.transform.position += new Vector3(-0.1f, 0f, 0f);
                    break;
                case "Down":
                    chosenPiece.transform.position += new Vector3(0.1f, 0f, 0f);
                    break;
                case "Left":
                    chosenPiece.transform.position += new Vector3(0f, 0f, -0.1f);
                    break;
                case "Right":
                    chosenPiece.transform.position += new Vector3(0f, 0f, 0.1f);
                    break;
            }
        }
        yield return new WaitForSeconds(1f);
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenCreatureToken.GetComponent<CreatureToken>().declareTile("Move");
        yield return null;
    }

    public void HasMoved()
    {
        Debug.Log("End of the movement reached desired position.");
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
