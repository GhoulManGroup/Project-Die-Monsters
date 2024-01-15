using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDungeonSpawner : MonoBehaviour
{
    //.From AI Dungeon Lord Check Every Tile that is a valid connection eg adjacent to the dungeon owned by the AI to see if the dungeon spawn patter fits
    //So Find each tile that could connect to the dungeon
    //Store those in a list of game objects then starting from the one with the lowest distance from player
    //Move the dungeon spawner to that position check if it could be placed there then return = Can place dungeon path too true.
    //Then rolle the Ai Dice check if we can summon a creature 
    //If true move dungeon spawener to the saved position // Call spawn dungeon method
    //Spawn creature for Ai opponent
    //clear all lists for next turn
    //Proceed to action phase.
}
