using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : PlayerComponent
{
    private void Update()
    {
        CheckDirectionalInput();
        CheckJumpInput();
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
}
