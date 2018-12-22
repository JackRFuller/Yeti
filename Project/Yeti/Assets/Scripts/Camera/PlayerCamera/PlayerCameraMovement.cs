using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : PlayerCameraComponent
{
    private Transform playerTargetTransform;
    private MovementState cameraMovementState;

    private float lockCameraCooldown = 1.0f;
    private bool canToggleCameraState = true;

    private enum MovementState
    {
        Locked,
        Following,
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        playerTargetTransform = GameManager.Instance.PlayerView.transform;
        cameraMovementState = MovementState.Following;  

        GameManager.Instance.PlayerView.GetPlayerInput.ToggleCameraLockState += LockCameraPosition;      
    }   

    private void LateUpdate()
    {
        if(cameraMovementState == MovementState.Following)
            FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 newCameraPosition = new Vector3(playerTargetTransform.position.x,
                                                playerTargetTransform.position.y,
                                                -10);
        
        transform.position = newCameraPosition;
    }

    private void LockCameraPosition()
    {
        if(canToggleCameraState)
        {
            cameraMovementState = cameraMovementState == MovementState.Following? MovementState.Locked:MovementState.Following; 
            StartCoroutine(CameraStateCooldown());
            canToggleCameraState = false;
        }        
    }

    private IEnumerator CameraStateCooldown()
    {
        yield return new WaitForSeconds(lockCameraCooldown);
        canToggleCameraState = true;
    }
}
