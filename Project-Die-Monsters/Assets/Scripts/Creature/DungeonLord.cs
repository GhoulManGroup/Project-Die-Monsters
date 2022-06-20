using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLord : MonoBehaviour
{
    public string myOwner;
    public int Health = 3;

    public void TakeDamage()
    {
        Health -= 1;
        if (Health <= 0)
        {
            // game state over dead.
        }
    }

}
