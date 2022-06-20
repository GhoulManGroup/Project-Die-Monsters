using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttackUIScript : MonoBehaviour
{
    GameObject lvlRef;
    GameObject creatureTargets;

    [Header("Target Pick")]
    public List<GameObject> targetPick = new List<GameObject>();
    public List<Text> targetText = new List<Text>();

    [Header("Attack_Window")]
    public List<GameObject> attackWindow = new List<GameObject>();
    string whatIsPlayer;
    GameObject attacker;
    GameObject defender;

    // Start is called before the first frame update
    void Start()
    {
        hideTargetPick();
        hideAttackWindow();
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showTargetPick()
    {
        creatureTargets = lvlRef.GetComponent<CreatureController>().ChosenCreature;
        int targetsToShow = creatureTargets.GetComponent<CreatureToken>().targets.Count;
                targetPick[0].GetComponent<Image>().enabled = true;
        switch (targetsToShow)
        {
            case 1:
                targetPick[1].GetComponent<Image>().enabled = true;
                targetPick[2].GetComponent<Image>().enabled = true;
                targetText[0].GetComponent<Text>().enabled = true;
                targetText[0].GetComponent<Text>().text = "Attack :" + creatureTargets.GetComponent<CreatureToken>().targets[0].name.ToString();
                targetPick[9].GetComponent<Image>().enabled = true;
                break;
            case 2:
                targetPick[1].GetComponent<Image>().enabled = true;
                targetPick[2].GetComponent<Image>().enabled = true;
                targetPick[3].GetComponent<Image>().enabled = true;
                targetPick[4].GetComponent<Image>().enabled = true;
                targetText[0].GetComponent<Text>().enabled = true;
                targetText[1].GetComponent<Text>().enabled = true;
                targetText[0].GetComponent<Text>().text = "Declare Attack Against :" + creatureTargets.GetComponent<CreatureToken>().targets[0].name.ToString();
                targetText[1].GetComponent<Text>().text = "Declare Attack Against :" + creatureTargets.GetComponent<CreatureToken>().targets[1].name.ToString();
                targetPick[9].GetComponent<Image>().enabled = true;
                break;
            case 3:
                targetPick[1].GetComponent<Image>().enabled = true;
                targetPick[2].GetComponent<Image>().enabled = true;
                targetPick[3].GetComponent<Image>().enabled = true;
                targetPick[4].GetComponent<Image>().enabled = true;
                targetPick[5].GetComponent<Image>().enabled = true;
                targetPick[6].GetComponent<Image>().enabled = true;
                targetText[0].GetComponent<Text>().enabled = true;
                targetText[1].GetComponent<Text>().enabled = true;
                targetText[2].GetComponent<Text>().enabled = true;
                targetText[0].GetComponent<Text>().text = "Declare Attack Against :" + creatureTargets.GetComponent<CreatureToken>().targets[0].name.ToString();
                targetText[1].GetComponent<Text>().text = "Declare Attack Against :" + creatureTargets.GetComponent<CreatureToken>().targets[1].name.ToString();
                targetText[2].GetComponent<Text>().text = "Declare Attack Against :" + creatureTargets.GetComponent<CreatureToken>().targets[2].name.ToString();
                targetPick[9].GetComponent<Image>().enabled = true;
                break;
            case 4:
                targetPick[1].GetComponent<Image>().enabled = true;
                targetPick[2].GetComponent<Image>().enabled = true;
                targetPick[3].GetComponent<Image>().enabled = true;
                targetPick[4].GetComponent<Image>().enabled = true;
                targetPick[5].GetComponent<Image>().enabled = true;
                targetPick[6].GetComponent<Image>().enabled = true;
                targetPick[7].GetComponent<Image>().enabled = true;
                targetPick[8].GetComponent<Image>().enabled = true;
                targetText[0].GetComponent<Text>().enabled = true;
                targetText[1].GetComponent<Text>().enabled = true;
                targetText[2].GetComponent<Text>().enabled = true;
                targetText[3].GetComponent<Text>().enabled = true;
                targetText[0].GetComponent<Text>().text = "Declare Attack Against :" + creatureTargets.GetComponent<CreatureToken>().targets[0].name.ToString();
                targetText[1].GetComponent<Text>().text = "Declare Attack Against :" + creatureTargets.GetComponent<CreatureToken>().targets[1].name.ToString();
                targetText[2].GetComponent<Text>().text = "Declare Attack Against :" + creatureTargets.GetComponent<CreatureToken>().targets[2].name.ToString();
                targetText[3].GetComponent<Text>().text = "Declare Attack Against :" + creatureTargets.GetComponent<CreatureToken>().targets[3].name.ToString();
                targetPick[9].GetComponent<Image>().enabled = true;
                break;
        }

    }

    public void hideTargetPick()
    {
        for (int i = 0; i < targetPick.Count; i++)
        {
            targetPick[i].GetComponent<Image>().enabled = false;
        }

        for (int i = 0; i < targetText.Count; i++)
        {
            targetText[i].GetComponent<Text>().enabled = false;
        }
    }

    public void ButtonPressed()
    {
        string attackBTNpressed = EventSystem.current.currentSelectedGameObject.name.ToString();
        print(attackBTNpressed);
        switch (attackBTNpressed)
        {
            case "0":
                whatIsPlayer = "Attacker";
                showAttackWindow();
                attacker = creatureTargets.gameObject;
                defender = creatureTargets.GetComponent<CreatureToken>().targets[0].gameObject;
                setAttackDetails();
                break;
            case "1":
                Debug.Log("Test2");
                break;
            case "2":
                Debug.Log("Test3");
                break;
            case "3":
                Debug.Log("Test4");
                break;
            case "CancelTarget":
                Debug.Log("TestCancel");
                // close this window on button press and reset the state.
                lvlRef.GetComponent<CreatureController>().ChosenAction = "Choosing";
                lvlRef.GetComponent<CreatureController>().attackWindowOpen = false;
                lvlRef.GetComponent<CreatureController>().HideAndShowButtons();
                hideTargetPick();
                break;
            case "AttackorDefenceBTN": // this button changes function depending on of this window is open for offencive or defencive reasons.
                if (whatIsPlayer == "Attacker")
                {

                }

                if (whatIsPlayer == "Defender")
                {

                }
                break;
            case "Ability":

                break;
               
        }
    }

    public void showAttackWindow()
    {
        for (int i = 0; i < attackWindow.Count; i++)
        {
            attackWindow[i].GetComponent<Image>().enabled = true;
        }
    }

    public void hideAttackWindow()
    {
        for (int i = 0; i < attackWindow.Count; i++)
        {
            attackWindow[i].GetComponent<Image>().enabled = false;
        }
    }

    public void setAttackDetails()
    {
        // check defender type (Creature Dungeon Lord)
        if (defender.GetComponent<DungeonLord>() != null)
        {
            // can only attack.
        }
        print(attacker.name);
        print(defender.name);
    }
}
