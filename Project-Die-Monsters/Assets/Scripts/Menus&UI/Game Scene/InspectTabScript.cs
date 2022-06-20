using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectTabScript : MonoBehaviour
{

    GameObject lvlRef;

    public List<GameObject> myComponents = new List<GameObject>();
    public Creature CurrentCreature;

    public GameObject creatureArt;
    public GameObject creatureLevel;
    public GameObject creatureTribe;
    public GameObject creatureType;
    public Text creatureName;
    public Text attackValue;
    public Text defenceValue;
    public Text healthValue;

    // Start is called before the first frame update
    void Start()
    {
        HideFunction();
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
    }

    public void setDetails()
    {
        creatureArt.GetComponent<Image>().sprite = CurrentCreature.CardArt;
        creatureLevel.GetComponent<Image>().sprite = CurrentCreature.LevelSprite;
        creatureTribe.GetComponent<Image>().sprite = CurrentCreature.TribeSprite;
        creatureType.GetComponent<Image>().sprite = CurrentCreature.TypeSprite;
        creatureName.GetComponent<Text>().text = CurrentCreature.creatureType.ToString();
        attackValue.GetComponent<Text>().text = "ATK" + CurrentCreature.Attack.ToString();
        defenceValue.GetComponent<Text>().text = "DEF" + CurrentCreature.Defence.ToString();
        healthValue.GetComponent<Text>().text = "HP" + CurrentCreature.Health.ToString();
    }

    public void HideFunction()
    {
        for (int i = 0; i < myComponents.Count; i++)
        {
            if (myComponents[i].GetComponent<Image>() != null)
            {
                myComponents[i].GetComponent<Image>().enabled = false;
            }

            if (myComponents[i].GetComponent<Text>() != null)
            {
                myComponents[i].GetComponent<Text>().enabled = false;
            }
        }
    }

    public void ShowFunction()
    {
        // add check for state to choosecreature  button if not in dice window;
        for (int i = 0; i < myComponents.Count; i++)
        {
            if (myComponents[i].GetComponent<Image>() != null)
            {
                myComponents[i].GetComponent<Image>().enabled = true;
            }

            if (myComponents[i].GetComponent<Text>() != null)
            {
                myComponents[i].GetComponent<Text>().enabled = true;
            }
        }
    }

    public void ChooseThisCreature()
    {
        if (lvlRef.GetComponent<ResourceUIManager>().whatUItoShow == "Dice")
        {
            lvlRef.GetComponent<ResourceUIManager>().DiceWindowButtonControl();

            // hide the 3 dice sprites.
            lvlRef.GetComponent<DiceHandManager>().desiredDiceState = "Hidden";
            lvlRef.GetComponent<DiceHandManager>().diceShow();
            lvlRef.GetComponent<DiceHandManager>().CreatureAddedToPool();

            // hide the inspect tab
            HideFunction();

            // switch to the board view.
            lvlRef.GetComponent<ResourceUIManager>().whatUItoShow = "Board";
            lvlRef.GetComponent<ResourceUIManager>().DiceWindow();
            lvlRef.GetComponent<CameraController>().ActiveCam = "Alt";
            lvlRef.GetComponent<CameraController>().switchCamera();

            //add creature to pool
            lvlRef.GetComponent<CreaturePoolController>().turnPlayer.GetComponent<Player>().CreaturePool.Add(CurrentCreature);
            lvlRef.GetComponent<CreaturePoolController>().enableButtons();
            
            //Player not performing action can press end turn BTN.
            lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false;
        }
    }
    
}
