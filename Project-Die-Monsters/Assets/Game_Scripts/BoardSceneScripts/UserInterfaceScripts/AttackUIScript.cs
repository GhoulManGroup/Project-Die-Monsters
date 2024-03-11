using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEditor.Experimental.GraphView;

public class AttackUIScript : MonoBehaviour
{
    GameObject lvlRef;
    GameObject AIManager;

    [Header("Participants")]

    // The creature pieces involved in the combat.
    public CreatureToken attacker;
    public CreatureToken defender;

    // The Players or AI Who own those pieces.
    public GameObject attackingPlayer;
    public GameObject defendingPlayer;

    // The components of both objects.
    public GameObject attackerDisplay;
    public GameObject defenderDisplay;

    [Header("UI Elements")]
    [SerializeField] GameObject phaseIcon;
    [SerializeField] GameObject defenderStateIcon;
    [SerializeField] GameObject attackerStateIcon;
    [SerializeField] Button attackBTN;
    [SerializeField] Button defendBTN;
    [SerializeField] Button abilityBTN;
    [SerializeField] Button declineBTN;

    [SerializeField] UISpriteList spriteList = ;

    struct UISpriteList 
    {
        Sprite attackIcon;
        Sprite defendIcon;
        Sprite abilityIcon;
        Sprite resoluctionIcon;
        Sprite deathIcon;
    }

  //If player wait for input, If AI just check for player can defend then await input if true else resolve combat while polsihing the entire process to have visual 

    [Header("Combat Priority")]

    public string Action = "Decide"; // Decide *AP or Attack* // Defend or Resolve.

    //Tracks if we apply defence to incoming damage in calcuation.
    bool applyDefence = false;

    public void Awake()
    {
        //Hide window at start.
        hideAttackWindow();
        AIManager = GameObject.FindGameObjectWithTag("AIController");
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
        //phaseIcon.GetComponent<Image>().sprite = 
    }

    public void setDetails()
    {

        //Establish who is attacking and who is defending.
        if (lvlRef.GetComponent<LevelController>().currentTurnParticipant.ToString() == "0")
        {
            attackingPlayer = lvlRef.GetComponent<LevelController>().participants[0].gameObject;
            defendingPlayer = lvlRef.GetComponent<LevelController>().participants[1].gameObject;
        }
        else if (lvlRef.GetComponent<LevelController>().currentTurnParticipant.ToString() == "1")
        {
            attackingPlayer = lvlRef.GetComponent<LevelController>().participants[1].gameObject;
            defendingPlayer = lvlRef.GetComponent<LevelController>().participants[0].gameObject;
        }

        // set the stats of the creature pieces to the UI.
        attackerDisplay.GetComponent<CreatureDisplayTab>().DisplayCombatParticpants(attacker);
        defenderDisplay.GetComponent<CreatureDisplayTab>().DisplayCombatParticpants(defender);
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
        for (int i = 1; i < UIElements.Count; i++)
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

        UIElements[2].GetComponent<Image>().sprite = spriteList[0];
        buttonState();
        setDetails();
    }

    public void ButtonPressed()
    {
        string attackBTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (attackBTNPressed)
        {
            case "AttackBTN":
                AttackAction();    
                break;

            case "DefendBTN":
                //Apply defence value to combat this fight.
                buttonState();
                applyDefence = true;
                UIElements[2].GetComponent<Image>().sprite = spriteList[2];
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
                        lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false;
                        lvlRef.GetComponent<PlayerCreatureController>().ChosenAction = "None";
                        lvlRef.GetComponent<PlayerCreatureController>().CheckPossibleActions();
                        lvlRef.GetComponent<PlayerCreatureController>().OpenAndCloseControllerUI();
                        break;
                }           
                break;
        }
    }

    public void AttackAction()
    {
        if (attackingPlayer.GetComponent<AIOpponent>() != null)
        {
            attackingPlayer.GetComponent<AIOpponent>().attackCrestPoints -= attacker.GetComponent<CreatureToken>().attackCost;

            if (defendingPlayer.GetComponent<Player>().defenceCrestPoints >= defender.defenseCost)
            {
                Action = "Defend";
                UIElements[15].GetComponent<Image>().sprite = spriteList[1];
                buttonState();
            }else
            {
                ResolveCombat();
            }

        }
        else if (attackingPlayer.GetComponent<Player>() != null)
        {
            attackingPlayer.GetComponent<Player>().attackCrestPoints -= attacker.GetComponent<CreatureToken>().attackCost;

            if (defendingPlayer.GetComponent<AIOpponent>() != null)
            {
                if (defendingPlayer.GetComponent<AIOpponent>().defenceCrestPoints >= defender.defenseCost) 
                {
                    applyDefence = true;
                }

                ResolveCombat();
            }else if (defendingPlayer.GetComponent<Player>() != null)
            {
                if (defendingPlayer.GetComponent<Player>().defenceCrestPoints >= defender.defenseCost)
                {
                    Action = "Defend";
                    UIElements[15].GetComponent<Image>().sprite = spriteList[1];
                    buttonState();
                }
                else
                {
                    ResolveCombat();
                }
            }
        }


      
        //Check if other player has enough points to cover their defence cost. if so offer choice if not skip to damage calc.
   
        
        else if (defendingPlayer.GetComponent<Player>().defenceCrestPoints == 0)
        {
            Action = "Resolve";
            UIElements[2].GetComponent<Image>().sprite = spriteList[2];
            buttonState();
            ResolveCombat();
        }
    }

    public void buttonState() //Update the button interactable state based on the action.
    {
        switch (Action)
        {
            case "AIDecision":
                AUIButtons[0].GetComponent<Button>().interactable = false;
                AUIButtons[1].GetComponent<Button>().interactable = false;
                AUIButtons[2].GetComponent<Button>().interactable = false;
                AUIButtons[3].GetComponent<Button>().interactable = false;
                break;
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
        if (attackingPlayer.GetComponent<Player>() != null) 
        {
            buttonState();

        }

        setDetails();

        // check if defender health is less than 0 if so run the chekcstate function to destory it then change the UI sprite to Empty.
        if (defender.GetComponent<CreatureToken>().currentHealth <= 0)
        {
           UIElements[0].GetComponent<Image>().enabled = true;
           attacker.GetComponent<AbilityManager>().CheckTrigger("OnKill", attacker.gameObject);
        }
        attacker.GetComponent<AbilityManager>().CheckTrigger("OnAttack", attacker.gameObject);
        defender.GetComponent<AbilityManager>().CheckTrigger("OnHit", attacker.gameObject);
        attacker.GetComponent<CreatureToken>().CheckForAttackTarget();
        attacker.GetComponent<CreatureToken>().hasAttackedThisTurn = true;
        lvlRef.GetComponent<LevelController>().CheckForTriggersToResolve();
    }
}
