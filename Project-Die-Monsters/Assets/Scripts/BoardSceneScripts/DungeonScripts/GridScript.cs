using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    [Header("refrences")]
    GameObject LvlRef;

    [Header("Tile States")]
    public string myState = "Empty"; // Dungeon // DungeonLord. Tracks the current form of the tile.
    public string myOwner = "None"; // tracks which participant owns the tile either no one 1 player or an AI.
    public bool turnPlayerDungeonConnection = false; // the tile is next to a tile owned by the turn player to check if it is a valid placement.
    public string TileContents = "Empty"; // Does the tile have a creature piece above it.

    [Header("PathFinding")]
    public int distanceFromStartTile = 0;

    [Header("Tile Visuals")]
    public List<Material> myMat = new List<Material>();

    [Header("PrefabsToSpawn")]
    public GameObject creatureSpawnFab;

    [Header("PieceManagement")]
    public string desiredDir;
    public List<GameObject> Neighbours = new List<GameObject>();


    void Start()
    {
        LvlRef = GameObject.FindGameObjectWithTag("LevelController");
        DeclareNeighbours();
        UpdateMaterial();
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
                                Neighbours.Add(Right.collider.gameObject);
                            }
                        }
                    }
                    break;
            }

        }
    }
    

    public void UpdateMaterial()
    {
        switch (myState)
        {
            case "Empty":
                this.GetComponent<MeshRenderer>().material = myMat[0];
                break;

            case "DungeonTile":
                if(myOwner == "Player0")
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


    public void SearchForMoveSpots()
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
                Debug.Log(Neighbours[i].GetComponent<GridScript>().distanceFromStartTile + 1);
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
                        //Add the neighbour to the list of possible tiles to move to.
                        LvlRef.GetComponent<PathController>().reachableTiles.Add(Neighbours[i].gameObject);                   
                    }  
                }
            }
        }
        //Remove this checked tile from the list of tiles to check add it to the checked list.
        LvlRef.GetComponent<PathController>().tilesToCheck.Remove(this.gameObject);
        LvlRef.GetComponent<PathController>().checkedTiles.Add(this.gameObject);
        LvlRef.GetComponent<PathController>().establishPossibleMoves();
    }

    public void OnMouseDown()
    {
        if (LvlRef.GetComponent<PathController>().reachableTiles.Contains(this.gameObject))
        {
            if (LvlRef.GetComponent<PathController>().quickMove == false)
            {

            }else if(LvlRef.GetComponent<PathController>().quickMove == true)
            {
                MoveCreaturetome();
            }
        }
        //Declare This as our desired move position.
    }

    public void MoveCreaturetome()
    {
        // Quick Hashed Together Quick Move no Anim system :??
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenCreatureToken.transform.position = new Vector3(this.transform.position.x, 0.3f, this.transform.position.z);
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>().ChosenCreatureToken.GetComponent<CreatureToken>().declareTile();
        LvlRef.GetComponent<LevelController>().participants[LvlRef.GetComponent<LevelController>().turnPlayer].GetComponent<Player>().moveCrestPoints -= distanceFromStartTile;
        Debug.Log(LvlRef.GetComponent<LevelController>().participants[LvlRef.GetComponent<LevelController>().turnPlayer].GetComponent<Player>().moveCrestPoints);
        LvlRef.GetComponent<PathController>().startPosition.GetComponent<GridScript>().myState = "Empty";
        TileContents = "Creature";
    }

}
