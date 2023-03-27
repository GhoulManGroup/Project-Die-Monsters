using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera BoardView;
    public Camera DiceView;
    public Camera AltView;
    public GameObject AltCamRotatePoint;
    bool EnableCamMovement;
    public string currentCam;

    // Start is called before the first frame update
    void Start()
    {
        switchCamera("Board");
    }

    public void Update()
    {
        MoveCamera();
    }

    // Update is called once per frame
    public void switchCamera(string ActiveCam)
    {
        Debug.Log("Hello From Me" + ActiveCam.ToString());
        switch (ActiveCam)
        {
            case "Board": // the top down birds eye view
                BoardView.enabled = true;
                DiceView.enabled = false;
                AltView.enabled = false;
                break;
            case "Dice": // the view of the dice rolling space.
                BoardView.enabled = false;
                DiceView.enabled = true;
                AltView.enabled = false;
                break;
            case "Alt": // the angled view of the board with limited movement < left , right >
                BoardView.enabled = false;
                DiceView.enabled = false;
                AltView.enabled = true;
                EnableCamMovement = true;
                break;
        }
    }

    public void MoveCamera()
    {
        if (EnableCamMovement == true)
        {
            if (Input.GetKey("z"))
            {
                AltView.transform.RotateAround(AltCamRotatePoint.transform.position, Vector3.up, 80 * Time.deltaTime);
            }

            if (Input.GetKey("c"))
            {
                AltView.transform.RotateAround(AltCamRotatePoint.transform.position, Vector3.up, -80 * Time.deltaTime);
            }
        }
    }


}
