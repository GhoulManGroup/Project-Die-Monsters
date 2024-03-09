using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using static UnityEngine.GraphicsBuffer;

public class CreatureDisplayTab : MonoBehaviour
{
    public GameObject MyController; //The Script that oversees the use of this instance of the creature display panel eg, inspect window, combat window, deck build window.

    [Header("Creature Details")]
    public GameObject creatureArt;
    public GameObject creatureLevel;
    public GameObject creatureTribe;
    public GameObject creatureType;
    public Text creatureName;
    public Text creatureAbility;
    public Text attackValue;
    public Text defenceValue;
    public Text healthValue;

    // Use for Inspect Window Details
    public void PreviewCreature(string usedFor)
    {
        InspectWindowController instance = MyController.GetComponent<InspectWindowController>();

        //Check if we are displaying a creature in the scriptable object store in a dice for example or a creature piece on the board then set the details of the UI Panel.
        if (usedFor == "DrawDice" || usedFor == "DieInspect" || usedFor == "PoolInspect")
        {
            creatureName.GetComponent<Text>().text = instance.currentCreature.CreatureName;
            if (instance.currentCreature.myAbility != null)
            {
                creatureAbility.GetComponent<Text>().text = instance.currentCreature.myAbility.AbilityName + " " + instance.currentCreature.myAbility.AbilityDescriptionText;
            }
            attackValue.GetComponent<Text>().text = "ATK" + instance.currentCreature.Attack;
            defenceValue.GetComponent<Text>().text = "DEF" + instance.currentCreature. Defence;
            healthValue.GetComponent<Text>().text = "HP" + instance.currentCreature.Health;
        }
        else if (usedFor == "PieceInspect")
        {
            creatureName.GetComponent<Text>().text = instance.currentCreature.CreatureName;
            if (instance.currentCreature.myAbility != null)
            {
                creatureAbility.GetComponent<Text>().text = instance.currentCreature.myAbility.AbilityName + " " + instance.currentCreature.myAbility.AbilityDescriptionText;
            }
            attackValue.GetComponent<Text>().text = "ATK" + instance.currentCreaturePiece.GetComponent<CreatureToken>().currentAttack;
            defenceValue.GetComponent<Text>().text = "DEF" + instance.currentCreaturePiece.GetComponent<CreatureToken>().currentDefence;
            healthValue.GetComponent<Text>().text = "HP" + instance.currentCreaturePiece.GetComponent<CreatureToken>().currentHealth;
        }
        else if (usedFor == "AttackTargetSelection")
        {
            CreatureToken target = instance.currentCreaturePiece.GetComponent<CreatureToken>().targets[instance.targetShown].GetComponent<CreatureToken>();
            creatureName.GetComponent<Text>().text = target.myCreature.name;
            if (instance.currentCreature.myAbility != null)
            {
                creatureAbility.GetComponent<Text>().text = instance.currentCreature.myAbility.AbilityName + " " + instance.currentCreature.myAbility.AbilityDescriptionText;
            }
            attackValue.GetComponent<Text>().text = "ATK" + target.currentAttack;
            defenceValue.GetComponent<Text>().text = "DEF" + target.currentDefence;
            healthValue.GetComponent<Text>().text = "HP" + target.currentHealth;
        }
        creatureArt.GetComponent<Image>().sprite = instance.currentCreature.CardArt;
        creatureLevel.GetComponent<Image>().sprite = instance.currentCreature.LevelSprite;
        creatureTribe.GetComponent<Image>().sprite = instance.currentCreature.TribeSprite;
        creatureType.GetComponent<Image>().sprite = instance.currentCreature.TypeSprite;
    }

    //use this to display combat creature details
    public void DisplayCombatParticpants(CreatureToken displayTarget )
    {
        creatureAbility.GetComponent<Text>().text = displayTarget.myCreature.myAbility.AbilityName + " " + displayTarget.myCreature.myAbility.AbilityDescriptionText;
        attackValue.GetComponent<Text>().text = "ATK" + displayTarget.currentAttack;
        defenceValue.GetComponent<Text>().text = "DEF" + displayTarget.currentDefence;
        healthValue.GetComponent<Text>().text = "HP" + displayTarget.currentHealth;
        creatureArt.GetComponent<Image>().sprite = displayTarget.myCreature.CardArt;
        creatureLevel.GetComponent<Image>().sprite = displayTarget.myCreature.LevelSprite;
        creatureTribe.GetComponent<Image>().sprite = displayTarget.myCreature.TribeSprite;
        creatureType.GetComponent<Image>().sprite = displayTarget.myCreature.TypeSprite;
    }
}
