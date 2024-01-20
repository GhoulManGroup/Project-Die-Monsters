using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnerTileScript : MonoBehaviour
{
    public bool amActive = true;
    public bool aboveEmptySpace = true;
    public bool wouldConnectToDungon = false; // checks if being placed here would connect this tile to dungeon.
    public bool amSpawnTile; // this bool simply identifies which of the six dungeon tiles needs to tell the bellow board tile to spawn a creature rather than adding in calls ect.

    public List<Material> myMat = new List<Material>();
    DungeonSpawner DungeonSpawner;

    // Start is called before the first frame update
    void Start()
    {
        DungeonSpawner = GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>();
        UpdateMaterial();
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
            // reset dungeonSpawner position rotation / pattern to last acceptable one to prevent the infinte loop.
            if (lastInput == "Move")
            {
                DungeonSpawner.transform.position = DungeonSpawner.lastPos;
            }else if (lastInput == "Rotate")
            {
            DungeonSpawner.transform.rotation = DungeonSpawner.lastRotation;
            }else if (lastInput == "PatternChange")
            {
            DungeonSpawner.dungeonDicePatterns[DungeonSpawner.lastPattern].GetComponent<DungeonPatternScript>().ApplyPattern();
            }
            DungeonSpawner.CheckPlacement(lastInput);
        }
    }

    public void AICheckPlacement()
    {
        aboveEmptySpace = false;

        RaycastHit Bellow;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Bellow, 5f))
        {
            if (Bellow.collider.gameObject != null)
            {
                Debug.LogWarning(Bellow.collider.gameObject.name);
                if (Bellow.collider.GetComponent<GridScript>().myState == "Empty")
                {
                    aboveEmptySpace = true;
                }
                else
                {
                    Debug.LogWarning("Out of Bounds AI ");
                    aboveEmptySpace = false;
                }
            }
            else
            {
                Debug.Log("Cry");
            }
        }
    }

   public IEnumerator DimensionTheDice(float Yrotation)
    {
        RaycastHit Bellow;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Bellow, 5f))
        {
            // If this tile out of the 6 is the designated spawn tile instanciate a creature token object above the board position bellow this tile.

            Bellow.collider.GetComponent<GridScript>().spawnMe(0, Yrotation);

            while (Bellow.collider.GetComponent<GridScript>().fabAnimationDone == false)
            {
                yield return null;
            }
            GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().waitForPath = false;
        }
   }

   public IEnumerator ApplyPathToBoard()
    {
        // Change the material colour of the tile bellow this object to that of the current players path and change its update its states
        RaycastHit Bellow;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Bellow, 5f))
        {

            switch (GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().currentTurnParticipant.ToString())
            {
                case "0":
                    Bellow.collider.GetComponent<GridScript>().myState = "DungeonTile";
                    Bellow.collider.GetComponent<GridScript>().myOwner = "0";
                    break;
                case "1":
                    Bellow.collider.GetComponent<GridScript>().myState = "DungeonTile";
                    Bellow.collider.GetComponent<GridScript>().myOwner = "1";
                    break;
            }

            Bellow.collider.GetComponent<GridScript>().UpdateMaterial();
        }

        yield return null;
    }

}
