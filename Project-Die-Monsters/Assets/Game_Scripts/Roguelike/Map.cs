using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Map Perameters")]

    [SerializeField] int mapHeightLimit;

    [SerializeField] int mapWidthLimit;

    [SerializeField] GameObject spawnNode;

    public List<GameObject> encounters = new List<GameObject>();

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
        mapWidthLimit = 1 + runManager.runProgress;
    }

    public void SpawnMapNodes()
    {
        List<int> usedPositons = new List<int>();
        GameObject SpawnedObject = null;

        for (int i = 0; i <= mapHeightLimit; i++)
        {
            if (i == 0)
            {
                // spawn in start spot
                SpawnedObject = Instantiate(spawnNode, this.transform.position, Quaternion.identity);
                SpawnedObject.transform.SetParent(this.gameObject.transform.parent, true);
                SpawnedObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(mapWidthLimit / 2 * 100, i * 100);
                SpawnedObject.name = i.ToString();
            }
            else if (i == mapHeightLimit)
            {
                // spawn 1 in end spot Random.Range(1, this.GetComponent<RunManager>().runProgress + 1);
                SpawnedObject = Instantiate(spawnNode,new Vector3(0f,0f,0f), Quaternion.identity);
                SpawnedObject.transform.SetParent(this.gameObject.transform.parent, true);
                SpawnedObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(mapWidthLimit / 2 * 100, mapHeightLimit * 100);
                SpawnedObject.name = i.ToString();
            }
            else
            {
                for (int j = 0; j < mapWidthLimit; j++)
                {
                    float positionX = Random.Range(0, mapWidthLimit);
                    SpawnedObject = Instantiate(spawnNode, this.transform.position, Quaternion.identity);
                    SpawnedObject.transform.SetParent(this.gameObject.transform.parent, true);
                    SpawnedObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(positionX * 100, i * 100);
                    SpawnedObject.name = i.ToString();
                }
            }
            SpawnedObject.GetComponent<MapEncounter>().myRow = i;
            encounters.Add(SpawnedObject);
        }
    }

        List<GameObject> currentRow = new List<GameObject>();
        List<GameObject> nextRow = new List<GameObject>();
    public void ConnectMapNodes()
    {
        //Chose a node starting from 0
        //Connect to every node.




        for (int i = 0; i < encounters.Count; i++)
        {
            if (i == 0)
            {
                
            }else if (i == mapHeightLimit)
            {

            }
            else
            {

            }
        }
    }

    public void ConnectMe(GameObject connectThis)
    {

    }

    #endregion
}
