using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuMulltiplayer : MonoBehaviour
{
    /*
    [SerializeField] GameObject hostBTN;
    [SerializeField] GameObject beginBTN;
    [SerializeField] GameObject joinClientBTN;

    GameObject MRef;
    public void Awake()
    {
        MRef = GameManagerScript.instance.gameObject;
    }
    public void BTNPressed()
    {
        string BTNPressed = EventSystem.current.currentSelectedGameObject.name.ToString();

        switch (BTNPressed)
        {
            case "Server":
                NetworkManager.Singleton.StartServer();
                break;
            case "Host":
                NetworkManager.Singleton.StartHost();
                GameManagerScript.instance.gameMode = "Multiplayer";
                MRef.GetComponent<DeckManager>().decksInPlay.Add(0);
                break;
            case "Client":
                NetworkManager.Singleton.StartClient();
                GameManagerScript.instance.gameMode = "Multiplayer";
                MRef.GetComponent<DeckManager>().decksInPlay.Add(0);
                break;
            case "Back":
                LoadingManager.loadingManager.LoadSceneAdditive("MenuMain");
                break;
            case "Begin":
                if (IsHost)
                {
                    NetworkManager.SceneManager.LoadScene("GameBoardScene", LoadSceneMode.Additive);
                    Scene unloadMe = SceneManager.GetSceneByName("MenuMultiplayer");
                    NetworkManager.SceneManager.UnloadScene(unloadMe);
                    //LoadingManager.loadingManager.LoadSceneAdditive("GameBoardScene");
                }
                break;
        }
    }
    */
}
