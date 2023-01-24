using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    // THIS SCRIPT IS TO FIND ALL POSSIBLE TARGETS NOT FILTER IF THEY ARE SUTIABLE FOR THE EFFECT.
    CreatureController creatureController;
    LevelController levelController;
    public AbilityEffect currentEffect;
    public bool hasDeclared = false;
    public List<GameObject> foundTargets = new List<GameObject>();
    public GameObject Position;
    public void Awake()
    {
        creatureController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>();
        levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
    }

    public List<GameObject> targetPool = new List<GameObject>();

    public void FindTarget()
    {
        switch (currentEffect.abilityTarget)
        {
            case AbilityEffect.EffectTargeting.random:

                break;

            case AbilityEffect.EffectTargeting.areaOfEffect:
                StartCoroutine("AOEEffectTargeting");
                break;

            case AbilityEffect.EffectTargeting.declared:
                switch (currentEffect.allowedTargets)
                {
                    case AbilityEffect.AllowedTargets.self:
                        foundTargets.Add(this.gameObject);
                        this.GetComponent<EffectManager>().targetsChecked = true;
                        break;

                    case AbilityEffect.AllowedTargets.friendly:
                        for (int i = 0; i < creatureController.CreaturesOnBoard.Count; i++)
                        {
                   
                            if (creatureController.CreaturesOnBoard[i].GetComponent<CreatureToken>().myOwner == levelController.whoseTurn)
                            {
                               targetPool.Add(creatureController.CreaturesOnBoard[i]);
                            }
                        }
                        StartCoroutine("DeclaringTargets");
                        break;

                    case AbilityEffect.AllowedTargets.hostile:
                        for (int i = 0; i < creatureController.CreaturesOnBoard.Count; i++)
                        {
                            if (creatureController.CreaturesOnBoard[i].GetComponent<CreatureToken>().myOwner != levelController.whoseTurn)
                            {
                               targetPool.Add(creatureController.CreaturesOnBoard[i]);                        
                            }
                        }
                        StartCoroutine("DeclaringTargets");
                        break;

                    case AbilityEffect.AllowedTargets.all:
                        for (int i = 0; i < creatureController.CreaturesOnBoard.Count; i++)
                        {
                            targetPool.Add(creatureController.CreaturesOnBoard[i]);                         
                        }
                        StartCoroutine("DeclaringTargets");
                        break;
                }
                break;
        }
    }

    #region Declare Targeting Code
    public IEnumerator DeclaringTargets()
    {

        for (int i = 0; i < targetPool.Count; i++)
        {
            targetPool[i].GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().SetIndicatorMaterial("PossibleTarget");
        }
        
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().ableToInteractWithBoard = true;
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().boardInteraction = "TargetPick";
        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().ShowAndUpdateInterface("Declare");

        while (hasDeclared == false)
        {
            yield return null;
        }

        for (int i = 0; i < targetPool.Count; i++)
        {
            targetPool[i].GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().ResetGridTile();
        }
        this.GetComponent<EffectManager>().targetsChecked = true;
        yield return null;
    }

    public void CheckDeclareState(GameObject clickedCreature)
    {
        if (foundTargets.Contains(clickedCreature))
        {
            foundTargets.Remove(clickedCreature);
            clickedCreature.GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().SetIndicatorMaterial("PossibleTarget");
        }
        else if (!foundTargets.Contains(clickedCreature))
        {
            if (foundTargets.Count != currentEffect.maximumTargetCount)
            {
                foundTargets.Add(clickedCreature);
                clickedCreature.GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().SetIndicatorMaterial("PickedTarget");
            }else if (foundTargets.Count == currentEffect.maximumTargetCount)
            {
                Debug.Log("Cant Add Already at cap");
            }
        }

        if (foundTargets.Count >= currentEffect.requiredTargetCount)
        {
            GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().confirmBTN.gameObject.GetComponent<Button>().interactable = true;
            GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().confirmBTNFunction = "DeclareTargets";
        }
        else if (foundTargets.Count < currentEffect.requiredTargetCount)
        {
            GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().confirmBTN.gameObject.GetComponent<Button>().interactable = false;
            GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().confirmBTNFunction = "Nothing";
        }
        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().ShowAndUpdateInterface("Declare");
    }

    #endregion

    #region AOE Targeting Code
    
    public IEnumerator AOEEffectTargeting()
    {
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().ableToInteractWithBoard = true;
        GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().boardInteraction = "AOETarget";
        DetermineAOEPosition();

        while(Position == null)
        {
            yield return null;
        }

        Debug.Log("Found Position" + Position.gameObject.name) ;

        yield return null;
    }

    public void DetermineAOEPosition()
    {//Determine where the AOE effect is occuring from based on this Enum then either go to next step or wait for player to declare game object.
        switch (currentEffect.AOEBoardPosition)
        {
            case AbilityEffect.AOEPosition.self:
                Position = this.gameObject.GetComponent<CreatureToken>().myBoardLocation;
                break;

            case AbilityEffect.AOEPosition.friendly:

                break;
        }
    }

    public void AOEDirection()
    {
        switch (currentEffect.AOEDirection)
        {
            case AbilityEffect.AOEDirections.front:

                break;
            case AbilityEffect.AOEDirections.frontBack:

                break;
            case AbilityEffect.AOEDirections.frontSides:

                break;
            case AbilityEffect.AOEDirections.sides:

                break;
            case AbilityEffect.AOEDirections.all:

                break;
        }
    }


    #endregion

    public void ResetManager()
    {
        currentEffect = null;
        hasDeclared = false;
        foundTargets.Clear();
        targetPool.Clear();
        StopAllCoroutines();
    }
}