using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Map Perameters")]

    [SerializeField] int mapHeightLimit;

    [SerializeField] int mapWidthLimit;

    [SerializeField] int rowWidth;
                

    public void SetDetails()
    {
        mapHeightLimit = 4 + this.GetComponent<RunManager>().runProgress;
        mapWidthLimit = Random.Range(1, this.GetComponent<RunManager>().runProgress + 1);
    }
}
    