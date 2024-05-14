using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Map Perameters")]

    [SerializeField] int mapHeightLimit;

    [SerializeField] int mapWidthLimit;

    [SerializeField] int rowWidth;

    [SerializeField] GameObject spawnNode;
    public List<Encounter> encounters = new List<Encounter>();

    public void SetDetails()
    {
        mapHeightLimit = 4 + this.GetComponent<RunManager>().runProgress;
        //mapWidthLimit = 1 + this.get
    }

    public void SpawnMap()
    {
        for (int i = 0; i < mapHeightLimit; i++)
        {
            if (i == 0)
            {
                // spawn in start spot
            }
            else if (i == mapHeightLimit)
            {
                // spawn 1 in end spot Random.Range(1, this.GetComponent<RunManager>().runProgress + 1);
            }
            else
            {
                // spawn multiple nodes = m
            }


        }
    }
}
    