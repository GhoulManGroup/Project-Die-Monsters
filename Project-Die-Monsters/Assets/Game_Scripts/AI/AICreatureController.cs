using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreatureController : MonoBehaviour
{
    public List<GameObject> myCreatures = new List<GameObject>();

    PathController pathfinding;
    public CreatureToken creature;
    AbilityManager ability;

    public bool actionsDone = false;
    public bool actionDone = false;

    [Header("Creature Actions")]
    [SerializeField] bool canMove = false;
    [SerializeField] bool wantToMove = true;
    [SerializeField] bool canAttack = false;
    [SerializeField] string attackTarget = "None";
    [SerializeField] bool CanAbility = false;

    [Header("Debug")]
    public string WhichActionIsBrokenTracker = "None";

    public void Start()
    {
        pathfinding = GameObject.FindGameObjectWithTag("LevelController").GetComponent<PathController>();
    }

    public IEnumerator ActionPhase()
    {
        for (int i = 0; i < myCreatures.Count; i++)
        {
            creature = myCreatures[i].GetComponent<CreatureToken>();

            ability = myCreatures[i].GetComponent<AbilityManager>();

            yield return CheckPossibleActions();

            actionsDone = false;

            yield return PerformActions();

            while (actionsDone == false)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }

       this.GetComponent<AIManager>().PhaseDone = true;
        ResetToDefault();
        actionsDone = false;
    }

    private void ResetToDefault()
    {
        actionDone = false;
        canMove = false;
        wantToMove = false;
        canAttack = false;
        CanAbility = false;
    }

    [SerializeField]  int actionsToTake = 0;

    public IEnumerator CheckPossibleActions()
    {
        actionsToTake = 0;

        yield return  ability.StartCoroutine("CheckAbilityCanBeCast");
        if (ability.canBeCast == true && creature.hasUsedAbilityThisTurn == false && creature.abilityCost <= this.GetComponent<AIManager>().currentOpponent.GetComponent<AIOpponent>().abiltyPowerCrestPoints)
        {
            CanAbility = true;
            actionsToTake += 1;
        }

        creature.CheckForAttackTarget();
        if (creature.canReachTarget == true && creature.hasAttackedThisTurn == false && creature.attackCost <= this.GetComponent<AIManager>().currentOpponent.GetComponent<AIOpponent>().attackCrestPoints)
        {
            canAttack = true;
            actionsToTake += 1;
        }else
        {
            canAttack = false;
        }

        yield return pathfinding.StartCoroutine("DeclarePathfindingConditions", creature.gameObject);
        if (creature.hasMovedThisTurn == false && pathfinding.possibleToMove == true)
        {
            if (canAttack == true)
            {
                wantToMove = false;
            }
            else
            {
                wantToMove = true;
                canMove = true;
                actionsToTake += 1;
            }
        }
        else
        {
            //clear list if not going to be used to move else it will block stuff
            Debug.Log("Can't Move Clear Pathfinding for next creature");
            pathfinding.StartCoroutine("ResetBoard", "Reset");
        }
    }

    public IEnumerator performActionAgain()
    {
        yield return StartCoroutine(PerformActions());
    }

    IEnumerator PerformActions()
    {
        Debug.Log("StartingPerformAction");

        if (canAttack == true)
        {
            Debug.Log("Starting Attack Action");
            yield return StartCoroutine(AICreatureAttack());
        }

        if (canMove == true && wantToMove == true)
        {
            Debug.Log("Starting Move Action");
            yield return StartCoroutine(AICreatureMove());
        }
        else
        {
            pathfinding.StartCoroutine("ResetBoard", "Reset");
        }

        if (CanAbility == true)
        {
            yield return StartCoroutine(AICreatureCastAbility());
        }


        yield return StartCoroutine(CheckPossibleActions());

        Debug.LogError("Actions Remaning : " + actionsToTake);

        if (actionsToTake != 0)
        {
            Debug.Log("Still Actions Can Be Taken By Creature : " + creature.name);
            StartCoroutine(performActionAgain());
            
        }
        else
        { 
            Debug.Log("No More Actions  Can Be Taken By Creature : " + creature.name);
            pathfinding.StartCoroutine("ResetBoard", "Reset");
            actionsDone = true;
            ResetToDefault();
        }   
    }

    IEnumerator AICreatureAttack()
    {
        WhichActionIsBrokenTracker = "Attack";
        //ADD Text to explain each step of the combat phase to the player eg, use defence crest? , Exit combat. Add text to Window I think for each Button.
        AttackUIScript combatWindow = GameObject.FindGameObjectWithTag("CombatWindow").GetComponent<AttackUIScript>();
        
        //Declare attacker
        combatWindow.attacker = creature;

        //declare defender


        //Check for Dungeon Lord else, pick creature with best chance of victory.
        for (int i = 0; i < creature.targets.Count; i++)
        {

            if (creature.targets[i].GetComponent<DungeonLord>() != null)
            {
                attackTarget = "DungeonLord";
                break;
            }
            
            if (creature.targets[i].GetComponent<CreatureToken>() != null)
            {
                CreatureToken defender = creature.targets[0].GetComponent<CreatureToken>();

                attackTarget = "Creature";
                if (creature.targets[i].GetComponent<CreatureToken>().myBoardLocation.GetComponent<GridScript>().distanceFromPlayerDungeonLord < defender.myBoardLocation.GetComponent<GridScript>().distanceFromPlayerDungeonLord)
                {
                    if (defender.currentHealth > creature.currentAttack) // If current target can't be killed switch to closer target to prioritse as kill is a better choice.
                    {
                        defender = creature.targets[i].GetComponent<CreatureToken>();
                    }
                }
            }
        }



        if (attackTarget == "DungeonLord")
        {
            //Do Damage to Dungeon Lord then Pass.
        }else if (attackTarget == "Creature")
        {
            combatWindow.defender = defender; // replace with pick target creature with lowest tile

            combatWindow.DisplayAttackWindow("AIDecision");

            yield return new WaitForSeconds(5f);

            combatWindow.AttackAction();
        }

        while (actionDone == false)
        {
            yield return null;
        }

        actionsToTake -= 1;
        actionDone = false;
        WhichActionIsBrokenTracker = "None";

    }

    IEnumerator AICreatureCastAbility()
    {
        yield return null;
    }

    IEnumerator AICreatureMove()
    {
        WhichActionIsBrokenTracker = "Movement";

        if (pathfinding.reachableTiles.Count == 0)
        {
            Debug.LogError("No path WHy Am I called");
            actionsToTake -= 1;
            yield break;
        }

        GameObject tileChosen = pathfinding.reachableTiles[0];

        for (int i = 0; i < pathfinding.reachableTiles.Count; i++)
        {
            if (pathfinding.reachableTiles[i].GetComponent<GridScript>().distanceFromPlayerDungeonLord < tileChosen.GetComponent<GridScript>().distanceFromPlayerDungeonLord)
            {
                //Debug.Log("Found Closer Tile Move to this instead" + tileChosen.gameObject.name);
                tileChosen = pathfinding.reachableTiles[i];
            }
        }

        pathfinding.desiredPosition = tileChosen;
        pathfinding.tilesToCheck.Add(pathfinding.desiredPosition);
        pathfinding.EstablishPossibleMoves("FindPath");

        while (actionDone == false)
        {
            yield return null;
        }

        actionsToTake -= 1;
        actionDone = false;
        WhichActionIsBrokenTracker = "None";
    }

    public void ResetCreatures()
    {
        for (int i = 0; i < myCreatures.Count; i++)
        {
            myCreatures[i].GetComponent<CreatureToken>().CreatureResetTurnEnd();
        }
    }


}
