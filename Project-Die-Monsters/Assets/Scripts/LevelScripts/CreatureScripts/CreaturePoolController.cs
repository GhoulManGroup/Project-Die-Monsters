using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreaturePoolController : MonoBehaviour
{
    public GameObject turnPlayer;
    public List<GameObject> creatureDiceBTNS = new List<GameObject>();
    public int creaturePick;
    LevelController levelControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        levelControllerScript = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
        for (int i = 0; i < creatureDiceBTNS.Count; i++)
        {
            creatureDiceBTNS[i].GetComponent<Button>().interactable = false;
        }
    }

    public void enableButtons() // sets the buttons 
    {
        for (int i = 0; i < creatureDiceBTNS.Count; i++)
        {
            creatureDiceBTNS[i].GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < turnPlayer.GetComponent<Player>().CreaturePool.Count; i++)
        {
            creatureDiceBTNS[i].GetComponent<Button>().interactable = true;
        }
    }

    public void SelectMe()
    {
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
        this.GetComponent<LevelController>().turnPlayerPerformingAction = false;
        enableButtons();
    }
}
