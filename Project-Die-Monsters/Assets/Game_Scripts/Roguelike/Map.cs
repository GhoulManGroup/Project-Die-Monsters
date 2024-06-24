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

    [SerializeField] GameObject CurrentNode;

    [SerializeField] GameObject BossRoom;

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
                currentNode.transform.SetParent(this.gameObject.transform.parent, true);
                currentNode.GetComponent<RectTransform>().anchoredPosition = new Vector2(j * 175, i * 175);
                currentNode.name = i.ToString() + " " + j.ToString();
                currentNode.GetComponent<MapEncounter>().myFloor = i;
                currentNode.GetComponent<MapEncounter>().myPostion = j +1;
                floorList[i].floorMapNodes.Add(currentNode);
            }
        }

        int chosenNode = Random.RandomRange(0, mapWidthLimit - 1);

        CurrentNode = floorList[0].floorMapNodes[chosenNode];

        GeneratePaths();
    }


    //Paths must only connect to node 1 down 1 up or above itself, may not go above if another apth is already going up from current node, first node must have two connections minmum all must connect to boss
    public void GeneratePaths()
    {
        StartCoroutine(ConnectNode(3));

        // Choose nodes in the first floor thats value are within a range of the starting node to be out first connections
        //Have those connections then chose two paths atleast that would not result in both targeting the same node.


        // Find Above MapNodes
       // for(floorList[CurrentNode.])
    }


    [SerializeField] GameObject Left;
    [SerializeField] GameObject Right;
    [SerializeField] GameObject Above;

    public IEnumerator ConnectNode(int connectionsToMake)
    {
        MapEncounter myComp = CurrentNode.GetComponent<MapEncounter>();

        if (myComp.myFloor != mapHeightLimit - 1)
        {
            Above = floorList[myComp.myFloor + 1].floorMapNodes[myComp.myPostion - 1];

            if (myComp.myPostion != 0)
            {
                Left = floorList[myComp.myFloor + 1].floorMapNodes[myComp.myPostion - 2];
            }
            if (myComp.myPostion != mapWidthLimit - 1)
            {
                Right = floorList[myComp.myFloor + 1].floorMapNodes[myComp.myPostion];
            }
        }
        else if (myComp.myFloor == mapHeightLimit - 1)
        {
            //Connect only to the boss room
        }

        while (myComp.myConnections.Count < connectionsToMake)
        {
            yield return null;

            int pickNode = Random.RandomRange(1, 4);

            Debug.Log(pickNode);

            if (pickNode == 1 && Right != null && !myComp.myConnections.Contains(Right))
            {
                myComp.myConnections.Add(Right);
                Right.GetComponent<MapEncounter>().connectionsToMake += 1;            }

            else if (pickNode == 2 && Above != null && !myComp.myConnections.Contains(Above))
            {
                myComp.myConnections.Add(Above);
                Above.GetComponent<MapEncounter>().connectionsToMake += 1;
            }

            else if (pickNode == 3 && Left != null && !myComp.myConnections.Contains(Left))
            {
                myComp.myConnections.Add(Left);
                Left.GetComponent<MapEncounter>().connectionsToMake += 1;
            }
        }

    }

    //Have a nice visual thing where all the encoutners are dice and they roll which encoutner they are at the start of the level.
    public void AssignEncoutners()
    {

    }

    #endregion

}
