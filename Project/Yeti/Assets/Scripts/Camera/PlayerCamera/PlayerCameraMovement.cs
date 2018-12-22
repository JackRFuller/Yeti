using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : PlayerCameraComponent
{
    private Transform playerTargetTransform;
    private MovementState cameraMovementState;

    private float lockCameraCooldown = 1.0f;
    private bool canToggleCameraState = true;

    [SerializeField] private Vector3Lerp cameraLerpingAttributes;

    private enum MovementState
    {
        Locked,
        Following,
        Returning,
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        playerTargetTransform = GameManager.Instance.PlayerView.transform;
        cameraMovementState = MovementState.Following;  

        GameManager.Instance.PlayerView.GetPlayerInput.ToggleCameraLockState += ToggleCameraMovementState;      
    }   

    private void LateUpdate()
    {
        if(cameraMovementState == MovementState.Following)
            FollowPlayer();

         if(cameraMovementState == MovementState.Returning)
            ReturnToPlayerPosition();
    }

    private void FollowPlayer()
    {
        Vector3 newCameraPosition = new Vector3(playerTargetTransform.position.x,
                                                playerTargetTransform.position.y,
                                                -10);
        
        transform.position = newCameraPosition;
    }

    private void ReturnToPlayerPosition()
    {
        float percenatgeComplete = cameraLerpingAttributes.ReturnPercentageComplete();
        Vector3 newCameraPosition = cameraLerpingAttributes.ReturnLerpProgress(percenatgeComplete);

        transform.position = newCameraPosition;

        if(percenatgeComplete >= 1.0)
        {
            cameraMovementState = MovementState.Following;
        }
    }

    private void ToggleCameraMovementState()
    {
        if(canToggleCameraState)
        {
            cameraMovementState = cameraMovementState == MovementState.Following? MovementState.Locked:MovementState.Returning; 
            StartCoroutine(CameraStateCooldown());
            canToggleCameraState = false;

            if(cameraMovementState == MovementState.Returning)
            {
                InitCameraReturn();
            }
        }        
    }

     private IEnumerator CameraStateCooldown()
    {
        yield return new WaitForSeconds(lockCameraCooldown);
        canToggleCameraState = true;
    }

    private void InitCameraReturn()
    {
        cameraLerpingAttributes.startValue = transform.position;
        cameraLerpingAttributes.targetValue = new Vector3(playerTargetTransform.position.x,
                                                          playerTargetTransform.position.y,
                                                          -10);

        cameraLerpingAttributes.timeStartedLerping = Time.time;
    }

   
}
