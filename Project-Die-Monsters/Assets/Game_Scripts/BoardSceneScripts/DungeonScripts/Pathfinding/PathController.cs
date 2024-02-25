using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{ // This class is our controller script for player related use of pathfinding in our project.
    [Header("PlaceHolder Constraints")]
    public bool quickMove = false;
    public bool possibleToMove = false;

    [Header("Declerations")]
    GameObject levelController;
    LevelController LCScript;
    public GameObject chosenPiece; // the board piece we are moving or doing other things ect.

    [Header("Pathfinding Varibles Movement")]
    public GameObject startPosition;
    public GameObject desiredPosition;

    public List<GameObject> tilesToCheck = new List<GameObject>(); // What tiles we want to check the state of its neighbours
    public List<GameObject> checkedTiles = new List<GameObject>();
    public List<GameObject> reachableTiles = new List<GameObject>(); //AI Go through this list and pick the one with the lowest value. 
    public List<GameObject> chosenPathTiles = new List<GameObject>();

    public int possibleMoveDistance; 
    public float rotationSpeed = 45;
    public float moveSpeed = 2f;
    float wantedDir;
    float directionToTurn;
    public Vector3 positionToMove;

    public void Awake()
    {
        levelController = GameObject.FindGameObjectWithTag("LevelController");
        LCScript = levelController.GetComponent<LevelController>();
    }

    //Rotate to face the direction we need to walk.

    public void DeclarePathfindingConditions(GameObject creatureTokenPicked)
    {
        chosenPiece = creatureTokenPicked;
        possibleMoveDistance = creatureTokenPicked.GetComponent<CreatureToken>().currentMoveDistance;
        startPosition = chosenPiece.GetComponent<CreatureToken>().myBoardLocation;
        tilesToCheck.Add(startPosition);
        //Old Crest Code
        //LCScript.participants[LCScript.currentTurnParticipant].GetComponent<Player>().moveCrestPoints / chosenPiece.GetComponent<CreatureToken>().moveCost;
        EstablishPossibleMoves("CheckPossibleMoves");
    }
    

    #region PathMovementCode
    public void EstablishPossibleMoves(string checkWhat)
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
                    possibleToMove = true;
                }
            }
        }
        else if (checkWhat == "ShowPossibleMoves")
        {
            for (int i = 0; i < reachableTiles.Count; i++)
            {
                reachableTiles[i].GetComponent<GridScript>().ShowPossibleMovements();
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

        //Rotate to Face it. Turn Back On When 3D Models are added for now sprites dont need to rotate as they are billboard sprites so this just slows the game down considerabley.
        //Also Rotation is bugged and still does not rotate in the shortest direction some times doing a 270 degreee turn rather than 90.
       
        // yield return StartCoroutine("Rotation");

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
            
        chosenPiece.GetComponent<CreatureToken>().FindTileBellowMe("Move");

        yield return null;
    }

    public void HasMoved()
    {
        //Set <CreatureToken> var myDir to value so we can use it to determine forward in AOE casting.
        switch (wantedDir)
        {
            case 0:
                chosenPiece.GetComponent<CreatureToken>().facingDirection = "East";
                break;
            case 90:
                chosenPiece.GetComponent<CreatureToken>().facingDirection = "South";
                break;
            case 270:
                chosenPiece.GetComponent<CreatureToken>().facingDirection = "North";
                break;
            case 180:
                chosenPiece.GetComponent<CreatureToken>().facingDirection = "West";
                break;
        }
        chosenPiece.GetComponent<CreatureToken>().hasMovedThisTurn = true;
        chosenPiece.GetComponent<CreatureToken>().CheckForAttackTarget();
        ResetBoard("Reset");
        startPosition.GetComponent<GridScript>().TileContents = "Empty";
        startPosition = chosenPiece.GetComponent<CreatureToken>().myBoardLocation;
        startPosition.GetComponent<GridScript>().TileContents = "Creature";

        //Add Check Here to see if Player or AI I current turn user ====================================================================== Then Do IF Else check to run the next step for either the PlayerCreatureController / AI Creature COntroller
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<PlayerCreatureController>().ChosenAction = "None";
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<PlayerCreatureController>().OpenAndCloseControllerUI();
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().boardInteraction = "None";
    }

    #endregion

    public void ResetBoard(string why)
    {// Go through every grid tile that has been interacted with and reset it to its default state.
     //Hides the Sprites above each tile that show movement or valid targets ect.
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