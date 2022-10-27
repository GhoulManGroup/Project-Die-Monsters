using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTileScript : MonoBehaviour
{
    public bool amActive = true;
    public bool aboveEmptySpace = true;
    public bool wouldConnectToDungon = false; // checks if being placed here would connect this tile to dungeon.
    public bool amSpawnTile; // this bool simply identifies which of the six dungeon tiles needs to tell the bellow board tile to spawn a creature rather than adding in calls ect.

    public List<Material> myMat = new List<Material>();
    // Start is called before the first frame update
    void Start()
    {
        UpdateMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMaterial()
    {
        if (aboveEmptySpace == false)
        {
            this.GetComponent<MeshRenderer>().material = myMat[0];
        }

        if (aboveEmptySpace == true)
        {
            if (GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().canPlaceDie == false)
            {
                this.GetComponent<MeshRenderer>().material = myMat[1];
            }
            if (GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().canPlaceDie == true)
            {
                this.GetComponent<MeshRenderer>().material = myMat[2];
            }

        }
    }

    public void CheckPlacement(string lastInput)
    {
        aboveEmptySpace = false;

        RaycastHit Bellow;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Bellow, 5f))
        {
            if (Bellow.collider != null) // if we collide with somthing
            {
                if (Bellow.collider.GetComponent<GridScript>().myState == "Empty") // check that the tile is empty.
                {
                    // When tile is empty have it check its neighbours.
                    Bellow.collider.GetComponent<GridScript>().CheckForDungeonConnection();
                    aboveEmptySpace = true;

                    if (Bellow.collider.GetComponent<GridScript>().turnPlayerDungeonConnection == true) // then check if that tile would connect us to the existing dungon.
                    {
                        wouldConnectToDungon = true;
                    }

                    if (Bellow.collider.GetComponent<GridScript>().turnPlayerDungeonConnection == false)
                    {
                        wouldConnectToDungon = false;
                    }
                }

                if (Bellow.collider.GetComponent<GridScript>().myState == "DungeonTile")
                {
                    aboveEmptySpace = false;
                    wouldConnectToDungon = false;
                }

                if (Bellow.collider.GetComponent<GridScript>().myState == "DungeonLord")
                {
                    aboveEmptySpace = false;
                    wouldConnectToDungon = false;
                }
            }

      
        }else
        {
            GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().transform.position = GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().lastPos;
            CheckPlacement(lastInput);
        }
    }

    public void dungeonToBePlaced()
    {
        RaycastHit Bellow;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Bellow, 5f))
        {
            switch (GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().whoseTurn)
            {
                case "Player0":
                    Bellow.collider.GetComponent<GridScript>().myState = "DungeonTile";
                    Bellow.collider.GetComponent<GridScript>().myOwner = "Player0";
                    break;
                case "Player1":
                    Bellow.collider.GetComponent<GridScript>().myState = "DungeonTile";
                    Bellow.collider.GetComponent<GridScript>().myOwner = "Player1";
                    break;
            }

            Bellow.collider.GetComponent<GridScript>().UpdateMaterial();

            if (amSpawnTile == true)
            {
                Bellow.collider.GetComponent<GridScript>().SpawnCreatureAbove();
            }
            
        }      
    }

}
