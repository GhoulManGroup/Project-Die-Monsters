using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnHoverInspectScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //This script will now display the contents of any object that contains a creature in the inspect UI;
    //Used to hover over dungeon lord to see their health, creature stats on the board, what creature is in the dice pool, what creature is in a dice.?
    private GameObject InspectWindow;

    public void Awake()
    {
        InspectWindow = GameObject.FindGameObjectWithTag("InspectWindow");
    }

    public void OnPointerEnter(PointerEventData eventData) // Used for UI Elements
    {
        if (this.GetComponent<CreaturePoolUISlot>() != null)
        {
            InspectWindow.GetComponent<InspectWindowController>().OpenInspectWindow("PoolInspect");
        }

        else if (this.GetComponent<CreaturePoolUISlot>() == null)
        {
            Debug.Log("Why is this script on this object" + this.gameObject.name);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InspectWindow.GetComponent<InspectWindowController>().CloseInspectWindow();
    }

    public void OnMouseEnter()
    {
        if (this.GetComponent<CreatureToken>() != null)
        {
            Debug.Log("Hello");
            InspectWindow.GetComponent<InspectWindowController>().currentCreature = this.GetComponent<CreatureToken>().myCreature;
            InspectWindow.GetComponent<InspectWindowController>().OpenInspectWindow("PieceInspect");
        }else if (this.GetComponent<DungeonLordPiece>() != null)
        {
            InspectWindow.GetComponent<InspectWindowController>().currentDungeonLord = this.GetComponent<DungeonLordPiece>().myDungeonLord;
            InspectWindow.GetComponent<InspectWindowController>().OpenInspectWindow("PieceInspect");
        }else if (this.GetComponent<CreatureToken>() == null && this.GetComponent<DungeonLordPiece>() == null)
        {
            Debug.Log("Why Am i Here " + gameObject.name);
        }

    }

    public void OnMouseExit()
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
