using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDieScript : MonoBehaviour
{
    [Header("Refrence Objects")]
    GameObject levelController;
    GameObject myController;
    UIDiceController playerController;
    AIRollManager AIController;
    InspectWindowController inspectTab;

    public bool diceSetUp; // If the dice has a creature inside.
    public string rollResult; // What the dice has rolled.
    public bool canBeChosen = false; //If the dice can be picked to add to the creature pool.
    public bool isFree = false;
    public bool rolledLevelCrest = false;

    Rigidbody myBody;
    Vector3 startingPosition;

    public Die myDie; // What die scriptable object is assigned to this die at the moment.

    public List<GameObject> myParts = new List<GameObject>(); // This list stores all the componenets which form the game dice. 0 = cube // others are faces.
    public List<string> myFaceCrests = new List<string>(); // this list stores what each face is in sequence 1-6.

    //List of materials which the dice show depending on the die scrtipable object enum results.
    public Material startingMat;
    public List<Material> dieColors = new List<Material>();
    public List<Material> dieCrests = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        levelController = GameObject.FindGameObjectWithTag("LevelController");
        if (levelController.GetComponent<LevelController>().turnPlayerObject.GetComponent<Player>() != null)
        {
            myController = GameObject.FindGameObjectWithTag("LevelController");
            playerController = myController.GetComponent<UIDiceController>();
        }
        else if (levelController.GetComponent<LevelController>().turnPlayerObject.GetComponent<AIOpponent>() != null)
        {
            myController = GameObject.FindGameObjectWithTag("AIController");
            AIController = myController.GetComponent<AIRollManager>();
        }
        else
        {
            Debug.LogError("current turn particpant returning value other than expected 0 || 1 please investigate");
        }

        
        inspectTab = GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectWindowController>();
        myBody = GetComponent<Rigidbody>();
        startingPosition = this.transform.position;
        resetDie();
    }

    public void OnMouseDown()
    {
        if (myController.GetComponent<UIDiceController>() != null)
        {
            if (playerController.hasRolledDice == false)
            {
                inspectTab.sceneDice = gameObject;
                playerController.DiceSelectWindow();
            }

            if (playerController.hasRolledDice == true)
            {
                if (canBeChosen == true && isFree == true || canBeChosen == true && myDie.dieCreature.summonCost <= playerController.turnPlayer.GetComponent<Player>().summmonCrestPoints)
                {
                    inspectTab.sceneDice = gameObject;
                    inspectTab.OpenInspectWindow("DieInspect");
                    playerController.UIElements[2].SetActive(true);
                    playerController.UIElements[5].GetComponent<Button>().interactable = true;
                    playerController.UIElements[6].GetComponent<Button>().interactable = true;
                }
            }
        }
        else
        {
            Debug.Log("Hello From OMD");
        }
    }

    public void setUpDie()
    {
        // Reset the contents of my face crest list before we add the new ones.
        myFaceCrests.Clear();
        //Check each enum in MyDie, and add the results to mycrest list as a string.
        myFaceCrests.Add(myDie.firstCrest.ToString());
        myFaceCrests.Add(myDie.secondCrest.ToString());
        myFaceCrests.Add(myDie.thirdCrest.ToString());
        myFaceCrests.Add(myDie.forthCrest.ToString());
        myFaceCrests.Add(myDie.fifthCrest.ToString());
        myFaceCrests.Add(myDie.sixthCrest.ToString());

        //Check what crest is on each face of this die, then assign the right material from the crest list depending on the results.
        for (int i = 1; i < myParts.Count; i++)
        {
            switch (myFaceCrests[i - 1].ToString())
            {
                case "Level":
                    switch (myDie.dieCreatureLevel.ToString())
                    {
                        case "one":
                            myParts[i].GetComponent<MeshRenderer>().material = dieCrests[0];
                            break;
                        case "two":
                            myParts[i].GetComponent<MeshRenderer>().material = dieCrests[1];
                            break;
                        case "three":
                            myParts[i].GetComponent<MeshRenderer>().material = dieCrests[2];
                            break;
                        case "four":
                            myParts[i].GetComponent<MeshRenderer>().material = dieCrests[3];
                            break;
                    }
                    break;
                case "Attack":
                    myParts[i].GetComponent<MeshRenderer>().material = dieCrests[4];
                    break;
                case "Defence":
                    myParts[i].GetComponent<MeshRenderer>().material = dieCrests[5];
                    break;
                case "AP":
                    myParts[i].GetComponent<MeshRenderer>().material = dieCrests[6];
                    break;
                case "Move":
                    myParts[i].GetComponent<MeshRenderer>().material = dieCrests[7];
                    break;
            }
        }

        //Check the die color and apply the right material from the die color material list.
        switch (myDie.dieColor.ToString())
        {
            case "Red":
                myParts[0].GetComponent<MeshRenderer>().material = dieColors[0];
                break;
            case "Blue":
                myParts[0].GetComponent<MeshRenderer>().material = dieColors[1];
                break;
            case "Yellow":
                myParts[0].GetComponent<MeshRenderer>().material = dieColors[2];
                break;
            case "Green":
                myParts[0].GetComponent<MeshRenderer>().material = dieColors[3];
                break;
            case "White":
                myParts[0].GetComponent<MeshRenderer>().material = dieColors[4];
                break;
            case "Black":
                myParts[0].GetComponent<MeshRenderer>().material = dieColors[5];
                break;
        }

        if (diceSetUp == false)
        {
            diceSetUp = true;
        }

        if (myController.GetComponent<UIDiceController>() != null)
        {
            playerController.CheckAllDiceSetUp();
        }
    }

    public void spinDie()
    {          
        float dirX = Random.Range(0, 65);
        float dirY = Random.Range(0, 65);
        float dirZ = Random.Range(0, 65);
        transform.rotation = Quaternion.identity;
        myBody.AddForce(transform.up * 500);
        myBody.AddTorque(dirX, dirY, dirZ);
        StartCoroutine(waitToCheckDice());
    }

    IEnumerator waitToCheckDice()
    {
        //Every 3 seconds we check if the dice has stoped moving.
        yield return new WaitForSeconds(2f);
        rollStopCheck();
    }

    public void rollStopCheck()
    {
        if (this.transform.position != startingPosition)
        {
            // if we are still moving run courtine agian
            startingPosition = this.transform.position;
            StartCoroutine(waitToCheckDice());
        }
        else if (this.transform.position == startingPosition)
        {
            // if we have stoped moving then chekc results 
            checkResults();
        }

    }

    public void checkResults()
    {
        GameObject topFace = myParts[1];
        int result = 0;

        for (int i = 1; i < myParts.Count; i++)
        {
            if (myParts[i].transform.position.y > topFace.transform.position.y)
            {
                topFace = myParts[i];
                result = i - 1;
            }
        }

        switch (myFaceCrests[result].ToString())
        {
            //Depending on the crest result either add crests to the turn players crest pool or declare summon crest rolled in the dice controller script.
            case "Level":
                switch (myDie.dieCreatureLevel.ToString())
                {
                    case "one":
                        rollResult = "LC1";
                        if (myController.GetComponent<UIDiceController>()!= null)
                        {
                            playerController.lvl1Crest += 1;
                        }
                        else if (myController.GetComponent<UIDiceController>() != null)
                        {

                        }

                        break;
                    case "two":
                        if (myController.GetComponent<UIDiceController>() != null)
                        {
                            playerController.lvl2Crest += 1;
                        }
                        else if (myController.GetComponent<UIDiceController>() != null)
                        {

                        }
                        rollResult = "LC2";
                        break;
                    case "three":
                        if (myController.GetComponent<UIDiceController>() != null)
                        {
                            playerController.lvl3Crest += 1;
                        }
                        else if (myController.GetComponent<UIDiceController>() != null)
                        {

                        }
                        rollResult = "LC3";
                        break;
                    case "four":
                        if (myController.GetComponent<UIDiceController>() != null)
                        {
                            playerController.lvl4Crest += 1;
                        }
                        else if (myController.GetComponent<UIDiceController>() != null)
                        {

                        }
                        rollResult = "LC4";
                        break;
                }
                rolledLevelCrest = true;
                canBeChosen = true;
                if (myController.GetComponent<UIDiceController>() != null)
                {
                    playerController.summonCrestPool += 1;
                }
                else if (myController.GetComponent<UIDiceController>() != null)
                {

                }
                break;
            case "Attack":
                rollResult = "Attack";
                if (myController.GetComponent<UIDiceController>() != null)
                {
                    playerController.turnPlayer.GetComponent<Player>().attackCrestPoints += 1;
                }
                else if (myController.GetComponent<UIDiceController>() != null)
                {

                }
                break;
            case "Defence":
                rollResult = "Defence";
                if (myController.GetComponent<UIDiceController>() != null)
                {
                    playerController.turnPlayer.GetComponent<Player>().defenceCrestPoints += 1;
                }
                else if (myController.GetComponent<UIDiceController>() != null)
                {

                }
                break;
            case "AP":
                rollResult = "AP";
                if (myController.GetComponent<UIDiceController>() != null)
                {
                    playerController.turnPlayer.GetComponent<Player>().abiltyPowerCrestPoints += 1;
                }
                else if (myController.GetComponent<UIDiceController>() != null)
                {

                }    
                break;
            case "Move":
                rollResult = "Move";
                if (myController.GetComponent<UIDiceController>() != null)
                {
                    playerController.turnPlayer.GetComponent<Player>().moveCrestPoints += 1;
                }
                else if (myController.GetComponent<UIDiceController>() != null)
                {

                }
                break;
        }

        if (myController.GetComponent<UIDiceController>() != null)
        {
            playerController.dicechecked += 1;
            playerController.CheckCanSummonCreature();
        }
        else if (myController.GetComponent<UIDiceController>() != null)
        {

        }
    }

    public void resetDie()
    {
        //reset the dice position
        transform.position = startingPosition;
        canBeChosen = false;
        isFree = false;

        //When the dice are done we reset the material.
        for (int i = 0; i < myParts.Count; i++)
        {
            myParts[i].GetComponent<MeshRenderer>().material = startingMat;
        }
    }
}
