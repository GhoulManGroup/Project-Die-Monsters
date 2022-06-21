using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResourceUIManager : MonoBehaviour // This Script now tracks player resources & Controls the dice hand UI.
{
    [Header("Refrences")]
    public GameObject turnPlayer;

    [Header("UIElementLists")]
    public GameObject[] crestTextScore;
    public List<GameObject> DieMang = new List<GameObject>();
    public List<GameObject> BoardMang = new List<GameObject>();

    [Header("Varibles")]
    public string whatUItoShow = "Board"; // diceManagement // board // Menu

    [Header("ActionsTaken")] // these bool control when the player can and can't press buttons
    public bool hasMulliganed = false;


    // Start is called before the first frame update
    void Start()
    {
        DiceWindow();
        DiceWindowButtonControl();

    }

    public void Update()
    {
        endTurnBtnControll();
    }

    public void bugFix()
    {
        DieMang[1].GetComponent<Button>().interactable = true;
        DieMang[2].GetComponent<Button>().interactable = true;
        DieMang[3].GetComponent<Button>().interactable = true;
        DieMang[4].GetComponent<Button>().interactable = true;

        DieMang[1].GetComponent<Button>().interactable = false;
        DieMang[2].GetComponent<Button>().interactable = false;
        DieMang[3].GetComponent<Button>().interactable = false;
        DieMang[4].GetComponent<Button>().interactable = false;
    }
    public void buttonPressed()
    {

        string buttonChosen = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (buttonChosen)
        {
            case "DrawDiceBTN":
                this.GetComponent<DiceHandManager>().drawDice();
                DieMang[1].GetComponent<Button>().interactable = false;
                if (hasMulliganed == false)
                {
                    DieMang[2].GetComponent<Button>().interactable = true;
                }
                DieMang[3].GetComponent<Button>().interactable = true;
                this.GetComponent<DiceHandManager>().desiredDiceState = "Shown";
                this.GetComponent<DiceHandManager>().diceShow();
                break;

            case "MulliganBTN":
                    DieMang[1].GetComponent<Button>().interactable = true;
                    DieMang[2].GetComponent<Button>().interactable = false;
                    DieMang[3].GetComponent<Button>().interactable = false;
                    hasMulliganed = true;
                   this.GetComponent<DiceHandManager>().mulliganDice();
                    this.GetComponent<DiceHandManager>().desiredDiceState = "Hidden";
                   this.GetComponent<DiceHandManager>().diceShow();
                break;

            case "RollDiceBTN":
                if (hasMulliganed == false)
                {
                    DieMang[2].GetComponent<Button>().interactable = false;
                }
                DieMang[3].GetComponent<Button>().interactable = false;
                DieMang[4].GetComponent<Button>().interactable = true;
               this.GetComponent<DiceHandManager>().rollDice();
                break;

            case "CloseDiceWindowBTN":
                this.GetComponent<LevelController>().turnPlayerPerformingAction = false;
                this.GetComponent<DiceHandManager>().returnToPool(); // returns the remaning dice back to the dice pool.
                whatUItoShow = "Board";
                this.GetComponent<DiceHandManager>().desiredDiceState = "Hidden";
                this.GetComponent<DiceHandManager>().diceShow();
                DiceWindow();
                this.GetComponent<CameraController>().ActiveCam = "Alt";
                this.GetComponent<CameraController>().switchCamera();
                break;
        }

    
    }
    public void DiceWindow() // This function opens the player dice window. / Where the player can draw a hand of dice and roll them.
    {
        switch (whatUItoShow)
        {
            case "Board":
                for (int i = 0; i < DieMang.Count; i++)
                {
                    if (DieMang[i].GetComponent<Image>() != null)
                    {
                        DieMang[i].GetComponent<Image>().enabled = false;
                    }

                    if (DieMang[i].GetComponent<Text>() != null)
                    {
                        DieMang[i].GetComponent<Text>().enabled = false;
                    }
                }
                break;

            case "Dice":
                for (int i = 0; i < 5; i++)
                {
                    if (DieMang[i].GetComponent<Image>() != null)
                    {
                        DieMang[i].GetComponent<Image>().enabled = true;
                    }

                    if (DieMang[i].GetComponent<Text>() != null)
                    {
                        DieMang[i].GetComponent<Text>().enabled = true;
                    }
                }
                DiceWindowButtonControl();
                break;
        }
        
    }

    public void DiceWindowButtonControl() // This function turns all the diemang buttons to non interactable so only the desired buttons are active.
    {
        DieMang[1].GetComponent<Button>().interactable = false;
        DieMang[2].GetComponent<Button>().interactable = false;
        DieMang[3].GetComponent<Button>().interactable = false;
        DieMang[4].GetComponent<Button>().interactable = false;

    }

    public void updateCrests() // update the value displayed on the crest UI;
    {
        crestTextScore[0].GetComponent<Text>().text = turnPlayer.GetComponent<Player>().moveCrestPoints.ToString();
        crestTextScore[1].GetComponent<Text>().text = turnPlayer.GetComponent<Player>().attackCrestPoints.ToString();
        crestTextScore[2].GetComponent<Text>().text = turnPlayer.GetComponent<Player>().defenceCrestPoints.ToString();
        crestTextScore[3].GetComponent<Text>().text = turnPlayer.GetComponent<Player>().abiltyPowerCrestPoints.ToString();
    }

    public void updateLifeText()
    {
        BoardMang[1].GetComponent<Text>().text = this.GetComponent<LevelController>().whoseTurn;
    }

    public void endTurnBtnControll()
    {
      /*  if (this.GetComponent<LevelController>().whoseTurn != "Player")
        {
            BoardMang[2].GetComponent<Button>().interactable = false;
        }

        else if (this.GetComponent<LevelController>().whoseTurn == "Player")
        {
            if (this.GetComponent<LevelController>().turnPlayerPerformingAction == true)
            {
                BoardMang[2].GetComponent<Button>().interactable = false;
            }

            else if (this.GetComponent<LevelController>().turnPlayerPerformingAction == false)
            {
                BoardMang[2].GetComponent<Button>().interactable = true;
            }
        }
        */
    }
}
