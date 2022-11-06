using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreaturePoolUISlot : MonoBehaviour
{
    private GameObject InspectWindow;
    private GameObject LevelController;

    public void Awake()
    {
        InspectWindow = GameObject.FindGameObjectWithTag("InspectWindow");
        LevelController = GameObject.FindGameObjectWithTag("LevelController");
    }

    // Does Nothing but exist to be identified.
    public void displayMyContents()
    {
        if (GetComponent<Button>().interactable == true)
        {
            LevelController.GetComponent<CreaturePoolController>().DeclareCreature(this.gameObject.name.ToString());
            InspectWindow.GetComponent<InspectWindowController>().OpenInspectWindow("PoolInspect");
        } else if (GetComponent<Button>().interactable == false)
        {
            // Do nothing
        }
    }
}
