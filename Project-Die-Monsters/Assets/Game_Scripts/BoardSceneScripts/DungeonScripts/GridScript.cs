using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridScript : MonoBehaviour
{
    [Header("refrences")]
    GameObject LvlRef;
    [SerializeField]
    GameObject myTextObject;
    [SerializeField]
    GameObject myIndicator;

    [Header("Tile States")]
    public string currrentInteraction = "None"; // On mouse down filter varible to allow us to press tiles for diffrent reasons eg declare target tile our ability location for teleport ect?
    public bool turnPlayerDungeonConnection = false; // the tile is next to a tile owned by the turn player to check if it is a valid placement.
    public string myState = "Empty"; //Empty // DungeonTile // DungeonLord. Tracks the current form of the tile.
    public string myOwner = "None"; // tracks which participant owns the tile either no one 1 player or an AI.
    public string TileContents = "Empty"; // Empty // Creature. Does the tile have a creature piece above it.

    [Header("PathFinding")]
    public int distanceFromStartTile = 0;

    [Header("Tile Visuals")]
    public List<Material> myMat = new List<Material>();
    public List<Material> indicatorMat = new List<Material>();  

    [Header("PrefabsToSpawn")]
    public GameObject creatureSpawnFab;

    [Header("PieceManagement")]
    public string desiredDir;
    public List<GameObject> Neighbours = new List<GameObject>();
    //These are here for AOE since some tiles have less than 4 neighbours and I cant use the existing list for what I want and don't want to break pathfinding.
    GameObject NeighbourNorth;
    GameObject NeighbourEast;
    GameObject NeighbourSouth;
    GameObject NeighbourWest;

    void Start()
    {
        myTextObject.GetComponent<TextMeshPro>().text = " ";
        LvlRef = GameObject.FindGameObjectWithTag("LevelController");
        DeclareNeighbours();
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        switch (myState)
        {
            case "Empty":
                this.GetComponent<MeshRenderer>().material = myMat[0];
                break;

            case "DungeonTile":
                if (myOwner == "Player0")
                {
                    this.GetComponent<MeshRenderer>().material = myMat[1];
                }

                if (myOwner == "Player1")
                {
                    this.GetComponent<MeshRenderer>().material = myMat[3];
                }

                break;

            case "DungeonLord":
                this.GetComponent<MeshRenderer>().material = myMat[2];
                break;
        }
    }

    public void DeclareNeighbours()
    {
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    RaycastHit Forward;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Forward, 1f))
                    {
                        if (Forward.collider.GetComponent<GridScript>() != null)
                        {
                            NeighbourEast = Forward.collider.gameObject;
                            Neighbours.Add(Forward.collider.gameObject);
                        }
                    }
                    break;
                case 1:
                    RaycastHit Back;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out Back, 1f))
                    {
                        if (Back.collider != null)
                        {
                            if (Back.collider.GetComponent<GridScript>() != null)
                            {
                                NeighbourWest = Back.collider.gameObject;
                                Neighbours.Add(Back.collider.gameObject);
                            }
                        }
                    }
                    break;
                case 2:
                    RaycastHit Left;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out Left, 1f))
                    {
                        if (Left.collider != null)
                        {
                            if (Left.collider.GetComponent<GridScript>() != null)
                            {
                                NeighbourNorth = Left.collider.gameObject;
                                Neighbours.Add(Left.collider.gameObject);
                            }
                        }
                    }
                    break;
                case 3:
                    RaycastHit Right;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out Right, 1f))
                    {
                        if (Right.collider != null)
                        {
                            if (Right.collider.GetComponent<GridScript>() != null)
                            {
                                NeighbourSouth = Right.collider.gameObject;
                                Neighbours.Add(Right.collider.gameObject);
                            }
                        }
                    }
                    break;
            }

        }
    }

    public void updateTMPRO()
    {
        myTextObject.GetComponent<TextMeshPro>().text = distanceFromStartTile.ToString();
    }

    #region Dungeon & Creature Placement
    public void CheckForDungeonConnection()
    {
        // If the tile that the dungeon spawner is above is empty and vaild for placement, check connecting tile states to determin connections.
        if (myState == "Empty")
        {
            for (int i = 0; i < Neighbours.Count; i++)
            {
                if (Neighbours[i].GetComponent<GridScript>().myOwner == LvlRef.GetComponent<LevelController>().whoseTurn) // tile is connected to our dungeon
                {
                    if (Neighbours[i].GetComponent<GridScript>().myState == "DungeonTile")
                    {
                        turnPlayerDungeonConnection = true;
                    }

                    if (Neighbours[i].GetComponent<GridScript>().myState == "DungeonLord")
                    {
                        turnPlayerDungeonConnection = true;
                    }
                }
            }
        }
    }

    public void SpawnCreatureAbove()
    {
        Instantiate(creatureSpawnFab, new Vector3(this.transform.position.x, 0.3f, this.transform.position.z), Quaternion.identity);
    }
    #endregion


    #region CreatureMovementCode
    public void IsAnyMovementPossible() 
    {
        LvlRef.GetComponent<CreatureController>().ableToMove = false;
        //This function checks if we can move at all before allowing the player to click the move button on the creature controller UI.
        for (int i = 0; i < Neighbours.Count; i++)
        {
            if (Neighbours[i].GetComponent<GridScript>().myState == "DungeonTile" && Neighbours[i].GetComponent<GridScript>().TileContents == "Empty")
            {
                LvlRef.GetComponent<CreatureController>().ableToMove = true;
            }
        }
    }

    public void FindPossibleMovements()
    {
        // We know how many tiles from start pos we could move now we check if there is anywhere we can move.
        for (int i = 0; i < Neighbours.Count; i++) 
        {
            if (LvlRef.GetComponent<PathController>().tilesToCheck.Contains(Neighbours[i].gameObject) || LvlRef.GetComponent<PathController>().checkedTiles.Contains(Neighbours[i].gameObject))
            {
                //Already Checked don't do anything more with it.
            }
            else
            {
                int Dist = distanceFromStartTile + 1;
                // Add 1 to distance from  its current distance from start tile value then if that value is within our possible move distance that neighbour is in reach of our board piece.
                if (Neighbours[i].GetComponent<GridScript>().distanceFromStartTile + Dist <= LvlRef.GetComponent<PathController>().possibleMoveDistance)
                {//The above statement can't be more than 1 beacuse when it is checked it will always be 0
                    //Check then if that tile is a dungeon tile and is currently not containing any other board piece.
                    if (Neighbours[i].GetComponent<GridScript>().myState == "DungeonTile" && Neighbours[i].GetComponent<GridScript>().TileContents == "Empty")
                    {
                        //Then if all these conditions are met we add the tile to the list to check and increase that tiles distancefromstart by 1 space to indicate its 1 away from this tile.
                        LvlRef.GetComponent<PathController>().tilesToCheck.Add(Neighbours[i].gameObject);
                        Neighbours[i].gameObject.GetComponent<GridScript>().distanceFromStartTile = distanceFromStartTile + 1;
                        Neighbours[i].gameObject.GetComponent<GridScript>().SetIndicatorMaterial("MoveSpace");
                        Neighbours[i].gameObject.GetComponent<GridScript>().updateTMPRO();
                        //Add the neighbour to the list of possible tiles to move to.
                        LvlRef.GetComponent<PathController>().reachableTiles.Add(Neighbours[i].gameObject);
                    }
                }
            }
        }
        //Remove this checked tile from the list of tiles to check add it to the checked list.
        LvlRef.GetComponent<PathController>().tilesToCheck.Remove(this.gameObject);
        LvlRef.GetComponent<PathController>().checkedTiles.Add(this.gameObject);
        LvlRef.GetComponent<PathController>().EstablishPossibleMoves("CheckPossibleMoves");
    }

    public void FindPossiblePathToStart()
    {
        //This list exists to pick a path if there are branching valid choices back.
        List<GameObject> dupliacteProtect = new List<GameObject>();

        for (int i = 0; i < Neighbours.Count; i++)
        {//If the current one isn't start then check if its one of the grid tiles we already established as a valid move then check its closer to start than we already are.      
            if (Neighbours[i] != LvlRef.GetComponent<PathController>().startPosition)
            {
                if (LvlRef.GetComponent<PathController>().reachableTiles.Contains(Neighbours[i]))
                {
                    if (Neighbours[i].GetComponent<GridScript>().distanceFromStartTile == distanceFromStartTile - 1)
                    {//Then encase there are multiple vaild routes back to start we add it to duplicates so we can randomly pick one of those neighbour tiles once we found them all.
                        dupliacteProtect.Add(Neighbours[i]);
                       // Debug.Log("Not Start Check for duplicates");
                    }
                }            
            }//Else if it is start we found our intended move spot so stop the function.
            else if (Neighbours[i] == LvlRef.GetComponent<PathController>().startPosition)
            {
                LvlRef.GetComponent<PathController>().tilesToCheck.Remove(this.gameObject);
                //LvlRef.GetComponent<PathController>().chosenPathTiles.Add(Neighbours[i]); Cant add start since we don't want to move into that spot just end the search for it.
                LvlRef.GetComponent<PathController>().EstablishPossibleMoves("FindPath");
                return;
            }

        }

        while (dupliacteProtect.Count > 1)
        {
            int removeMe = Random.Range(0, dupliacteProtect.Count);
            dupliacteProtect.RemoveAt(removeMe);
        }
        
        if (dupliacteProtect.Count == 1)
        {
            LvlRef.GetComponent<PathController>().chosenPathTiles.Add(dupliacteProtect[0]);
            LvlRef.GetComponent<PathController>().tilesToCheck.Add(dupliacteProtect[0]);
            LvlRef.GetComponent<PathController>().tilesToCheck.Remove(this.gameObject);
            LvlRef.GetComponent<PathController>().EstablishPossibleMoves("FindPath");
        }
 
    }

    public void MoveCreaturetome()
    {// Use for current quick (Instant move) only. // Might use for abilities which instalty teleport so keep in during rewrite
        if (LvlRef.GetComponent<PathController>().quickMove == true)
        {
            GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenCreatureToken.transform.position = new Vector3(this.transform.position.x, 0.3f, this.transform.position.z);
            GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenCreatureToken.GetComponent<CreatureToken>().declareTile("Move");
            LvlRef.GetComponent<LevelController>().participants[LvlRef.GetComponent<LevelController>().turnPlayerSlot].GetComponent<Player>().moveCrestPoints -= distanceFromStartTile;            
            TileContents = "Creature";
            LvlRef.GetComponent<PathController>().HasMoved();
        }
    }
    #endregion


    #region AOEAbilityCode
    public IEnumerator FindTargetsInDirection(string direction) //0 east / 1 west /2 north 3/ soith
    {
        yield return null;
    }
    #endregion

    public void OnMouseDown()
    {
        if (GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().boardInteraction == "Move")
        {
            if (LvlRef.GetComponent<PathController>().reachableTiles.Contains(this.gameObject) && LvlRef.GetComponent<PathController>().allowedToMove == true)
            {
                if (LvlRef.GetComponent<PathController>().quickMove == false)
                {
                    LvlRef.GetComponent<LevelController>().participants[LvlRef.GetComponent<LevelController>().turnPlayerSlot].GetComponent<Player>().moveCrestPoints -= distanceFromStartTile;
                    LvlRef.GetComponent<PathController>().desiredPosition = this.gameObject;
                    LvlRef.GetComponent<PathController>().tilesToCheck.Clear();
                    LvlRef.GetComponent<PathController>().tilesToCheck.Add(LvlRef.GetComponent<PathController>().desiredPosition);
                    LvlRef.GetComponent<PathController>().EstablishPossibleMoves("FindPath");
                }
                else if (LvlRef.GetComponent<PathController>().quickMove == true)
                {
                    MoveCreaturetome();
                }

                LvlRef.GetComponent<PathController>().allowedToMove = false;
            }
            else
            {
                //Debug.Log("Not Allowed" + gameObject.name + myState + myOwner + TileContents);
            }
        }
        else if (GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().boardInteraction == "AOEPick")
        {
            //Do Nothing Yet assign this game object as the position of our AOE effect.
        }
    }


    public void SetIndicatorMaterial(string whatMaterial)
    {
        myIndicator.gameObject.SetActive(true);
        switch (whatMaterial)
        {
            
            case "MoveSpace":
                myIndicator.GetComponent<MeshRenderer>().material = indicatorMat[0];
                break;
            case "PossibleTarget":
                myIndicator.GetComponent<MeshRenderer>().material = indicatorMat[1];
                break;                     
            case "PickedTarget":
                myIndicator.GetComponent<MeshRenderer>().material = indicatorMat[2];
                break;
            case "AOEMarker":
                myIndicator.GetComponent<MeshRenderer>().material = indicatorMat[3];
                break;
        }
    }

    public void ResetGridTile()
    {
        distanceFromStartTile = 0;
        myTextObject.GetComponent<TextMeshPro>().text = " ";
        myIndicator.gameObject.SetActive(false);
    }

}