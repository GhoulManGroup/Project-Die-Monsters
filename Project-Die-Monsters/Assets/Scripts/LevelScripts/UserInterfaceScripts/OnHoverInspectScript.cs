using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnHoverInspectScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //This script will now display the contents of any object that contains a creature in the inspect UI;
    //Used to hover over dungeon lord to see their health, creature stats on the board, what creature is in the dice/Creature pool , what creature is in a dice.?
    private GameObject InspectWindow;
    private GameObject LevelController;

    public void Awake()
    {
        InspectWindow = GameObject.FindGameObjectWithTag("InspectWindow");
        LevelController = GameObject.FindGameObjectWithTag("LevelController");
    }

    public void OnPointerEnter(PointerEventData eventData) // Used for UI Elements
    {
        if (LevelController.GetComponent<CreatureController>().ChosenAction == "None")
        {
            if (this.GetComponent<CreaturePoolUISlot>() != null)
            {
                this.GetComponent<CreaturePoolUISlot>().displayMyContents();
            }

            else if (this.GetComponent<CreaturePoolUISlot>() == null)
            {
                Debug.Log("Why is this script on this object" + this.gameObject.name);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (LevelController.GetComponent<CreatureController>().ChosenAction == "None")
        {
            InspectWindow.GetComponent<InspectWindowController>().CloseInspectWindow();
        }
    }

    public void OnMouseEnter()
    {
        if (LevelController.GetComponent<CreatureController>().ChosenAction == "None")
        {
            if (this.GetComponent<CreatureToken>() != null)
            {
                InspectWindow.GetComponent<InspectWindowController>().currentCreature = this.GetComponent<CreatureToken>().myCreature;
                InspectWindow.GetComponent<InspectWindowController>().currentCreaturePiece = this.gameObject;
                InspectWindow.GetComponent<InspectWindowController>().OpenInspectWindow("PieceInspect");
            }
            else if (this.GetComponent<DungeonLordPiece>() != null)
            {
                InspectWindow.GetComponent<InspectWindowController>().currentDungeonLordPiece = this.gameObject;
                InspectWindow.GetComponent<InspectWindowController>().OpenInspectWindow("DungeonLordInspect");
            }
            else if (this.GetComponent<CreatureToken>() == null && this.GetComponent<DungeonLordPiece>() == null)
            {
                Debug.Log("Why Am i Here " + gameObject.name);
            }

            // Add Item eventually
        }

    }

    public void OnMouseExit()
    {
        if (LevelController.GetComponent<CreatureController>().ChosenAction == "None")
        {
            if (this.GetComponent<CreatureToken>() != null)
            {
                InspectWindow.GetComponent<InspectWindowController>().CloseInspectWindow();
            }
            else if (this.GetComponent<DungeonLordPiece>() != null)
            {
                InspectWindow.GetComponent<InspectWindowController>().CloseInspectWindow();
            }
            else if (this.GetComponent<CreatureToken>() == null && this.GetComponent<DungeonLordPiece>() == null)
            {
                Debug.Log("Why Am i Here " + gameObject.name);
            }
        }
    }



}
