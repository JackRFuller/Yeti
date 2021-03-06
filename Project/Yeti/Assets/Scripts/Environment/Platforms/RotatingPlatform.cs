﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{    
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private float rotationFactor = 2; //Either 4 or 2
    private Vector3Lerp lerpingAttributes;
    private PlayerView playerView;

    private void Start()
    {
        lerpingAttributes = new Vector3Lerp();
        lerpingAttributes.lerpSpeed = rotationSpeed;
    }

    public void TriggerRotation(Transform player)
    {        
        if(!playerView)
            playerView = player.GetComponent<PlayerView>();

        playerView.GetPlayerMovement.SetNewPlayerParent(transform);

        //Calculate if player is below or above the center point
        float platformRotation = Extensions.ReturnObjectOrientation(transform);  

        float rotationDirection = 0;
        bool playerIsAbove = player.position.y > transform.position.y? true:false;
        bool playerIsOnRightSide = player.position.x > transform.position.x? true:false;

        if(platformRotation == 0 || platformRotation == 180)
        {
             if(playerIsAbove)
                rotationDirection = playerIsOnRightSide? 360 / -rotationFactor:360 / rotationFactor;
            else
                rotationDirection = playerIsOnRightSide? 360 / rotationFactor:360 / -rotationFactor;
        }
        else if(platformRotation == 90 || platformRotation == 270)
        {
            if(playerIsAbove)
                rotationDirection = playerIsOnRightSide? 360 / rotationFactor:360 / -rotationFactor;
            else
            {
                rotationDirection = playerIsOnRightSide? 360 / -rotationFactor:360 / rotationFactor;
            }
        }

        Vector3 targetValue = new Vector3(transform.eulerAngles.x,
                                        transform.eulerAngles.y,
                                        Mathf.Round(transform.eulerAngles.z + rotationDirection));       

        lerpingAttributes.startValue = transform.eulerAngles;
        lerpingAttributes.targetValue = targetValue;

        lerpingAttributes.timeStartedLerping = Time.time;
        lerpingAttributes.hasStartedLerp = true; 
    }

    private void Update()
    {
        if(lerpingAttributes.hasStartedLerp)
            RotatePlatform();
    }

    private void RotatePlatform()
    {
        float percentageComplete = lerpingAttributes.ReturnPercentageComplete();
        Vector3 newRotation = lerpingAttributes.ReturnLerpProgress(percentageComplete);

        transform.eulerAngles = newRotation;

        if(percentageComplete >= 1.0f)
        {
            Vector3 forcedRotation = lerpingAttributes.targetValue;

            if(Mathf.Approximately(360,forcedRotation.z))
                forcedRotation.z = 0;              

            transform.eulerAngles = forcedRotation;           
            lerpingAttributes.hasStartedLerp = false;
          
            if(Mathf.Approximately(0f, transform.eulerAngles.z))
            {
                Vector3 tempTransform = Vector3.zero;
                transform.eulerAngles = tempTransform;
            }
            
            playerView.GetPlayerMovement.SetPlayerToOriginalParent();
            playerView.GetPlayerMovement.SnapPlayerOrientation();
            playerView.UnlockPlayer(); 
        }
    }
}
