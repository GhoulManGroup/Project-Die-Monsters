using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InspectTabScript : MonoBehaviour // A UI display of a creature card used for checking whats inside a die and picking a attack target.
{
    [Header("Window Components")]
    public List<GameObject> myComponents = new List<GameObject>();

    public GameObject creatureArt;
    public GameObject creatureLevel;
    public GameObject creatureTribe;
    public GameObject creatureType;
    public Text creatureName;
    public Text attackValue;
    public Text defenceValue;
    public Text healthValue;

    GameObject lvlRef;

    [Header("Window States")]
    public string usedFor; // this tracks what we are opening the window four which will then control what it displays & what components are enabled.

    [Header("WindowContents")]
    public Creature CurrentCreature; // the creature scriptableobject stored inside the die or creature pool.

    public GameObject CurrentCreatureToken; // the board piece that is being used for a combat action.
    int targetShown = 0;


    // Start is called before the first frame update
    void Start()
    {
        HideFunction();
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
    }

    public void ButtonPressed()
    {
        string inspectBTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (inspectBTNPressed)
        {
            case "SelectCreatureBTN":
                if (usedFor == "DieInspect")
                {
                    AddCreatureToPool();
                }

                if (usedFor == "PoolInspect")
                {

                }

                if (usedFor == "AttackTargetSelection")
                {
                    CreatureIsAttackTarget();
                }
                break;

            case "ForwardBTN":
                if (targetShown < CurrentCreatureToken.GetComponent<CreatureToken>().targets.Count -1)
                {
                    targetShown += 1;
                    DisplayCurrentTarget();
                    Debug.Log(targetShown);
                }
                break;

            case "BackBTN":
                if (targetShown > 0)
                {
                    targetShown -= 1;
                    DisplayCurrentTarget();
                    Debug.Log(targetShown);
                }
                break;
            case "CloseBTN":
                // go back to previous step.
                lvlRef.GetComponent<CreatureController>().ChosenAction = "Choosing";
                lvlRef.GetComponent<CreatureController>().CheckPossibleActions();
                lvlRef.GetComponent<CreatureController>().HideAndShowButtons();
                HideFunction();
                break;
        }
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
        int componentsToDisplay = 0;

        switch (usedFor)
        {
            case "DieInspect":
                componentsToDisplay = 11;
                break;
            case "PoolInspect":
                componentsToDisplay = 10;
                break;
            case "AttackTargetSelection":
                DisplayCurrentTarget();
                componentsToDisplay = myComponents.Count;
                break;
        }
        // add check for state to choosecreature  button if not in dice window;
        for (int i = 0; i < componentsToDisplay; i++)
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

    // These functions are souly for displaying the contents of the dice in the die UI and creature pool

    public void setDetails() // sets the inspect window when displaying die contents either in manager or creature pool.
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

    public void AddCreatureToPool()
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

    //These functions are for attack target selection

    public void DisplayCurrentTarget()
    {
        Creature target = CurrentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].GetComponent<CreatureToken>().myCreature;
        creatureArt.GetComponent<Image>().sprite = target.CardArt;
        creatureLevel.GetComponent<Image>().sprite = target.LevelSprite;
        creatureTribe.GetComponent<Image>().sprite = target.TribeSprite;
        creatureType.GetComponent<Image>().sprite = target.TypeSprite;
        creatureName.GetComponent<Text>().text = target.creatureType.ToString();
        attackValue.GetComponent<Text>().text = "ATK" + target.Attack.ToString();
        defenceValue.GetComponent<Text>().text = "DEF" + target.Defence.ToString();
        healthValue.GetComponent<Text>().text = "HP" + target.Health.ToString();
    }
    
    public void CreatureIsAttackTarget()
    {
        GameObject CW = GameObject.FindGameObjectWithTag("CombatWindow");
        CW.GetComponent<AttackUIScript>().attacker = CurrentCreatureToken;
        CW.GetComponent<AttackUIScript>().defender = CurrentCreatureToken.GetComponent<CreatureToken>().targets[targetShown].gameObject;
        CW.GetComponent<AttackUIScript>().displayAttackWindow();
        //Hide this Window for now
        HideFunction();
    }
}
