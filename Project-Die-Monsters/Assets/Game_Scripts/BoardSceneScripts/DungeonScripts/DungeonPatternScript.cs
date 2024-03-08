using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is attached to the pattern parent which contains 6 game objects arranged in all the diffrent unfolded dice patterns it changes the spawners pattern by moving the tiles
/// to the game object transform.position of the objects in the list.
/// </summary>
public class DungeonPatternScript : MonoBehaviour
{
    public List<GameObject> PatternTilePostion = new List<GameObject>();

    public void ApplyPattern()
    {
        for (int i = 0; i < PatternTilePostion.Count; i++)
        {
            GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().dungeonTiles[i].transform.position = PatternTilePostion[i].transform.position;
        }
    }
}
