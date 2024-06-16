using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;


public class Map : MonoBehaviour
{
    [Header("Map Perameters")]

    [SerializeField] int mapHeightLimit;

    [SerializeField] int mapWidthLimit;

    [SerializeField] GameObject spawnNode;

    private GameObject CurrentNode;

    [System.Serializable]
    public class Floors
    {
        public List<GameObject> floorMapNodes = new List<GameObject>();
    }

    public List<Floors> floorList = new List<Floors>();
    
    RunManager runManager;

    #region Setup

    private void Start()
    {
         runManager = GameObject.FindGameObjectWithTag("RunManager").GetComponent<RunManager>();
         SetDetails();
         SpawnMapNodes();
    }


    public void SetDetails()
    {
        mapHeightLimit = 4 + runManager.runProgress;
        mapWidthLimit = 4 + runManager.runProgress;
    }

  //The slay the spire system is smarter than my dumb idea of trying to randomly spawn nodes and paths in random psoiton rather than its system whihc is more logical and smarter than me
    public void SpawnMapNodes()
    {
        for (int i = 0; i <= mapHeightLimit -1; i++)
        {
            floorList.Add(new Floors());

            for (int j = 0; j < mapWidthLimit -1; j++)
            {
                GameObject currentNode = Instantiate(spawnNode, new Vector3(j, i, 0), Quaternion.identity);
                currentNode.GetComponent<MapEncounter>().myFloor = i;
                currentNode.GetComponent<MapEncounter>().myPostion = j;
                floorList[i].floorMapNodes.Add(currentNode);
            }
        }

        int chosenNode = Random.RandomRange(0, mapWidthLimit - 1);

        CurrentNode = floorList[0].floorMapNodes[chosenNode];
    }


    //Paths must only connect to node 1 down 1 up or above itself, may not go above if another apth is already going up from current node, first node must have two connections minmum all must connect to boss
    public void GeneratePaths()
    {
        
        // Choose nodes in the first floor thats value are within a range of the starting node to be out first connections
        //Have those connections then chose two paths atleast that would not result in both targeting the same node.


        // Find Above MapNodes
       // for(floorList[CurrentNode.])
    }

    public void ConnectNode()
    {
        MapEncounter myComp = CurrentNode.GetComponent<MapEncounter>();
        GameObject Left;
        GameObject Right;
        GameObject Above;

        Above = floorList[myComp.myFloor +1].floorMapNodes[myComp.myPostion];
        if (myComp.myPostion != 0)
        {
            Left = floorList[myComp.myFloor + 1].floorMapNodes[myComp.myPostion -1];
        }
        if (myComp.myPostion != mapWidthLimit)
        {

        }


    }
    //Have a nice visual thing where all the encoutners are dice and they roll which encoutner they are at the start of the level.
    public void AssignEncoutners()
    {

    }

    #endregion

}
