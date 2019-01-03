using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCameraMovement : PlayerCameraComponent
{
    public event Action<float> CameraLocked;
    public event Action<float> CameraTimerRunning;
    public event Action CameraUnlocked;

    private Transform playerTargetTransform;
    private MovementState cameraMovementState; 

    [SerializeField] private Vector3Lerp cameraLerpingAttributes;

    private float cameraLockTimerLength;
    private float cameraLockTimer;

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
    }   

    private void Update()
    {
        if(cameraMovementState == MovementState.Locked)
            RunCameraLockTimer();
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

    #region Camera Locking

    public void LockCamera(float timeLockedFor)
    {
        cameraLockTimerLength += timeLockedFor;
        cameraMovementState = MovementState.Locked;            
        
        CameraLocked(timeLockedFor);
    }

    private void RunCameraLockTimer()
    {
        cameraLockTimer += Time.deltaTime;
        CameraTimerRunning(cameraLockTimer);     
        if(cameraLockTimer >= cameraLockTimerLength)
        {
            
            cameraLockTimer = 0;
            cameraLockTimerLength = 0;
            InitCameraReturn();

            if(CameraUnlocked != null)
                CameraUnlocked();
        } 
    }

    private void InitCameraReturn()
    {
        cameraLerpingAttributes.startValue = transform.position;
        cameraLerpingAttributes.targetValue = new Vector3(playerTargetTransform.position.x,
                                                          playerTargetTransform.position.y,
                                                          -10);

        cameraLerpingAttributes.timeStartedLerping = Time.time;

        cameraMovementState = MovementState.Returning;
    }

    #endregion
}
