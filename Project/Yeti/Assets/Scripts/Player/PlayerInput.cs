using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : PlayerComponent
{
    public event Action ToggleCameraLockState;

    private void Update()
    {
        CheckDirectionalInput();
        CheckJumpInput();
        GetCameraLockingInput();
    }    

    private void CheckDirectionalInput()
    {
        Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		playerView.GetPlayerMovement.SetDirectionalInput (directionalInput);
    }

    private void CheckJumpInput()
    {
        if (Input.GetKeyDown (KeyCode.Space)) {
			playerView.GetPlayerMovement.OnJumpInputDown ();
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			playerView.GetPlayerMovement.OnJumpInputUp ();
		}
    }

    private void GetCameraLockingInput()
    {
        if(Input.GetMouseButtonUp(0))
            ToggleCameraLockState();
    }
}
