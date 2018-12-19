using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PlayerInput playerInput;
    private Controller2D playerController2D;
    private PlayerMovement playerMovement;

    public PlayerInput GetPlayerInput { get {return playerInput;}}
    public Controller2D GetPlayerController2D {get {return playerController2D;}}
    public PlayerMovement GetPlayerMovement {get {return playerMovement;}}

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerController2D = GetComponent<Controller2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }
}
