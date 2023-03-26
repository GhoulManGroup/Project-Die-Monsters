using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreaturePoolController : MonoBehaviour
{
    public GameObject turnPlayer;
    public List<GameObject> creatureDiceBTNS = new List<GameObject>();
    public int creaturePick; // the entry in the list this creature is stored in.
    public Creature currentCreature; // what creature in the pool we want to do stuff with.
    LevelController levelControllerScript;

    void Start()
    {
        levelControllerScript = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
        for (int i = 0; i < creatureDiceBTNS.Count; i++)
        {
            creatureDiceBTNS[i].GetComponent<Button>().interactable = false;
        }
    }

    public void enableButtons() // sets the buttons off then on depending on the length of the creature pool.
    {
        for (int i = 0; i < creatureDiceBTNS.Count; i++)
        {
            creatureDiceBTNS[i].GetComponent<Button>().interactable = false;
        }
        if (turnPlayer.GetComponent<Player>() != null)
        {
            for (int i = 0; i < turnPlayer.GetComponent<Player>().CreaturePool.Count; i++)
            {
                creatureDiceBTNS[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void DeclareCreature(string chosenCreature)
    {
        switch (chosenCreature)
        {
            case "0":
                creaturePick = 0;
                break;
            case "1":
                creaturePick = 1;
                break;
            case "2":
                creaturePick = 2;
                break;
            case "3":
                creaturePick = 3;
                break;
            case "4":
                creaturePick = 4;
                break;
        }

        currentCreature = turnPlayer.GetComponent<Player>().CreaturePool[creaturePick];
    }

    public void SelectMe()
    {
        string whatCreaturePoolPressed = EventSystem.current.currentSelectedGameObject.name.ToString();
        DeclareCreature(whatCreaturePoolPressed);
        if (this.GetComponent<LevelController>().ableToInteractWithBoard == true) //Player isn't currently doing which prevents board piece interaction.
        {
            if (this.GetComponent<LevelController>().turnPlayerPerformingAction == false) //Player isnt already doing an action that prevents placing a creature.
            { 
                this.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                levelControllerScript.placingCreature = true;
                GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().HideandShow();
                levelControllerScript.GetComponent<LevelController>().creaturePlacedFrom = "CreaturePool";
                this.GetComponent<CameraController>().switchCamera("Board");
            }
        }
    }

    public void creaturePlayed()
    {
        turnPlayer.GetComponent<Player>().CreaturePool.RemoveAt(creaturePick);
        currentCreature = null;
        enableButtons();
    }
}
