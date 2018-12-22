using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {get {return instance;}}

    [SerializeField] private PlayerView playerView;
    [SerializeField] private PlayerCameraView playerCameraView;

    public PlayerView PlayerView {get{return playerView;}}
    public PlayerCameraView PlayerCameraView { get {return playerCameraView;}}

    private void Awake()
    {        
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
