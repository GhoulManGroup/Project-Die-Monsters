using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera BoardView;
    public Camera AltView;
    public string ActiveCam = "Board";

    public GameObject AltCamRotatePoint;

    // Start is called before the first frame update
    void Start()
    {
        switchCamera();
      

    }

    public void Update()
    {
        MoveCamera();
    }

    // Update is called once per frame
    public void switchCamera()
    {
        if (ActiveCam == "Board")
        {
            BoardView.enabled = true;
            AltView.enabled = false;
        }else if (ActiveCam == "Alt")
        {
            BoardView.enabled = false;
            AltView.enabled = true;
        }
    }

    public void MoveCamera()
    {
        if (ActiveCam == "Alt")
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
