using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : PlayerComponent
{
    private InputState inputState;
    private enum InputState
    {
        Locked,
        Free,
    }

    protected override void Start()
    {
        base.Start();

        inputState = InputState.Free;

        playerView.FreezePlayer += LockInput;
        playerView.UnFreezePlayer += UnlockInput;
    }

    private void Update()
    {
        if(inputState != InputState.Free)
            return;

        CheckDirectionalInput();
        CheckJumpInput();       
        CheckGroundPoundInput();
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

    private void CheckGroundPoundInput()
    {
        if(Input.GetMouseButtonUp(0))
            playerView.GetPlayerMovement.InitiateGroundPound();
    }    

    private void LockInput()
    {
        inputState = InputState.Locked;
    }

    private void UnlockInput()
    {
        inputState = InputState.Free;
    }
}
