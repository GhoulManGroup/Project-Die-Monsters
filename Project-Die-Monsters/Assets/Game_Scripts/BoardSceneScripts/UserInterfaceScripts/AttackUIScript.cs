using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class AttackUIScript : MonoBehaviour
{
    #region Varibles

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

    [SerializeField] GameObject panel;
    [SerializeField] GameObject phaseIcon;
    [SerializeField] GameObject defenderStateIcon;
    [SerializeField] GameObject attackerStateIcon;
    [SerializeField] Button attackBTN;
    [SerializeField] Button defendBTN;
    [SerializeField] Button abilityBTN;
    [SerializeField] Button declineBTN;

    [SerializeField] UISpriteList Sprites = default;


    [Serializable]
    struct UISpriteList
    {
        public Sprite attackIcon;
        public Sprite defendIcon;
        public Sprite abilityIcon;
        public Sprite resoluctionIcon;
        public Sprite deathIcon;
    }
    #endregion

    //If player wait for input, If AI just check for player can defend then await input if true else resolve combat while polsihing the entire process to have visual 

    [Header("Combat Priority")]

    public string Action = "Decide"; // Decide *AP or Attack* // Defend or Resolve.

    //Tracks if we apply defence to incoming damage in calcuation.
    bool applyDefence = false;

    public void Awake()
    {
        //Hide window at start.
        HideAttackWindow();
        AIManager = GameObject.FindGameObjectWithTag("AIController");
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
        phaseIcon.GetComponent<Image>().sprite = Sprites.attackIcon;
    }

    public void SetDetails()
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

    public void HideAttackWindow()
    {
        panel.SetActive(false);
        attackBTN.interactable = false;
        defendBTN.interactable = false;
        abilityBTN.interactable = false;
        declineBTN.interactable = false;
    }

    public void DisplayAttackWindow(string action)
    {
        Action = action;
        panel.SetActive(true);
        buttonState();
        SetDetails();
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
                
                buttonState();
                applyDefence = true;

                //phaseIcon.GetComponent<Image>().sprite = spriteList.defed
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
                        HideAttackWindow();
                        GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().OpenInspectWindow("AttackTargetSelection");
                        break;
                    case "Over":
                        //Combat over contiune turn.
                        HideAttackWindow();
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

    public void buttonState() //Update the button interactable state based on the action.
    {
        switch (Action)
        {
            case "AIDecision":
                attackBTN.interactable = false;
                defendBTN.interactable = false;
                abilityBTN.interactable = false;
                declineBTN.interactable = false;
                break;
            case "Decide":
                attackBTN.interactable = true;
                defendBTN.interactable = false;
                abilityBTN.interactable = false;
                declineBTN.interactable = true;
                break;
            case "Resolve":
                attackBTN.interactable = true;
                defendBTN.interactable = true;
                abilityBTN.interactable = true;
                declineBTN.interactable = true;
                break;
            case "Defend":
                attackBTN.interactable = false;
                defendBTN.interactable = true;
                abilityBTN.interactable = false;
                declineBTN.interactable = true;
                break;
            case "Ability":
                attackBTN.interactable = false;
                defendBTN.interactable = false;
                abilityBTN.interactable = true;
                declineBTN.interactable = true;
                break;
            case "Over":
                attackBTN.interactable = false;
                defendBTN.interactable = false;
                abilityBTN.interactable = false;
                declineBTN.interactable = true;
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
                phaseIcon.GetComponent<Image>().sprite = Sprites.defendIcon;
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
                    phaseIcon.GetComponent<Image>().sprite = Sprites.defendIcon;
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
            phaseIcon.GetComponent<Image>().sprite = Sprites.resoluctionIcon;
            buttonState();
            ResolveCombat();
        }
    }

    public void ResolveCombat()
    {
        // Set damage value.
        int damage = attacker.GetComponent<CreatureToken>().currentAttack;

        attackerStateIcon.GetComponent<Image>().sprite = Sprites.attackIcon;
        //Play Attack Noise

        //Check for defence.
        switch (applyDefence)
        {
            // Subtract the damage value from defender health if damage > 0.
            case true:

                defenderStateIcon.GetComponent<Image>().sprite = Sprites.defendIcon;

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

        SetDetails();

        attacker.GetComponent<AbilityManager>().CheckTrigger("OnAttack", attacker.gameObject);

        defender.GetComponent<AbilityManager>().CheckTrigger("OnHit", attacker.gameObject);

        // check if defender health is less than 0 if so run the chekcstate function to destory it then change the UI sprite to Empty.
        if (defender.GetComponent<CreatureToken>().currentHealth <= 0)
        {
           defenderDisplay.GetComponent<Image>().sprite = Sprites.deathIcon;
           attacker.GetComponent<AbilityManager>().CheckTrigger("OnKill", attacker.gameObject);
        }

        attacker.GetComponent<CreatureToken>().CheckForAttackTarget();
        attacker.GetComponent<CreatureToken>().hasAttackedThisTurn = true;
        lvlRef.GetComponent<LevelController>().CheckForTriggersToResolve();
    }
}
