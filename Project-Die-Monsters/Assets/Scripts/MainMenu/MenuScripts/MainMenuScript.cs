using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    string ActiveUI = "MainMenu";
    string gameMode = "QuickPlay";
    string opponentType;

    GameObject MRef;

    public GameObject MenuMain;
    public GameObject deckPick;

    // Start is called before the first frame update
    void Start()
    {
        MRef = GameObject.FindGameObjectWithTag("GameController");
        showWhichUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuBTNPressed()
    {
        string BTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (BTNPressed)
        {
            case "RogueLike":
                print("In Development");
                break;
            case "QuickPlay":
                MRef.GetComponent<DeckManager>().decksInPlay.Add(0);
                MRef.GetComponent<DeckManager>().decksInPlay.Add(0);
                MRef.GetComponent<GameManagerScript>().gameMode = "Freeplay";
                MRef.GetComponent<GameManagerScript>().desiredOpponent = "Player";
                SceneManager.LoadScene("BoardScene");
                showWhichUI();
                break;
            case "Multiplayer":
                print("In Development");
                break;
            case "DeckBuilder":
                SceneManager.LoadScene("DeckBuilder");
                break;
            case "Quit":
                print("In Development");
                break;
            case "Back":
                switch (ActiveUI)
                {
                    case "OpponentPick":
                        ActiveUI = "MainMenu";
                        showWhichUI();
                        break;
                    case "DeckSelect":
                        ActiveUI = "OpponentPick";
                        showWhichUI();
                        break;
                }
                break;
            case "PVP":
                ActiveUI = "DeckSelect";
                opponentType = "Player";
                showWhichUI();
                break;
            case "PVE":
                ActiveUI = "DeckSelect";
                opponentType = "AI";
                showWhichUI();
                break;
            case "DeckOne":
                
                break;
            case "DeckTwo":

                break;
            case "DeckThree":

                break;
            case "DeckFour":

                break;
            case "DeckFive":

                break;
            case "Play":

                break;
        }
    }

    public void showWhichUI()
    {
        MenuMain.active = false;
        deckPick.active = false;

        switch (ActiveUI)
        {
            case "MainMenu":
                MenuMain.active = true;
                break;
            case "DeckSelect":
                deckPick.active = true;
                break;
        }
    }
}
