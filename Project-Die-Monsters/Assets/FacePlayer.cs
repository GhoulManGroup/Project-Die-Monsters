using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    Camera mainCamera;
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CameraController>().AltView;
    }
    void LateUpdate()
    {
        transform.rotation = mainCamera.transform.rotation;
    }
}
