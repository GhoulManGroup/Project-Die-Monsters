using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureToken : MonoBehaviour
{
    public Creature myCreature; // what scriptable object do I pull my details from.
    public GameObject myArtSlot; // what material to display
    public GameObject myBoardLocation; // the board tile I am Above.

    GameObject lvlRef;
    LevelController lcScript;

    [Header("Creature Varibles")] // creature stats.
    public int currentHealth;
    public int currentAttack;
    public int currentDefence;


    public int moveCost = 1; // how many move crests per tile.
    public int attackDistance = 1; // how far we can attack.
    public int attackCost = 1; // how much to attack.
    public int abilityCost = 1; // how much does the ability of creature cost.
    public string myOwner; // who owns this piece.

    public string facingDirection = "Right";

    [Header("Creature Action Checks")]
    public bool hasAttackedThisTurn = false; //A creature may only attack once this turn unless an ability specifies otherwise.
    public bool hasMovedThisTurn; //A creature may only move once per turn unless an abilty specfies otherwise.
    public bool hasUsedAbilityThisTurn; // A creature may only use their ability once per turn unless an ability specfies otherwise.
    public bool canReachTarget = false;

    [Header("CreatureCombat")]
    public List<GameObject> targets = new List<GameObject>();


    void Start()
    {
        //Set level ref to level controler object.
        lvlRef = GameObject.FindGameObjectWithTag("LevelController");
        lcScript = lvlRef.GetComponent<LevelController>();  
        myOwner = lcScript.whoseTurn;  //set my owner to either player+ playerslotnumber or AI.

        //Check for either a player script or opponent script then pull the desired creature from the correct objects creaturelist and assign it to the creature piece. 
        if (lcScript.participants[lcScript.turnPlayerSlot].GetComponent<Player>() != null)
        {
            if (lcScript.creaturePlacedFrom == "CreaturePool") 
            {
                // set my creature to be the same scriptble object that was chosen from the creaturePoolControllerList, then run the creature played fuciton to remove that creature from said list.
                myCreature = lvlRef.GetComponent<CreaturePoolController>().currentCreature;
                lvlRef.GetComponent<CreaturePoolController>().creaturePlayed();
            }
            
            else if (lcScript.creaturePlacedFrom == "DiceBoard")
            {
                //If played directly from dice board area instead creature is current creature stored in inspect tab, 
                myCreature = GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>().currentCreature;
            }

            lvlRef.GetComponent<CreatureController>().CreaturesOnBoard.Add(this.gameObject); // add this piece to the creature controller script.
            lvlRef.GetComponent<LevelController>().turnPlayerPerformingAction = false; // piece placed player no longer performing action.
            lvlRef.GetComponent<LevelController>().ableToInteractWithBoard = true; // Player can now interact with the board.
        }

        if (myOwner == "AI") // change to opponent
        {

        }
        this.GetComponent<AbilityManager>().myAbility = this.myCreature.myAbility;

        setDetails();
       declareTile("Start");
    }

    public void setDetails()
    { // This function sets all the details of the piece token on the board, once it has had its creature sciptable object assigned from where ever it is being summoned from.
        this.gameObject.name = myCreature.name;
        myArtSlot.GetComponent<MeshRenderer>().material = myCreature.cardArt3D;
        currentHealth = myCreature.Health;
        currentAttack = myCreature.Attack;
        currentDefence = myCreature.Defence;
        abilityCost = myCreature.myAbility.abilityCost;
    }


    public void declareTile(string why) // declare where we are.
    {
        RaycastHit Down;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Down, 1f))
        {
            myBoardLocation = Down.collider.gameObject;
            if (why == "Start") // Added string so that if I want traps in future can make a check here to trigger effect while creature moves across the board.
            {
                Down.collider.GetComponent<GridScript>().TileContents = "Creature";
            }
        }
    }

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
    } // raycast adjacent tiles for enemy creature pieces,

    public void OnMouseDown()
    {
        if (lcScript.ableToInteractWithBoard == true)
        {
            switch (lcScript.boardInteraction)
            {
                case "TargetPick":
                    Debug.Log("Picking");
                    //Find the creature casting its ability in the ability UI Script, if we are a valid target pass this creature through to see if we can be added to declared creature list.
                    TargetManager castingCreature = GameObject.FindGameObjectWithTag("AbilityWindow").GetComponent<AbilityUIController>().currentCreature.GetComponent<TargetManager>();
                    if (castingCreature.targetPool.Contains(this.gameObject))
                    {
                        castingCreature.CheckDeclareState(this.gameObject);
                    }
                    break;

                case "None":
                    if (myOwner == lvlRef.GetComponent<LevelController>().whoseTurn)
                    {
                        myBoardLocation.GetComponent<GridScript>().IsAnyMovementPossible();
                        lvlRef.GetComponent<CameraController>().switchCamera("Board");
                        lvlRef.GetComponent<CreatureController>().ChosenAction = "None";
                        lvlRef.GetComponent<CreatureController>().ChosenCreatureToken = this.gameObject;
                        lcScript.ableToInteractWithBoard = false;
                        lvlRef.GetComponent<CreatureController>().OpenAndCloseControllerUI();
                        CheckForAttackTarget();
                    }
                    break;
            }
        }else
        {
            // Do Nothing
        }
    }

    public void CheckState()
    {
        if (currentHealth <= 0)
        {
            myBoardLocation.GetComponent<GridScript>().TileContents = "Empty";
            lvlRef.GetComponent<CreatureController>().CreaturesOnBoard.Remove(this.gameObject);
            Destroy(this.gameObject);
            Debug.Log("Creature Dead");
        }
    }

}
