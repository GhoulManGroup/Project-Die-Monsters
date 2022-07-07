using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureToken : MonoBehaviour
{
    public Creature myCreature; // what scriptable object do I pull my details from.
    public GameObject myArtSlot; // what material to display
    public GameObject myBoardLocation; // the board tile I am Above.

    GameObject LvlRef;

    [Header("Creature Varibles")] // creature stats.
    public int health;
    public int attack;
    public int defence;

    public int moveCost = 1; // how many move crests per tile.
    public int attackDistance = 1; // how far we can attack.
    public int attackCost = 1; // how much to attack.
    public int abilityCost; // how much does the ability of creature cost.
    public string myOwner; // who owns this piece.

    [Header("Creature Action Checks")]
    public bool HasAttackedThisTurn = false; //A creature may only attack once this turn unless an ability specifies otherwise.
    public bool HasMovedThisTurn; //A creature may only move once per turn unless an abilty specfies otherwise.
    public bool hasUsedAbilityThisTurn; // A creature may only use their ability once per turn unless an ability specfies otherwise.
    public bool canReachTarget = false;

    [Header("CreatureCombat")]
    public List<GameObject> targets = new List<GameObject>();


    void Start()
    {
        //Set level ref to level controler object.
        LvlRef = GameObject.FindGameObjectWithTag("LevelController");

        //set my owner to either player+ playerslotnumber or AI.
        myOwner = LvlRef.GetComponent<LevelController>().whoseTurn;

        //Check for either a player script or opponent script then pull the desired creature from the correct objects creaturelist and assign it to the creature piece.
        if (LvlRef.GetComponent<LevelController>().participants[LvlRef.GetComponent<LevelController>().turnPlayer].GetComponent<Player>() != null) 
        {
            // set my creature to be the same scriptble object that was chosen from the creaturePoolControllerList, then run the creature played fuciton to remove that creature from said list.
            myCreature = LvlRef.GetComponent<CreaturePoolController>().turnPlayer.GetComponent<Player>().CreaturePool[LvlRef.GetComponent<CreaturePoolController>().creaturePick];

            LvlRef.GetComponent<CreaturePoolController>().creaturePlayed();
            LvlRef.GetComponent<CreatureController>().CreaturesOnBoard.Add(this.gameObject);
        }

        if (myOwner == "AI") // change to opponent
        {

        }

       setDetails();
       declareTile();
    }

    public void setDetails()
    {       
        myArtSlot.GetComponent<MeshRenderer>().material = myCreature.cardArt3D;
        health = myCreature.Health;
        attack = myCreature.Attack;
        defence = myCreature.Defence;
    }

    public void OnMouseDown() // assign this as active creature target. Switch Camera State to Board View. 
    {
        if (myOwner == LvlRef.GetComponent<LevelController>().whoseTurn)
        {
            if (LvlRef.GetComponent<CreatureController>().canPickPiece == true)
            {
                LvlRef.GetComponent<CameraController>().ActiveCam = "Board";
                LvlRef.GetComponent<CameraController>().switchCamera();
                LvlRef.GetComponent<CreatureController>().ChosenAction = "Choosing";
                LvlRef.GetComponent<CreatureController>().ChosenCreature = this.gameObject;
                LvlRef.GetComponent<CreatureController>().canPickPiece = false;
                LvlRef.GetComponent<CreatureController>().HideAndShowButtons();
                CheckForAttackTarget();
            }
        }
        
    }

    public void declareTile() // declare where we are.
    {
        RaycastHit Down;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out Down, 1f))
        {
            myBoardLocation = Down.collider.gameObject;
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

                            if (Forward.collider.GetComponent<DungeonLord>() != null)
                            {
                                if (Forward.collider.GetComponent<DungeonLord>().myOwner != myOwner)
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

                            if (Back.collider.GetComponent<DungeonLord>() != null)
                            {
                                if (Back.collider.GetComponent<DungeonLord>().myOwner != myOwner)
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

                            if (Up.collider.GetComponent<DungeonLord>() != null)
                            {
                                if (Up.collider.GetComponent<DungeonLord>().myOwner != myOwner)
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

                            if (Down.collider.GetComponent<DungeonLord>() != null)
                            {
                                if (Down.collider.GetComponent<DungeonLord>().myOwner != myOwner)
                                {
                                    targets.Add(Down.collider.gameObject);
                                }
                            }
                        }
                    }
                    break;
            }          

        }

        if (targets.Count != 0)
        {
            canReachTarget = true;
        }
    } // raycast adjacent tiles for enemy creature pieces,

    public void CheckState()
    {
        if (health <= 0)
        {
            LvlRef.GetComponent<CreatureController>().CreaturesOnBoard.Remove(this.gameObject);
            Destroy(this.gameObject);
            Debug.Log("Creature Dead");
        }
    }
    
}
