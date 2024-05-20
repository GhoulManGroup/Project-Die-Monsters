using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Map Perameters")]

    [SerializeField] int mapHeightLimit;

    [SerializeField] int mapWidthLimit;

    [SerializeField] GameObject spawnNode;
    public List<Encounter> encounters = new List<Encounter>();

    RunManager runManager;

    private void Start()
    {
        runManager = GameObject.FindGameObjectWithTag("RunManager").GetComponent<RunManager>() ;
        SetDetails();
        SpawnMap();
    }

    public void SetDetails()
    {
        mapHeightLimit = 4 + runManager.runProgress;
        mapWidthLimit = 1 + runManager.runProgress;
    }

    public void SpawnMap()
    {
        List<int> usedPositons = new List<int>();

        for (int i = 0; i < mapHeightLimit; i++)
        {
            if (i == 0)
            {
                // spawn in start spot
                Instantiate(spawnNode, new Vector3(mapWidthLimit / 2, i, 0), Quaternion.identity);
            }
            else if (i == mapHeightLimit)
            {
                // spawn 1 in end spot Random.Range(1, this.GetComponent<RunManager>().runProgress + 1);
                Instantiate(spawnNode, new Vector3(mapWidthLimit / 2, mapHeightLimit, 0), Quaternion.identity);
            }
            else
            {
                for (int j = 0; j < mapWidthLimit; j++)
                {
                    Instantiate(spawnNode, new Vector3(i, mapHeightLimit, 0), Quaternion.identity);
                }
            }


        }
    }
}
    