using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class CreatureToken : MonoBehaviour
{
    public Creature myCreature; // what scriptable object do I pull my details from.
    public GameObject myArtSlot; // what material to display
    public GameObject myBoardLocation; // the board tile I am Above.

    GameObject lvlRef;
    LevelController lcScript;

    [Header("Creature Varibles")] 
    //Current Creature Values
    public int currentHealth;
    public int currentAttack;
    public int currentDefence;
    public int currentMoveDistance;

    //Creature Starting / Constant Values
    public int healthCap;
    public int attackCap;
    public int defenceCap;
    public int moveDistanceCap;

    //Action Costs
   // public int moveCost = 1; // how many move crests per it costs to move per tile of distance on the board crest no longer in use.
    public int attackDistance = 1; // how far we can attack.
    public int attackCost = 1; // how much to attack.
    public int abilityCost = 1; // how much does the ability of creature cost to cast.

    //Creature Owner
    public string myOwner;


    [Header("Directions")]
    public string facingDirection = "East";
    public string behindDirection;
    public string sideDirection;
    public string otherSideDirection;

    [Header("Creature Action Checks")]
    public bool hasAttackedThisTurn = false; //A creature may only attack once this turn unless an ability specifies otherwise.
    public bool hasMovedThisTurn; //A creature may only move once per turn unless an abilty specfies otherwise.
    public bool hasUsedAbilityThisTurn; // A creature may only use their ability once per turn unless an ability specfies otherwise.
    public bool canReachTarget = false;

    [Header("CreatureCombat")]
    public List<GameObject> targets = new List<GameObject>();
    internal GameObject startPosition;

    void Start()
    {
        //Set level ref to level controler object.
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
        lcScript = lvlRef.GetComponent<LevelController>();
        myOwner = lcScript.currentTurnParticipant.ToString();  //set my owner to either player+ playerslotnumber or AI.

        //Check for either a player script or opponent script then pull the desired creature from the correct objects creaturelist and assign it to the creature piece. 
        if (lcScript.participants[lcScript.currentTurnParticipant].GetComponent<Player>() != null)
        {
            /*
            if (lcScript.creaturePlacedFrom == "CreaturePool") //Cut From current Build as an option due to slow game pacing
            {
               // set my creature to be the same scriptble object that was chosen from the creaturePoolControllerList, then run the creature played fuciton to remove that creature from said list.
              myCreature = lvlRef.GetComponent<CreaturePoolController>().currentCreature;
               lvlRef.GetComponent<CreaturePoolController>().creaturePlayed();
            }*/

           if (lcScript.creaturePlacedFrom == "DiceBoard")
           {
                //If played directly from dice board area instead creature is current creature stored in inspect tab, 
                myCreature = GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().currentCreature;
           }

            lvlRef.GetComponent<PlayerCreatureController>().CreaturesOnBoard.Add(this.gameObject); // add this piece to the creature controller script.
            lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false; // piece placed player no longer performing action.
            lvlRef.GetComponent<LevelController>().ableToInteractWithBoard = true; // Player can now interact with the board.
            lvlRef.GetComponent<LevelController>().CanEndTurn();
        }

        if (myOwner == "AI") // change to opponent
        {

        }

        this.GetComponent<AbilityManager>().myAbility = this.myCreature.myAbility;

        setDetails();
        FindTileBellowMe("Start");
    }

    // This function sets all the details of the piece token on the board, once it has had its creature sciptable object assigned.
    public void setDetails()
    { 
        this.gameObject.name = myCreature.CreatureName;

        //Swap this back to 3D model when we switch from placeholders billboard sprties to final creature 3d models.
        myArtSlot.GetComponent<Image>().sprite = myCreature.CardArt;
        currentHealth = myCreature.Health;
        currentAttack = myCreature.Attack;
        currentDefence = myCreature.Defence;
        currentMoveDistance= myCreature.MoveDistance;
        healthCap = myCreature.Health;
        attackCap = myCreature.Attack;
        defenceCap = myCreature.Defence;
        moveDistanceCap= myCreature.MoveDistance;

        if (myCreature.myAbility != null)
        {
            abilityCost = myCreature.myAbility.abilityCost;
        }
        setDirections();
    }

    public void setDirections()
    {
        switch (facingDirection)
        {
            case "North":
                behindDirection = "South";
                sideDirection = "East";
                otherSideDirection = "West";
                break;
            case "South":
                behindDirection = "North";
                sideDirection = "West";
                otherSideDirection = "East";
                break;
            case "West":
                behindDirection = "East";
                sideDirection = "North";
                otherSideDirection = "South";
                break;
            case "East":
                behindDirection = "West";
                sideDirection = "South";
                otherSideDirection = "North";
                break;
        }
    }

    //Assign which tile game object this creature token is above on the game board.
    public void FindTileBellowMe(string why) 
    {
        RaycastHit Down;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Down, 1f))
        {
            if (why == "Start" || why == "Move") // Added string so that if I want traps in future can make a check here to trigger effect while creature moves across the board.
            {
                if (myBoardLocation != null)
                {
                    myBoardLocation.GetComponent<GridScript>().TileContents = "Empty";
                    myBoardLocation.GetComponent<GridScript>().creatureAboveMe = null;
                }
                myBoardLocation = Down.collider.gameObject;
                Debug.Log(Down.collider.gameObject);
                Down.collider.GetComponent<GridScript>().TileContents = "Creature";
                Down.collider.GetComponent<GridScript>().creatureAboveMe = this.gameObject;
            }
        }
    }

    //Raycast in adjacent directions to find any other creature tokens that arent yours and add those to the possible attack target list
    public void CheckForAttackTarget()
    {
        targets.Clear(); // empty the list of any existing targets then add new ones.

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {                
                case 0:
                    RaycastHit Forward;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Forward, attackDistance))
                    {
                        if (Forward.collider != null) // check to see if we hit anything
                        {
                            if (Forward.collider.GetComponent<CreatureToken>() != null)
                            {
                                if (Forward.collider.GetComponent<CreatureToken>().myOwner != myOwner)
                                {
                                    targets.Add(Forward.collider.gameObject);
                                }
                            }

                            if (Forward.collider.GetComponent<DungeonLordPiece>() != null)
                            {
                                if (Forward.collider.GetComponent<DungeonLordPiece>().myOwner != myOwner)
                                {
                                    targets.Add(Forward.collider.gameObject);
                                }
                            }
                        }
                    }                  
                        break;

                case 1:
                    RaycastHit Back;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out Back, attackDistance))
                    {
                        if (Back.collider != null) // check to see if we hit anything
                        {
                            if (Back.collider.GetComponent<CreatureToken>() != null)
                            {
                                if (Back.collider.GetComponent<CreatureToken>().myOwner != myOwner)
                                {
                                    targets.Add(Back.collider.gameObject);
                                }
                            }

                            if (Back.collider.GetComponent<DungeonLordPiece>() != null)
                            {
                                if (Back.collider.GetComponent<DungeonLordPiece>().myOwner != myOwner)
                                {
                                    targets.Add(Back.collider.gameObject);
                                }
                            }
                        }
                    }
                    break;

                case 2:
                    RaycastHit Up;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out Up, attackDistance))
                    {
                        if (Up.collider != null) // check to see if we hit anything
                        {
                            if (Up.collider.GetComponent<CreatureToken>() != null)
                            {
                                if (Up.collider.GetComponent<CreatureToken>().myOwner != myOwner)
                                {
                                    targets.Add(Up.collider.gameObject);
                                }
                            }

                            if (Up.collider.GetComponent<DungeonLordPiece>() != null)
                            {
                                if (Up.collider.GetComponent<DungeonLordPiece>().myOwner != myOwner)
                                {
                                    targets.Add(Up.collider.gameObject);
                                }
                            }
                        }
                    }
                    break;
                case 3:
                    RaycastHit Down;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out Down, attackDistance))
                    {
                        if (Down.collider != null) // check to see if we hit anything
                        {
                            if (Down.collider.GetComponent<CreatureToken>() != null)
                            {
                                if (Down.collider.GetComponent<CreatureToken>().myOwner != myOwner)
                                {
                                    targets.Add(Down.collider.gameObject);
                                }
                            }

                            if (Down.collider.GetComponent<DungeonLordPiece>() != null)
                            {
                                if (Down.collider.GetComponent<DungeonLordPiece>().myOwner != myOwner)
                                {
                                    targets.Add(Down.collider.gameObject);
                                }
                            }
                        }
                    }
                    break;
            }          

        }

        //Debug.Log("Targets Found " + targets.Count);

        if (targets.Count != 0)
        {
            canReachTarget = true;
        }
    } 


    public void OnMouseDown()
    {
        if (lcScript.ableToInteractWithBoard == true)
        {
            switch (lcScript.boardInteraction)
            {
                case "AOETarget":

                    break;

                case "TargetPick":
                    //Find the creature casting its ability in the ability UI Script, if we are a valid target pass this creature through to see if we can be added to declared creature list.
                    TargetManager castingCreature = GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().currentCreature.GetComponent<TargetManager>();

                    if (castingCreature.targetPool.Contains(this.gameObject))
                    {
                        castingCreature.CheckDeclareState(this.gameObject);
                    }
                    break;

                case "None":
                    if (myOwner == lvlRef.GetComponent<LevelController>().currentTurnParticipant.ToString())
                    {
                        myBoardLocation.GetComponent<GridScript>().IsAnyMovementPossible();
                        lvlRef.GetComponent<CameraController>().switchCamera("Board");
                        lvlRef.GetComponent<PlayerCreatureController>().ChosenAction = "None";
                        lvlRef.GetComponent<PlayerCreatureController>().ChosenCreatureToken = this.gameObject;
                        lcScript.ableToInteractWithBoard = false;
                        lvlRef.GetComponent<PlayerCreatureController>().OpenAndCloseControllerUI();
                        CheckForAttackTarget();
                    }
                    break;
            }
        }else
        {
            // Do Nothing
        }
    }

    public IEnumerator CheckCreatureHealth()
    {// Used to detemine if the creature is dead or to remove any overhealing.
        if (currentHealth > healthCap)
        {
            currentHealth = healthCap;
        }

        if (currentHealth <= 0)
        {
            myBoardLocation.GetComponent<GridScript>().TileContents = "Empty";
            myBoardLocation.GetComponent<GridScript>().creatureAboveMe = null;

            if (lvlRef.GetComponent<PlayerCreatureController>().ChosenCreatureToken == this.gameObject)
            {
                lvlRef.GetComponent<PlayerCreatureController>().CancleBTNFunction();
                Debug.Log("Chosen Creature Died");
            }

            lvlRef.GetComponent<PlayerCreatureController>().CreaturesOnBoard.Remove(this.gameObject);
            Destroy(this.gameObject);
        }

        yield return null;
    }

    public void CreatureResetTurnEnd()
    {
        hasAttackedThisTurn = false;
        hasMovedThisTurn = false;
        hasUsedAbilityThisTurn = false;

        currentAttack = attackCap;
        currentDefence = defenceCap;
    }
}
