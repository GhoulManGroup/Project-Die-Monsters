using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuMulltiplayer : MonoBehaviour
{
    [SerializeField] GameObject hostBTN;
    [SerializeField] GameObject joinClientBTN;

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
                break;
            case "Client":
                NetworkManager.Singleton.StartClient();
                break;
            case "Back":
                SceneManager.LoadScene("MenuMain");
                break;
        }
    }
}
