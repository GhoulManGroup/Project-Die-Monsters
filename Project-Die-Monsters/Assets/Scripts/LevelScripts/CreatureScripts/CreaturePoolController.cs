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
    public bool placingCreature = false;
    // Start is called before the first frame update
    void Start()
    {
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
        if (this.GetComponent<LevelController>().ableToInteractWithBoard == true) // Can Only be pressed with player is in View.
        {
            if (this.GetComponent<LevelController>().turnPlayerPerformingAction == false)
            { 
                this.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                placingCreature = true;
                GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().HideandShow();
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
