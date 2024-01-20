using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class AIDungeonSpawner : MonoBehaviour
{// This class oversees the AI opponents deciding on if and where it can place the dungeon.

    //.From AI Dungeon Lord Check Every Tile that is a valid connection eg adjacent to the dungeon owned by the AI to see if the dungeon spawn patter fits
    //So Find each tile that could connect to the dungeon
    //Store those in a list of game objects then starting from the one with the lowest distance from player
    //Move the dungeon spawner to that position check if it could be placed there then return = Can place dungeon path too true.
    //Then rolle the Ai Dice check if we can summon a creature 
    //If true move dungeon spawener to the saved position // Call spawn dungeon method
    //Spawn creature for Ai opponent
    //clear all lists for next turn
    //Proceed to action phase.


    [Header("AIDungeonSpawner")]
    [HideInInspector]
    public GameObject PlayerGridTile;
    [HideInInspector]
    public GameObject MyStartTile;

    public List<GameObject> tilesToCheck = new List<GameObject>();
    public List<GameObject> checkedTiles = new List<GameObject>();
    public List<GameObject> spawnPointsToCheck= new List<GameObject>();

    [Header("Spawn Patterns")]
    public List<GameObject> spawnPattern = new List<GameObject>();
    public bool CanPlaceHere;

    #region AI Dungeon Spawner

    //Set up the tiles distance from player avatar so it knows which tiles are closer to the win condition to prioritise
    public void DungeonSizeSetup()
    {
        Debug.Log(PlayerGridTile.name + "Inside Map Dungeon Size");

        if (PlayerGridTile == null)
        {
            Debug.Log("Object Null Make Change to Fix");
        }

        tilesToCheck.Add(PlayerGridTile);

        StartCoroutine(AITileCheck("MapDungeonSize"));
    }

    public IEnumerator AITileCheck(string UseFor)
    {// Method used to check through the board grid for the AI to find out things like size of dungeon or which tiles it could possibly expand the dungeon from.

        if (tilesToCheck.Count != 0)
        {
            if (UseFor == "MapDungeonSize")
            {
                tilesToCheck[0].GetComponent<GridScript>().MapDungeonSizeForAI();
            }
            else if (UseFor == "CheckSpawnLocations")
            {
                tilesToCheck[0].GetComponent<GridScript>().CheckForDungeonExpansion();
            }

            if (tilesToCheck.Count != 0)
            {
                StartCoroutine(AITileCheck(UseFor));
            }
            else if (tilesToCheck.Count == 0)
            {
                checkedTiles.Clear();
            }
        }
        yield return null;
    }

    GameObject checkHere;
    List<string> WaitCheck = new List<string>();
    int rotationAttempts = 1;

    [Header("Current Stored Dungeon Expansion Data")]
    [SerializeField]
    GameObject tileCanSpawnFrom;
    [SerializeField]
    GameObject patternToSpawnFrom;
    [SerializeField]
    float transformToSpawnFrom;

    //This corutine cycle through every game tile which is next to the AI dungeon and empty as these are possible points to expand the dungeon.
    //It does this by first finding the tile closest to the AI 
    public IEnumerator CanAIDungeonExpand()
    {
        Debug.Log("Starting Check Of Point " + spawnPointsToCheck.Count );

        //Check all spawn points found by AItileCheck
        if (spawnPointsToCheck.Count != 0)
        {
            checkHere = spawnPointsToCheck[0];

            for (int i = 0; i < spawnPointsToCheck.Count; i++)
            {
                if (spawnPointsToCheck[i].GetComponent<GridScript>().distanceFromPlayerDungeonLord < checkHere.GetComponent<GridScript>().distanceFromPlayerDungeonLord)
                {
                    checkHere = spawnPointsToCheck[i];
                    Debug.LogError("Replaced Spawn Point for Closer Point Tile");
                }
            }

            StartCoroutine("CheckIfCanPlaceHere");

            while (WaitCheck.Contains("CICP"));
            {
                Debug.LogError("Inside While Loop CICP");
                yield return null;
            }

            if (CanPlaceHere == true)
            {
                tileCanSpawnFrom = checkHere;
                GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>().PhaseDone = true;
            }

            else
            {
                spawnPointsToCheck.Remove(checkHere);
                StartCoroutine("CanAIDungeonExpand");
                //contiune the check till all spawn points are done.
            }
            //GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>().PhaseDone = true;
        }
        else
        {
            Debug.Log("Cant Find a tile to expand from cant place dungeon");
            GameObject.FindGameObjectWithTag("AIController").GetComponent<AIManager>().PhaseDone = true;
        }

        yield return null;
    }

    public IEnumerator CheckIfCanPlaceHere()
    {

        if (!WaitCheck.Contains("CICP"))
        {
            WaitCheck.Add("CICP");
        }

        //Position us to the current possible spawn location
        this.transform.position = new Vector3 (checkHere.transform.position.x, this.transform.position.y, checkHere.transform.position.z);

        for (int i = 0; i < spawnPattern.Count; i++)
        {
            //Loop through each possible style of pattern on the board then call check dungeon pattern to see if that position / pattern results in a valid placement.
            spawnPattern[i].GetComponent<DungeonPatternScript>().ApplyPattern();

            StartCoroutine("CheckDungeonPattern");

            while (WaitCheck.Contains("CDP"))
            {
                Debug.LogError("Inside While Loop CDP");
                yield return null;
            }

            if (CanPlaceHere == true)
            {
                Debug.Log("Check Done Pattern / Location Found");
                patternToSpawnFrom = spawnPattern[i].gameObject;
                WaitCheck.Remove("CICP");
                break;
            }
            else
            {
                rotationAttempts = 1;
                Debug.Log("Swapping Pattern");
            }
        }

        yield return null;
    }

    public IEnumerator CheckDungeonPattern()
    {
        //Check if current pattern is above 6 empty tiles
        //If not rotate 3 times to see if that pattern/positon works when on either of the three other rotations.


        if (!WaitCheck.Contains("CDP"))
        {
            WaitCheck.Add("CDP");
        }

        CanPlaceHere = false;

        int tilesCanSpawn = 0;

        for (int j = 0; j < this.GetComponent<DungeonSpawner>().dungeonTiles.Count; j++)
        {
            this.GetComponent<DungeonSpawner>().dungeonTiles[j].GetComponent<SpawnerTileScript>().AICheckPlacement();

            if (this.GetComponent<DungeonSpawner>().dungeonTiles[j].GetComponent<SpawnerTileScript>().aboveEmptySpace == true)
            {
                tilesCanSpawn += 1;
            }
        }

        if (tilesCanSpawn == 6)
        {
            CanPlaceHere = true;
            transformToSpawnFrom = this.transform.rotation.y;
            WaitCheck.Remove("CDP");
        }

        if (tilesCanSpawn != 6)
        {
            if (rotationAttempts < 4)
            {
                Debug.Log(rotationAttempts);

                this.transform.Rotate(0f, 90f, 0f);

                rotationAttempts += 1;

               StartCoroutine("CheckDungeonPattern");
            }
            else if (rotationAttempts >= 4)
            {
                Debug.LogError("Pattern Resulted in No Sucess");
                WaitCheck.Remove("CDP");
            }
        }
        yield return null;
    }

    public void ResetSpawner()
    {
        tilesToCheck.Clear();
        checkedTiles.Clear();
        spawnPointsToCheck.Clear();
        WaitCheck.Clear();
        rotationAttempts = 1;
        checkHere = null;
    }

    IEnumerator SpawnCreatureAI()
    {
       // this.transform.position = 

        yield return null;
    }

    #endregion

    }
