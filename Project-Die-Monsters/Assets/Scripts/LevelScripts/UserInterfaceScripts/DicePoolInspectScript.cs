using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DicePoolInspectScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int IntFromName = 0;
    // Start is called before the first frame update
    void Start()
    {
        // add function to check object name and convert that into a int to add that object from the list to the inspect window creature tab.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable == true)
        {
            // Possibly combine this function with the creature pool switch statement so this funciton changes the creature int to the last hovered over not pressed.
            string NameToInt = this.gameObject.name.ToString();

            switch (NameToInt)
            {
                case "0":
                    IntFromName = 0;
                    break;
                case "1":
                    IntFromName = 1;
                    break;
                case "2":
                    IntFromName = 2;
                    break;
                case "3":
                    IntFromName = 3;
                    break;
                case "4":
                    IntFromName = 4;
                    break;
            }
            GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().usedFor = "PoolInspect";
            GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().ShowFunction();
            GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().currentCreature = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreaturePoolController>().turnPlayer.GetComponent<Player>().CreaturePool[IntFromName];
            GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().DisplayCreatureDetails();
            GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreaturePoolController>().creaturePick = IntFromName;
        }
        //throw new System.NotImplementedException();      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.GetComponent<Button>().interactable == true)
        {
            //Debug.Log("Test");
            GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>().HideFunction();
        }
        //throw new System.NotImplementedException();      
    }
}
