using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDieScript : MonoBehaviour
{
    [Header("Refrence Objects")]
    UIDiceController myController;
    InspectTabScript inspectTab;

    public bool diceSetUp; // If the dice has a creature inside.
    public string rollResult; // What the dice has rolled.
    public bool canBeChosen = false; //If the dice can be picked to add to the creature pool.

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
        myController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<UIDiceController>();
        inspectTab = GameObject.FindGameObjectWithTag("InspectWindow").GetComponent<InspectTabScript>();
        myBody = GetComponent<Rigidbody>();
        startingPosition = this.transform.position;
        resetDie();
    }

    public void OnMouseDown()
    {
        if (myController.hasRolledDice == false)
        {
            //declare this as first dice pick then open window.
            inspectTab.sceneDice = gameObject;
            inspectTab.usedFor = "DrawDice";
            inspectTab.ShowFunction();
        }

        if (myController.hasRolledDice == true)
        {
            if (canBeChosen == true)
            {
                //Check if dice already done.
                inspectTab.sceneDice = gameObject;
                inspectTab.usedFor = "DieInspect";
                inspectTab.ShowFunction();
            }
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
            switch (myFaceCrests[i-1].ToString())
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

        if(diceSetUp == false)
        {
            diceSetUp = true;
        }
    }

    public void spinDie()
    {          
        float dirX = Random.RandomRange(0, 65);
        float dirY = Random.RandomRange(0, 65);
        float dirZ = Random.RandomRange(0, 65);
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
                        myController.lvl1Crest += 1;
                        break;
                    case "two":
                        rollResult = "LC2";
                        myController.lvl2Crest += 1;
                        break;
                    case "three":
                        rollResult = "LC3";
                        myController.lvl3Crest += 1;
                        break;
                    case "four":
                        rollResult = "LC4";
                        myController.lvl4Crest += 1;
                        break;
                }
                break;
            case "Attack":
                rollResult = "Attack";
                myController.turnPlayer.GetComponent<Player>().attackCrestPoints += 1;
                break;
            case "Defence":
                rollResult = "Defence";
                myController.turnPlayer.GetComponent<Player>().defenceCrestPoints += 1;
                break;
            case "AP":
                rollResult = "AP";
                myController.turnPlayer.GetComponent<Player>().abiltyPowerCrestPoints += 1;
                break;
            case "Move":
                rollResult = "Move";
                myController.turnPlayer.GetComponent<Player>().moveCrestPoints += 1;
                break;
        }
        myController.dicechecked += 1;
        myController.matchingSummonCrestCheck();
    }

    public void resetDie()
    {
        //reset the dice position
        transform.position = startingPosition;

        //When the dice are done we reset the material.
        for (int i = 0; i < myParts.Count; i++)
        {
            myParts[i].GetComponent<MeshRenderer>().material = startingMat;
        }
    }


}
