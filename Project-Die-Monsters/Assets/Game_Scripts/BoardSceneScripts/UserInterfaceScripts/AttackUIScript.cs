using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttackUIScript : MonoBehaviour
{
    GameObject lvlRef;

    [Header("UI Elements")]
    public List<GameObject> UIElements = new List<GameObject>();
    public List<GameObject> AUIButtons = new List<GameObject>();
    public List<Sprite> spriteList = new List<Sprite>();

    [Header("Participants")]

    // The creature pieces involved in the combat.
    public GameObject attacker;
    public GameObject defender;

    // The Players or AI Who own those pieces.
    public GameObject attackingPlayer;
    public GameObject defendingPlayer;

    // The components of both objects.
    public List<GameObject> AttackerDisplay = new List<GameObject>();
    public List<GameObject> DefenderDisplay = new List<GameObject>();

    [Header("Combat Priority")]

    public string Action = "Decide"; // Decide *AP or Attack* // Defend or Resolve.

    //Tracks if we apply defence to incoming damage in calcuation.
    bool applyDefence = false;

    public void Awake()
    {
        //Hide window at start.
        hideAttackWindow();
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
        UIElements[12].GetComponent<Image>().sprite = spriteList[0];
    }

    public void hideAttackWindow()
    {
        for (int i = 0; i < UIElements.Count; i++)
        {
            if (UIElements[i].GetComponent<Image>() != null)
            {
                UIElements[i].GetComponent<Image>().enabled = false;
            }

            if (UIElements[i].GetComponent<Text>() != null)
            {
                UIElements[i].GetComponent<Text>().enabled = false;
            }
        }

        AUIButtons[0].GetComponent<Button>().interactable = false;
        AUIButtons[1].GetComponent<Button>().interactable = false;
        AUIButtons[2].GetComponent<Button>().interactable = false;
        AUIButtons[3].GetComponent<Button>().interactable = false;
    }

    public void displayAttackWindow()
    {
        // show the attack window UI
        for (int i = 0; i < 13; i++)
        {
            if (UIElements[i].GetComponent<Image>() != null)
            {
                UIElements[i].GetComponent<Image>().enabled = true;
            }

            if (UIElements[i].GetComponent<Text>() != null)
            {
                UIElements[i].GetComponent<Text>().enabled = true;
            }
        }

        UIElements[12].GetComponent<Image>().sprite = spriteList[0];
        buttonState();
        setDetails();
    }

    public void ButtonPressed()
    {
        string attackBTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (attackBTNPressed)
        {
            case "AttackBTN":
                //Subtract the cost of the attack from the player attack crests pool.
                attackingPlayer.GetComponent<Player>().attackCrestPoints -= attacker.GetComponent<CreatureToken>().attackCost;

                //Check if other player has enough points to cover their defence cost. if so offer choice if not skip to damage calc.
                if (defendingPlayer.GetComponent<Player>().defenceCrestPoints > 0)
                {
                    Action = "Defend";
                    UIElements[12].GetComponent<Image>().sprite = spriteList[1];
                    buttonState();
                    
                }else if (defendingPlayer.GetComponent<Player>().defenceCrestPoints == 0)
                {
                    Action = "Resolve";
                    UIElements[12].GetComponent<Image>().sprite = spriteList[2];
                    buttonState();
                    ResolveCombat();
                }
                break;

            case "DefendBTN":
                //Apply defence value to combat this fight.
                buttonState();
                applyDefence = true;
                UIElements[12].GetComponent<Image>().sprite = spriteList[2];
                Action = "Resolve";
                ResolveCombat();
                break;

            case "AbilityBTN":
                buttonState();
                break;

            case "CancleBTN":
                switch (Action)
                {
                    case "Defend":
                        // decide not to use defence crest in this combat.
                        ResolveCombat();
                        break;
                    case "Decide":
                        // go back to target list.
                        hideAttackWindow();
                        GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().OpenInspectWindow("AttackTargetSelection");
                        break;
                    case "Over":
                        //Combat over contiune turn.
                        hideAttackWindow();
                        //Call creature controller and set us back to controling the piece.
                        lvlRef.GetComponent<CreatureController>().ChosenAction = "None";
                        lvlRef.GetComponent<CreatureController>().CheckPossibleActions();
                        lvlRef.GetComponent<CreatureController>().OpenAndCloseControllerUI();
                        break;
                }           
                break;
        }
    }

    public void setDetails()
    {

        //Establish who is attacking and who is defending.
        if (lvlRef.GetComponent<LevelController>().whoseTurn == "Player0")
        {
            attackingPlayer = lvlRef.GetComponent<LevelController>().participants[0].gameObject;
            defendingPlayer = lvlRef.GetComponent<LevelController>().participants[1].gameObject;
        }
        else if (lvlRef.GetComponent<LevelController>().whoseTurn == "Player1")
        {
            attackingPlayer = lvlRef.GetComponent<LevelController>().participants[1].gameObject;
            defendingPlayer = lvlRef.GetComponent<LevelController>().participants[0].gameObject;
        }

        // set the stats of the creature pieces to the UI.
        AttackerDisplay[1].GetComponent<Text>().text = attacker.GetComponent<CreatureToken>().currentAttack.ToString();
        AttackerDisplay[2].GetComponent<Text>().text = attacker.GetComponent<CreatureToken>().currentDefence.ToString();
        AttackerDisplay[3].GetComponent<Text>().text = attacker.GetComponent<CreatureToken>().currentHealth.ToString();

        DefenderDisplay[1].GetComponent<Text>().text = defender.GetComponent<CreatureToken>().currentAttack.ToString();
        DefenderDisplay[2].GetComponent<Text>().text = defender.GetComponent<CreatureToken>().currentDefence.ToString();
        DefenderDisplay[3].GetComponent<Text>().text = defender.GetComponent<CreatureToken>().currentHealth.ToString();
    }

    public void buttonState() //Update the button interactable state based on the action.
    {
        switch (Action)
        {
            case "Decide":
                AUIButtons[0].GetComponent<Button>().interactable = true;
                AUIButtons[1].GetComponent<Button>().interactable = false;
                AUIButtons[2].GetComponent<Button>().interactable = true;
                AUIButtons[3].GetComponent<Button>().interactable = true;
                break;
            case "Resolve":
                AUIButtons[0].GetComponent<Button>().interactable = false;
                AUIButtons[1].GetComponent<Button>().interactable = false;
                AUIButtons[2].GetComponent<Button>().interactable = false;
                AUIButtons[3].GetComponent<Button>().interactable = false;
                break;
            case "Defend":
                AUIButtons[0].GetComponent<Button>().interactable = false;
                AUIButtons[1].GetComponent<Button>().interactable = true;
                AUIButtons[2].GetComponent<Button>().interactable = false;
                AUIButtons[3].GetComponent<Button>().interactable = true;
                break;
            case "Ability":

                break;
            case "Over":
                AUIButtons[1].GetComponent<Button>().interactable = false;
                AUIButtons[3].GetComponent<Button>().interactable = true;
                break;
            
        }
    }

    public void ResolveCombat()
    {
        // Set damage value.
        int damage = attacker.GetComponent<CreatureToken>().currentAttack;

        //Check for defence.
        switch (applyDefence)
        {
            // Subtract the damage value from defender health if damage > 0.
            case true:
                Debug.Log("Defence True");

                damage -= defender.GetComponent<CreatureToken>().currentDefence;
                print(damage);

                if (damage > 0)
                {
                    Debug.Log("Defence < Damage");
                    defender.GetComponent<CreatureToken>().currentHealth -= damage;
                    
                }else if (damage <= 0)
                {
                    Debug.Log("Defence > Damage");
                }
                break;

            case false:
                Debug.Log("Defence False");
                defender.GetComponent<CreatureToken>().currentHealth -= damage;             
                break;
        }

        //Change state to over.
        Action = "Over";

        //Update our button states & update the UI details eg health.
        buttonState();
        setDetails();

        // check if defender health is less than 0 if so run the chekcstate function to destory it then change the UI sprite to Empty.
        if (defender.GetComponent<CreatureToken>().currentHealth <= 0)
        {
           UIElements[13].GetComponent<Image>().enabled = true;
            attacker.GetComponent<AbilityManager>().CheckTrigger("OnKill", attacker.gameObject);
            //Add call here for Has Killed Trigger Check!-----------------------------------------------------------------------------------------------------------------
        }
        attacker.GetComponent<AbilityManager>().CheckTrigger("OnAttack", attacker.gameObject);
        defender.GetComponent<AbilityManager>().CheckTrigger("OnHit", attacker.gameObject);

        // tell each piece in the fight to update their states.
        defender.GetComponent<CreatureToken>().StartCoroutine("CheckCreatureHealth");
        attacker.GetComponent<CreatureToken>().CheckForAttackTarget();
        attacker.GetComponent<CreatureToken>().hasAttackedThisTurn = true;
    }

}
