using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEncounter : MonoBehaviour
{
    public List<GameObject> myConnections = new List<GameObject>();

    bool isActive = false;

    public Encounter myEncounter;

    public int myFloor;
    public int myPostion;


    public List<Sprite> encounterIcon = new List<Sprite>();

    public void Start()
    {
        //Set sprites here among other setups
    }

    public void ChosenNode()
    {

    }

    public void EnableConnections()
    {

    }

}
