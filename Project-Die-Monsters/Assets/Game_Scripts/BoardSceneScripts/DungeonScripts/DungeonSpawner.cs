using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The dungeon spawner is the parent object & manager of our dungeon spawner system and all assosiated game objects.
/// </summary>
public class DungeonSpawner : MonoBehaviour
{

    [Header("BoardLists")]
    int currentPattern = 0;
    public List<GameObject> dungeonDicePatterns = new List<GameObject>();
    public List<GameObject> dungeonTiles = new List<GameObject>();
    public List<GameObject> boardTiles = new List<GameObject>();

    [Header("SpawnStuff")]
    public bool canPlaceDie = false;
    [HideInInspector]
    public float SpawnerYRotation = 0;

    [Header("ResetPosition")]
    Vector3 resetPoint; // This the board position the spawner moves to everytime the player would move summon a creature.
    public Vector3 lastPos; // The last position the spawner occupied without being out of bounds.
    public Quaternion lastRotation; // The last the spawner had without being out of bounds.
    public int lastPattern; // The last pattern that was not out of bounds.

    GameObject lvlRef;

    [SerializeField]
    GameObject ControlSchemeUI; //UI Image showing user control scheme for moving the Spawner Object

    void Start()
    {
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
        resetPoint = this.transform.position;
        SpawnerYRotation = this.transform.localRotation.y;
        UpdateBoard();
        HideandShow();
    }

    void Update()
    {
        checkPlayerAction();  
    }

    #region Player Dungeon Spawning
    public void checkPlayerAction()
    {
        if (lvlRef.GetComponent<LevelController>().placingCreature == true)
        {
            MoveDungeonSpawner();
            PlaceDungeonPath();
        }
    }

    public void HideandShow()
    {
        if (lvlRef.GetComponent<LevelController>().placingCreature == true) // Next Condition to Change.
        {
            for (int i = 0; i < dungeonTiles.Count; i++)
            {
                dungeonTiles[i].GetComponent<MeshRenderer>().enabled = true;
                CheckPlacement("None");
            }
        }

        if (lvlRef.GetComponent<LevelController>().placingCreature == false)
        {
            for (int i = 0; i < dungeonTiles.Count; i++)
            {
                dungeonTiles[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public void MoveDungeonSpawner()
    {

        if (Input.GetKeyDown("w"))
        {
            this.transform.position += transform.position = new Vector3(-1f, 0f, 0f);
            CheckPlacement("Move");
            lastPos = this.transform.position;
        }

        if (Input.GetKeyDown("s"))
        {
            this.transform.position += transform.position = new Vector3(1f, 0f, 0f);
            CheckPlacement("Move");
            lastPos = this.transform.position;
        }

        if (Input.GetKeyDown("a"))
        {
            this.transform.position += transform.position = new Vector3(0f, 0f, -1f);
            CheckPlacement("Move");
            lastPos = this.transform.position;
        }

        if (Input.GetKeyDown("d"))
        {
            this.transform.position += transform.position = new Vector3(0f, 0f, 1f);
            CheckPlacement("Move");
            lastPos = this.transform.position;
        }

        if (Input.GetKeyDown("e"))
        {
            this.transform.Rotate(0f, 90, 0f);
            CheckPlacement("Rotate");
            lastRotation = this.transform.rotation;
        }

        if (Input.GetKeyDown("q"))
        {
            this.transform.Rotate(0f, -90, 0f);
            CheckPlacement("Rotate");
            lastRotation = this.transform.rotation;
        }

        if (Input.GetKeyDown("f"))
        {
            lastPattern = currentPattern;

            if (currentPattern < dungeonDicePatterns.Count)
            {
                currentPattern += 1;
            }

            if (currentPattern == dungeonDicePatterns.Count)
            {
                currentPattern = 0;
            }

            //Tell the spawner tiles to arrange themselves in the next pattern stored in the list then check if it is valid.
            dungeonDicePatterns[currentPattern].GetComponent<DungeonPatternScript>().ApplyPattern();
            CheckPlacement("PatternChange");
        }

        SpawnerYRotation = this.transform.rotation.y;
    }

    public void CheckPlacement(string lastInput)
    {
        int placeableTiles = 0;
        bool spawnableCubeWouldConnectToDungeon = false;
        canPlaceDie = false;

        for (int i = 0; i < dungeonTiles.Count; i++)
        {
            dungeonTiles[i].GetComponent<SpawnerTileScript>().CheckPlacement(lastInput);
        }

        for (int i = 0; i < dungeonTiles.Count; i++)
        {
            if (dungeonTiles[i].GetComponent<SpawnerTileScript>().aboveEmptySpace == true)
            {
                placeableTiles += 1;
            }

            if (dungeonTiles[i].GetComponent<SpawnerTileScript>().wouldConnectToDungon == true)
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

    bool placingDungeon = false;
    public void PlaceDungeonPath()
    { // Press (R) in order to place the current dungeon path displayed by the dungeon spawner onto the board.
        if (Input.GetKeyDown("r")) 
        {
            if (canPlaceDie == true && placingDungeon == false)
            {
                placingDungeon = true;
                StartCoroutine(SpawnDungeonPath());
            }
        }
    }

    //Place the 3D model down first wait untill the animation is done then change the tiles bellow the spawner to match the turn players path colour.
    [HideInInspector]
    public bool waitForPath = true;

    IEnumerator SpawnDungeonPath()
    {
        for (int i = 0; i < dungeonTiles.Count; i++)
        {
            if (dungeonTiles[i].GetComponent<SpawnerTileScript>().amSpawnTile == true)
            {
                dungeonTiles[i].GetComponent<SpawnerTileScript>().StartCoroutine("DimensionTheDice", this.transform.localEulerAngles.y);
            }
        }

        while (waitForPath == true)
        {
            yield return null;
        }

        for (int i = 0; i < dungeonTiles.Count; i++)
        {
            dungeonTiles[i].GetComponent<SpawnerTileScript>().StartCoroutine("ApplyPathToBoard", this.transform.localEulerAngles.y);
        }

        lvlRef.GetComponent<LevelController>().placingCreature = false;
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<CameraController>().switchCamera("Alt");
        HideandShow();
        UpdateBoard();
        waitForPath = true;
        this.transform.position = resetPoint;
        placingDungeon = false;
        yield return null;

    }

    #endregion

    public void UpdatePieceMaterials()
    {
        for (int i = 0; i < dungeonTiles.Count; i++)
        {
            dungeonTiles[i].GetComponent<SpawnerTileScript>().UpdateMaterial();
        }
    }

    public void UpdateBoard()
    {
        for (int i = 0; i < boardTiles.Count; i++)
        {
            boardTiles[i].GetComponent<GridScript>().turnPlayerDungeonConnection = false;
        }
    }
}
