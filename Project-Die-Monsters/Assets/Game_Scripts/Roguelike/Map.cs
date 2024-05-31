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
        }
    }
}
    