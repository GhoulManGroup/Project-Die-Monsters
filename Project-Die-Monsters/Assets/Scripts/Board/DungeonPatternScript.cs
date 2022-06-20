using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPatternScript : MonoBehaviour
{
    public List<GameObject> PatternTilePostion = new List<GameObject>();



    public void Start()
    {
        
    }

    public void ApplyPattern()
    {

        for (int i = 0; i < PatternTilePostion.Count; i++)
        {
            GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().DungeonTiles[i].transform.position = PatternTilePostion[i].transform.position;
        }
    }
}
