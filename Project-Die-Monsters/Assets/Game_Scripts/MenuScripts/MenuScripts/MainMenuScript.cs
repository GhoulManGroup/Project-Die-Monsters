using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    string ActiveUI = "MainMenu";
    string gameMode = "QuickPlay";
    string opponentType = null;

    GameObject MRef;
    public Image dUMMY;

    public GameObject MenuMain;
    public GameObject deckPick;

    [Header("UI Elements")]
    public List<GameObject> UIPanels = new List<GameObject>();

    // Start is called before the first frame update
    
    IEnumerator Start()
    {
        MRef = GameObject.FindGameObjectWithTag("GameController");

         UIPanels[0].GetComponent<CanvasGroup>().alpha = 0f;

       // while (LoadingManager.loadingManager.isLoaded == false)
       // {
           yield return null;
       // }

        StartCoroutine(Fade(null, false));
    }
    
    IEnumerator Fade(GameObject fadeThis, bool fadeIn)
    {

        dUMMY.GetComponent<Image>().CrossFadeAlpha(0f, 5f, false);
        if (fadeIn == true) 
        {

        }

        else if (fadeIn == false)
        {

        }

        yield return null;
    }


    public void MenuBTNPressed()
    {
        string BTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (BTNPressed)
        {
            case "RogueLike":
                MRef.GetComponent<DeckManager>().decksInPlay.Add(0);
                MRef.GetComponent<GameManagerScript>().gameMode = "RogueLike";
                LoadingManager.loadingManager.LoadSceneAdditive("GameRogueLike", true);
                LoadingManager.loadingManager.UnloadScene("MenuMain");
                break;
            case "QuickPlay":
                MRef.GetComponent<DeckManager>().decksInPlay.Add(0);
                MRef.GetComponent<GameManagerScript>().gameMode = "Freeplay";
                MRef.GetComponent<GameManagerScript>().desiredOpponent = "Player";
                LoadingManager.loadingManager.LoadSceneAdditive("GameBoardScene",true);
                LoadingManager.loadingManager.UnloadScene("MenuMain");
                //showWhichUI();
                break;
            case "Multiplayer":
                Debug.Log("In Development");
                //LoadingManager.loadingManager.LoadSceneAdditive("MenuMultiplayer");
                //LoadingManager.loadingManager.UnloadScene("MenuMain");
                break;
            case "DeckBuilder":
                LoadingManager.loadingManager.LoadSceneAdditive("MenuDeckBuilder");
                LoadingManager.loadingManager.UnloadScene("MenuMain");
                break;
            case "AI":
                MRef.GetComponent<DeckManager>().decksInPlay.Add(0);
                MRef.GetComponent<GameManagerScript>().gameMode = "Freeplay";
                MRef.GetComponent<GameManagerScript>().desiredOpponent = "AI";
                LoadingManager.loadingManager.LoadSceneAdditive("GameBoardScene", true);
                LoadingManager.loadingManager.UnloadScene("MenuMain");
                break;
            case "Quit":
                Application.Quit();
                break;
            case "Back":
                switch (ActiveUI)
                {
                    case "OpponentPick":
                        ActiveUI = "MainMenu";
                       // showWhichUI();
                        break;
                    case "DeckSelect":
                        ActiveUI = "OpponentPick";
                        //showWhichUI();
                        break;
                }
                break;
            case "PVP":
                ActiveUI = "DeckSelect";
                opponentType = "Player";
               // showWhichUI();
                break;
            case "PVE":
                ActiveUI = "DeckSelect";
                opponentType = "AI";
                //showWhichUI();
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
}
