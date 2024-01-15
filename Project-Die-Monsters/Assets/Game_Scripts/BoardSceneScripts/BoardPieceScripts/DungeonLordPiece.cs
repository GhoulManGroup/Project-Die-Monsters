using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLordPiece : MonoBehaviour
{
    public DungeonLord myDungeonLord;
    public string myOwner;
    public string myName;
    public int Health = 0;
    //ADD CODE on level controller to have these game objects spawned in at the start like a creature is played.
    public void SetDungeonLordTile()
    {
        // Set the tile bellow the dungeon lord object 
        RaycastHit Down;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Down, 3f))
        {
            Down.collider.GetComponent<GridScript>().myOwner = myOwner;
            Down.collider.GetComponent<GridScript>().myState = "DungeonLord";
            Down.collider.GetComponent<GridScript>().UpdateMaterial();

            if (myOwner == "0")
            {
                GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().PlayerGridTile = Down.collider.gameObject;
            }
        }



        Health = myDungeonLord.dungeonLordHealth;
        myName = myDungeonLord.dungeonLordName;
    }

    public void takeDamage()
    {
        Health -= 1;
        if (Health <= 0)
        {
            GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().EndTurnFunction();
        }
    }

}
