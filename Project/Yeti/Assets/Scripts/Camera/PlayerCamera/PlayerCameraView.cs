using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraView : MonoBehaviour
{
    private Camera playerCamera;
    private PlayerCameraMovement cameraMovement;
   
    public Camera Camera {get {return playerCamera;}}
    public PlayerCameraMovement GetCameraMovement { get {return cameraMovement;}}    

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();
        cameraMovement = GetComponent<PlayerCameraMovement>();
    }
}
