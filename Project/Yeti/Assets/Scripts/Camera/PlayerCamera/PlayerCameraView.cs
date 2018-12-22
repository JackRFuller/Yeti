using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraView : MonoBehaviour
{
    private Camera camera;
    private PlayerView playerView;

    public Camera Camera {get {return camera;}}
    public PlayerView PlayerView {get {return playerView;}}

    private void Start()
    {
        camera = GetComponent<Camera>();
        playerView = GameManager.Instance.PlayerView;        
    }
}
