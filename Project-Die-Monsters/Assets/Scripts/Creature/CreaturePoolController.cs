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

    // Update is called once per frame
    void Update()
    {

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
        if (this.GetComponent<CameraController>().ActiveCam == "Alt") // prevents the button from being pressed while in the wrong window.
        {
            if (this.GetComponent<LevelController>().turnPlayerPerformingAction == false)
            { 
                this.GetComponent<LevelController>().turnPlayerPerformingAction = true;
                placingCreature = true;
                GameObject.FindGameObjectWithTag("DungeonSpawner").GetComponent<DungeonSpawner>().HideandShow();
                this.GetComponent<CameraController>().ActiveCam = "Board";
                this.GetComponent<CameraController>().switchCamera();
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
