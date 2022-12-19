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

    public List<GameObject> tilesToCheck = new List<GameObject>(); // What tiles we want to check the state of its neighbours
    public List<GameObject> checkedTiles = new List<GameObject>();
    public List<GameObject> reachableTiles = new List<GameObject>();
    public List<GameObject> chosenPathTiles = new List<GameObject>();

    public int possibleMoveDistance;
    public float moveSpeed = 2f;
    public float rotationSpeed = 45;
    float wantedDir;
    float directionToTurn;

    public Vector3 positionToMove;

    public void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag("LevelController");
        LCScript = levelController.GetComponent<LevelController>();
    }

    //Rotate to face the direction we need to walk.

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
            case "Ability":
                chosenPiece = creatureTokenPicked;
                break;
        }
    }
    

    #region PathMovementCode
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
  
    IEnumerator MovePieceThroughPath()
    {
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

        yield return StartCoroutine("Rotation");

        //Move Towards It
        yield return StartCoroutine("WalkToTile");
  
        if (chosenPiece.GetComponent<CreatureToken>().myBoardLocation == desiredPosition)
        {
            HasMoved();
        }
        else if (chosenPiece.GetComponent<CreatureToken>().myBoardLocation != desiredPosition)
        {
            chosenPathTiles.Remove(chosenPathTiles[chosenPathTiles.Count -1]);
            StartCoroutine("MovePieceThroughPath");
        }

    }

    void NextTileLocation(GameObject desiredPos, GameObject currentPos)
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
        rotationSpeed = 45;
        float value = rotationSpeed;
        switch (chosenPiece.transform.eulerAngles.y)
        {
            case 0:
                switch (wantedDir)
                {
                    case 90:
                        rotationSpeed = value;
                        break;
                    case 270:
                        rotationSpeed = -value;
                        break;
                    case 180:
                        int dir = Random.Range(1, 2);
                        if (dir == 1)
                        {
                            rotationSpeed = -value;
                        }
                        else
                        {
                            rotationSpeed = value;
                        }
                        print(dir);
                        break;
                }
                break;
            case 90:
                switch (wantedDir)
                {
                    case 0:
                        rotationSpeed = -value;
                        break;
                    case 270:
                        int dir = Random.Range(1, 2);
                        if (dir == 1)
                        {
                            rotationSpeed = -value;
                        }
                        else
                        {
                            rotationSpeed = value;
                        }
                        
                        break;
                    case 180:
                        rotationSpeed = value;
                        break;
                }
                break;
            case 180:
                switch (wantedDir)
                {
                    case 90:
                        rotationSpeed = -value;
                        break;
                    case 270:
                        rotationSpeed = value;
                        break;
                    case 0:
                        int dir = Random.Range(1, 2);
                        if (dir == 1)
                        {
                            rotationSpeed = value;
                        }
                        else
                        {
                            rotationSpeed = -value;
                        }
                        print(dir);
                        break;
                }
                break;
            case 270:
                switch (wantedDir)
                {
                    case 90:
                        int dir = Random.Range(1, 2);
                        if (dir == 1)
                        {
                            rotationSpeed = -value;
                        }
                        else
                        {
                            rotationSpeed = -value;
                        }
                        break;
                    case 0:
                        rotationSpeed = value;
                        break;
                    case 180:
                        rotationSpeed = -value;
                        break;
                }
                break;
        }

    }
    IEnumerator Rotation()
    {
        float myDir = chosenPiece.transform.eulerAngles.y;

        while (myDir != wantedDir)
        {
            myDir = Mathf.Round(chosenPiece.transform.eulerAngles.y);
            chosenPiece.transform.Rotate(new Vector3(0f, rotationSpeed, 0f) * Time.deltaTime, Space.World);
            Debug.Log(rotationSpeed);
            yield return null;
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
        chosenPiece.GetComponent<CreatureToken>().hasMovedThisTurn = true;
        chosenPiece.GetComponent<CreatureToken>().CheckForAttackTarget();
        ResetBoard("Reset");
        startPosition.GetComponent<GridScript>().TileContents = "Empty";
        startPosition = chosenPiece.GetComponent<CreatureToken>().myBoardLocation;
        startPosition.GetComponent<GridScript>().TileContents = "Creature";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenAction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().OpenAndCloseControllerUI();
    }

    #endregion

    #region Abilties

    #endregion
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
                if (checkedTiles[i] != startPosition && checkedTiles[i] != desiredPosition && !chosenPathTiles.Contains(checkedTiles[i]))
                {
                 checkedTiles[i].GetComponent<GridScript>().ResetGridTile();
                }
            }
        }
    }
}