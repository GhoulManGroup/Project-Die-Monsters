using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLordPiece : MonoBehaviour
{
    public string myOwner;
    public DungeonLord myDungeonLord;
    public int Health = 3;

    public void TakeDamage()
    {
        Health -= 1;
        if (Health <= 0)
        {
            // game state over dead.
        }
    }

    public void SetDungeonLordTile()
    {
        // Set the tile bellow the dungeon lord object 
        RaycastHit Down;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Down, 3f))
        {
            Down.collider.GetComponent<GridScript>().myOwner = myOwner;
            Down.collider.GetComponent<GridScript>().myState = "DungeonLord";
            Down.collider.GetComponent<GridScript>().UpdateMaterial();
        }
    }

}
