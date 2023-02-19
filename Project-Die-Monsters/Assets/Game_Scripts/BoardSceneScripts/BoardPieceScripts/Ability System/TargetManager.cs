using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    // THIS SCRIPT IS TO FIND ALL POSSIBLE TARGETS NOT FILTER IF THEY ARE SUTIABLE FOR THE EFFECT.
    CreatureController creatureController;
    LevelController levelController;
    public AbilityEffect currentEffect;
    public bool hasDeclared = false;
    public List<GameObject> foundTargets = new List<GameObject>(); //What EffectManager Stores in its effect corutine as the chosen targets.
    public List<GameObject> targetPool = new List<GameObject>();//Where we store the possible targets for this effect.

    [Header("AOE Specific Varibles")]
    public GameObject Position;
    public bool directionIndicated;

    public void Awake()
    {
        creatureController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreatureController>();
        levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
    }

    public void FindTarget()
    {
        hasDeclared = false;
        directionIndicated = false;
        switch (currentEffect.howAbilityTarget)
        {
            case AbilityEffect.EffectTargeting.random:
                StartCoroutine("RandomTarget");
                break;

            case AbilityEffect.EffectTargeting.areaOfEffect:
                StartCoroutine("AOEEffectTargeting");
                break;

            case AbilityEffect.EffectTargeting.declared:

                switch (currentEffect.allowedTargets)
                {
                    case AbilityEffect.AllowedTargets.otherTrigger:
                        foundTargets.Add(this.GetComponent<AbilityManager>().TriggeringCreature);
                        this.GetComponent<EffectManager>().targetsChecked = true;
                        break;

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

    #region Random Targeting Code
    private IEnumerator RandomTarget()
    {
        switch (currentEffect.allowedTargets)
        {
            case AbilityEffect.AllowedTargets.self:
                Debug.LogError("Random Target Self Not Supported Please Check Effect Settings");
                break;

            case AbilityEffect.AllowedTargets.friendly:
                for (int i = 0; i < creatureController.CreaturesOnBoard.Count; i++)
                {

                    if (creatureController.CreaturesOnBoard[i].GetComponent<CreatureToken>().myOwner == levelController.whoseTurn)
                    {
                        targetPool.Add(creatureController.CreaturesOnBoard[i]);
                    }
                }
                break;

            case AbilityEffect.AllowedTargets.hostile:
                for (int i = 0; i < creatureController.CreaturesOnBoard.Count; i++)
                {
                    if (creatureController.CreaturesOnBoard[i].GetComponent<CreatureToken>().myOwner != levelController.whoseTurn)
                    {
                        targetPool.Add(creatureController.CreaturesOnBoard[i]);
                    }
                }
  
                break;

            case AbilityEffect.AllowedTargets.all:
                for (int i = 0; i < creatureController.CreaturesOnBoard.Count; i++)
                {
                    targetPool.Add(creatureController.CreaturesOnBoard[i]);
                }
                break;
        }

        while(targetPool.Count > currentEffect.requiredTargetCount)
        {
            int removeAt = Random.Range(0, targetPool.Count);
            targetPool.RemoveAt(removeAt);
        }

        foundTargets.AddRange(targetPool);

        this.GetComponent<EffectManager>().targetsChecked = true;
        yield return null;
    }   
    #endregion

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

        ResetGridIndicators();
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

        AOEDirection();
        TargetsAllowed();
        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().ShowAndUpdateInterface("AOE");
        GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().confirmBTNFunction = "DeclareAOEPosition";

        while (directionIndicated == false)
        {
            yield return null;
        }

        ResetGridIndicators();

        this.GetComponent<EffectManager>().targetsChecked = true;
        yield return null;
    }

    public void DetermineAOEPosition()
    {//Determine where the AOE effect is occuring from based on this Enum then either go to next step or wait for player to declare game object.
        switch (currentEffect.AOEBoardPosition)
        {
            case AbilityEffect.AOEPosition.self:
                Position = this.gameObject.GetComponent<CreatureToken>().myBoardLocation;
                break;
        }
    }

    public void AOEDirection()
    {
        CreatureToken mySelf = this.gameObject.GetComponent<CreatureToken>();

        switch (currentEffect.AOEDirection)
        {
            case AbilityEffect.AOEDirections.front:
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.facingDirection, 0, this.gameObject);
                break;
            case AbilityEffect.AOEDirections.frontBack:
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.facingDirection, 0, this.gameObject);
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.behindDirection, 0, this.gameObject);
                break;
            case AbilityEffect.AOEDirections.frontSides:
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.facingDirection, 0, this.gameObject);
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.sideDirection, 0, this.gameObject);
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.otherSideDirection, 0, this.gameObject);
                break;
            case AbilityEffect.AOEDirections.sides:
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.sideDirection, 0, this.gameObject);
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.otherSideDirection, 0, this.gameObject);
                break;
            case AbilityEffect.AOEDirections.all:
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.facingDirection, 0, this.gameObject);
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.behindDirection, 0, this.gameObject);
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.sideDirection, 0, this.gameObject);
                mySelf.myBoardLocation.GetComponent<GridScript>().FindTargetsInDirection(mySelf.otherSideDirection, 0, this.gameObject);
                break;
        }
    }

    public void TargetsAllowed()
    {
        switch (currentEffect.allowedTargets)
        {
            case AbilityEffect.AllowedTargets.all:
                for (int i = 0; i < targetPool.Count; i++)
                {
                    if (targetPool[i].GetComponent<GridScript>().creatureAboveMe != null)
                    {
                        foundTargets.Add(targetPool[i].GetComponent<GridScript>().creatureAboveMe);
                        targetPool[i].GetComponent<GridScript>().SetIndicatorMaterial("PickedTarget");
                    }
                }
                break;

            case AbilityEffect.AllowedTargets.friendly:
                for (int i = 0; i < targetPool.Count; i++)
                {
                    if (targetPool[i].GetComponent<GridScript>().creatureAboveMe != null)
                    {
                        if (targetPool[i].GetComponent<GridScript>().creatureAboveMe.GetComponent<CreatureToken>().myOwner == levelController.whoseTurn)
                        {
                            foundTargets.Add(targetPool[i].GetComponent<GridScript>().creatureAboveMe);
                            targetPool[i].GetComponent<GridScript>().SetIndicatorMaterial("PickedTarget");
                        }
                    }
                }
                break;

            case AbilityEffect.AllowedTargets.hostile:
                for (int i = 0; i < targetPool.Count; i++)
                {
                    if (targetPool[i].GetComponent<GridScript>().creatureAboveMe != null)
                    {
                        if (targetPool[i].GetComponent<GridScript>().creatureAboveMe.GetComponent<CreatureToken>().myOwner != levelController.whoseTurn)
                        {
                            foundTargets.Add(targetPool[i].GetComponent<GridScript>().creatureAboveMe);
                            targetPool[i].GetComponent<GridScript>().SetIndicatorMaterial("PickedTarget");
                        }
                    }
                }
                break;

            case AbilityEffect.AllowedTargets.self:
                Debug.Log("Error Should not be able to effect self check for wrong code");
                break;
        }
    }
    #endregion

    public void ResetManager()
    {
        ResetGridIndicators();
        currentEffect = null;
        directionIndicated = false;
        hasDeclared = false;
        foundTargets.Clear();
        targetPool.Clear();
        StopAllCoroutines();
    }

    public void ResetGridIndicators()
    {
        if (currentEffect != null)
        {
            if (currentEffect.howAbilityTarget == AbilityEffect.EffectTargeting.declared)
            {
                for (int i = 0; i < targetPool.Count; i++)
                {
                    targetPool[i].GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().ResetGridTile();
                }
            }
            else if (currentEffect.howAbilityTarget == AbilityEffect.EffectTargeting.areaOfEffect)
            {
                for (int i = 0; i < targetPool.Count; i++)
                {
                    targetPool[i].GetComponent<GridScript>().ResetGridTile();
                }
            }
        }

        targetPool.Clear();
    }

    #region CheckCanBeCast
    //Checks if the ability in question can be cast by checking if their are enough suitable targets to meet each effects requirement.
    public IEnumerator HasPossibleTargets()
    {
        int possibleTargetsFound = 0;
        switch (currentEffect.howAbilityTarget)
        {
            case AbilityEffect.EffectTargeting.areaOfEffect:
                possibleTargetsFound = currentEffect.requiredTargetCount;  
                break;
            case AbilityEffect.EffectTargeting.declared:
                switch (currentEffect.allowedTargets)
                {
                    case AbilityEffect.AllowedTargets.friendly:
                        for (int i = 0; i < creatureController.CreaturesOnBoard.Count; i++)
                        {
                            if (creatureController.CreaturesOnBoard[i].GetComponent<CreatureToken>().myOwner == levelController.whoseTurn)
                            {
                                possibleTargetsFound += 1;
                                targetPool.Add(creatureController.CreaturesOnBoard[i]);
                            }
                        }
                        break;

                    case AbilityEffect.AllowedTargets.hostile:
                        for (int i = 0; i < creatureController.CreaturesOnBoard.Count; i++)
                        {
                            if (creatureController.CreaturesOnBoard[i].GetComponent<CreatureToken>().myOwner != levelController.whoseTurn)
                            {
                                possibleTargetsFound += 1;
                                targetPool.Add(creatureController.CreaturesOnBoard[i]);
                            }
                        }
                        break;

                    case AbilityEffect.AllowedTargets.self:
                        targetPool.Add(this.gameObject);
                        yield break;

                    case AbilityEffect.AllowedTargets.all:
                        possibleTargetsFound = creatureController.CreaturesOnBoard.Count;
                        targetPool.AddRange(creatureController.CreaturesOnBoard);
                        break;
                }
                break;
            case AbilityEffect.EffectTargeting.random:
                switch (currentEffect.allowedTargets)
                {
                    case AbilityEffect.AllowedTargets.friendly:
                        for (int i = 0; i < creatureController.CreaturesOnBoard.Count; i++)
                        {
                            if (creatureController.CreaturesOnBoard[i].GetComponent<CreatureToken>().myOwner == levelController.whoseTurn)
                            {
                                possibleTargetsFound += 1;
                                targetPool.Add(creatureController.CreaturesOnBoard[i]);
                            }
                        }
                        break;

                    case AbilityEffect.AllowedTargets.hostile:
                        for (int i = 0; i < creatureController.CreaturesOnBoard.Count; i++)
                        {
                            if (creatureController.CreaturesOnBoard[i].GetComponent<CreatureToken>().myOwner != levelController.whoseTurn)
                            {
                                possibleTargetsFound += 1;
                                targetPool.Add(creatureController.CreaturesOnBoard[i]);
                            }
                        }
                        break;

                    case AbilityEffect.AllowedTargets.self:
                        Debug.LogError("Hello Self Isnt a supported allowTarget option for Random Please Change");
                        yield break;

                    case AbilityEffect.AllowedTargets.all:
                        possibleTargetsFound = creatureController.CreaturesOnBoard.Count;
                        targetPool.AddRange(creatureController.CreaturesOnBoard);
                        break;
                }
                break;
        }

        if (possibleTargetsFound >= currentEffect.requiredTargetCount)
        {
            this.GetComponent<EffectManager>().targetsExist = true;
            Debug.Log("Hello From Targets Found" + targetPool.Count); 
        }else if (possibleTargetsFound < currentEffect.requiredTargetCount)
        {
            this.GetComponent<AbilityManager>().ResetAbilitySystem();
            Debug.Log("Hello From Targets Not Found");
        }
        yield return null;
    }
    #endregion
}
