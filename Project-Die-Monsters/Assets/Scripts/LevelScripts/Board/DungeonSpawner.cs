using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    [Header("BoardLists")]
    int currentPattern = 0;
    public List<GameObject> DungeonDicePatterns = new List<GameObject>();
    public List<GameObject> DungeonTiles = new List<GameObject>();
    public List<GameObject> BoardTiles = new List<GameObject>();

    [Header("SpawnStuff")]
    public bool canPlaceDie = false;

    [Header("ResetPosition")]
    Vector3 resetPoint;
    public Vector3 lastPos;

    GameObject lvlRef;

    // Start is called before the first frame update
    void Start()
    {
        resetPoint = this.transform.position;
        UpdateBoard();
        HideandShow();
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayerAction();  
    }

    public void checkPlayerAction()
    {
        if (GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreaturePoolController>().placingCreature == true)
        {
            MoveDungeonSpawner();
            PlaceDungeonPath();
        }
    }

    public void HideandShow()
    {
        if (GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreaturePoolController>().placingCreature == true)
        {
            for (int i = 0; i < DungeonTiles.Count; i++)
            {
                DungeonTiles[i].GetComponent<MeshRenderer>().enabled = true;
                // Run check placement when set active so that the Tile isnt set to can place from the last use.
                CheckPlacement();
            }
        }

        if (GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreaturePoolController>().placingCreature == false)
        {
            for (int i = 0; i < DungeonTiles.Count; i++)
            {
                DungeonTiles[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public void SetPattern() // sets the unfolded dice pattern to use and apply.
    {
        DungeonDicePatterns[currentPattern].GetComponent<DungeonPatternScript>().ApplyPattern();
        CheckPlacement();
    }

    public void MoveDungeonSpawner()
    {
        // Need to Hide ect.
        if (Input.GetKeyDown("w"))
        {
            this.transform.position += transform.position = new Vector3(-1f, 0f, 0f);
            CheckPlacement();
            lastPos = this.transform.position;
        }

        if (Input.GetKeyDown("s"))
        {
            this.transform.position += transform.position = new Vector3(1f, 0f, 0f);
            CheckPlacement();
            lastPos = this.transform.position;
        }

        if (Input.GetKeyDown("a"))
        {
            this.transform.position += transform.position = new Vector3(0f, 0f, -1f);
            CheckPlacement();
            lastPos = this.transform.position;
        }

        if (Input.GetKeyDown("d"))
        {
            this.transform.position += transform.position = new Vector3(0f, 0f, 1f);
            CheckPlacement();
            lastPos = this.transform.position;
        }
        // rotate object
        if (Input.GetKeyDown("e"))
        {
            this.transform.Rotate(0f, 90, 0f);
            CheckPlacement();
        }

        if (Input.GetKeyDown("q"))
        {
            this.transform.Rotate(0f, -90, 0f);
            CheckPlacement();
        }

        if (Input.GetKeyDown("f")) // change pattern of dungeon piece
        {
            if (currentPattern < DungeonDicePatterns.Count)
            {
                currentPattern += 1;
            }

            if (currentPattern == DungeonDicePatterns.Count)
            {
                currentPattern = 0;
            }
            SetPattern();
        }
    }

    public void CheckPlacement()
    {
        int placeableTiles = 0;
        bool spawnableCubeWouldConnectToDungeon = false;
        canPlaceDie = false;

        for (int i = 0; i < DungeonTiles.Count; i++)
        {
            DungeonTiles[i].GetComponent<DungeonTileScript>().CheckPlacement();
        }

        for (int i = 0; i < DungeonTiles.Count; i++)
        {
            if (DungeonTiles[i].GetComponent<DungeonTileScript>().aboveEmptySpace == true)
            {
                placeableTiles += 1;
            }

            if (DungeonTiles[i].GetComponent<DungeonTileScript>().wouldConnectToDungon == true)
            {
                spawnableCubeWouldConnectToDungeon = true;
            }
        }

        if (placeableTiles == 6 && spawnableCubeWouldConnectToDungeon == true)
        {
            canPlaceDie = true;
            placeableTiles = 0;
            spawnableCubeWouldConnectToDungeon = false;
        }

        UpdatePieceMaterials();
    }

    public void PlaceDungeonPath()
    {
        if (Input.GetKeyDown("r")) // press place button. // check tiles are all okay to place // Check that any have connection if so spawn. // Else do not.
        {
            if (canPlaceDie == true)
            {
                for (int i = 0; i < DungeonTiles.Count; i++)
                {
                    DungeonTiles[i].GetComponent<DungeonTileScript>().dungeonToBePlaced();
                }
                GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreaturePoolController>().placingCreature = false;
                GameObject.FindGameObjectWithTag("LevelController").GetComponent<CameraController>().ActiveCam = "Alt";
                GameObject.FindGameObjectWithTag("LevelController").GetComponent<CameraController>().switchCamera();
                HideandShow();
                UpdateBoard();
                this.transform.position = resetPoint;
            }
        }

    }

    public void UpdatePieceMaterials()
    {
        for (int i = 0; i < DungeonTiles.Count; i++)
        {
            DungeonTiles[i].GetComponent<DungeonTileScript>().UpdateMaterial();
        }
    }

    public void UpdateBoard()
    {
        // Runs a for loop and tells every tile in the grid 
        for (int i = 0; i < BoardTiles.Count; i++)
        {
            //BoardTiles[i].GetComponent<GridScript>().CheckForDungeonConnection();
            BoardTiles[i].GetComponent<GridScript>().turnPlayerDungeonConnection = false;
        }
    }
}
