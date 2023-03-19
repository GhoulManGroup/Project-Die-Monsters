using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    [Header("Game Location & Other Settings")]

    public string gameLocation = "MainMenu";
    public string gameMode; // check to see what mode of play we are in so we know which player constraint to apply eg normal game rules or rougelike session modified ones.
    public string desiredOpponent; // this varible stores what opponent the player will face based on the options they pick in the menu so the level controler knows what settings to intialize.

    public void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

}
