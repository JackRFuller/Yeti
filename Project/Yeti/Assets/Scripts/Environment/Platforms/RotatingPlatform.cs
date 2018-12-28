using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{    
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private float rotationFactor = 2; //Either 4 or 2
    private Vector3Lerp lerpingAttributes;

    private void Start()
    {
        lerpingAttributes = new Vector3Lerp();
        lerpingAttributes.lerpSpeed = rotationSpeed;
    }

    public void TriggerRotation(Transform player)
    {
        player.parent = this.transform;

        //Calculate if player is below or above the center point
        bool playerIsAbove = player.position.y > transform.position.y? true:false;
        bool playerIsOnRightSide = player.position.x > transform.position.x? true:false;

        float rotationDirection = 0;

        if(playerIsAbove)
        {
            rotationDirection = playerIsOnRightSide? 360 / -rotationFactor:360 / rotationFactor;
        }
        else
        {
            rotationDirection = playerIsOnRightSide? 360 / rotationFactor:360 / -rotationFactor;
        }

        lerpingAttributes.startValue = transform.eulerAngles;
        lerpingAttributes.targetValue = new Vector3(transform.eulerAngles.x,
                                                    transform.eulerAngles.y,
                                                    transform.eulerAngles.z + rotationDirection);

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
            lerpingAttributes.hasStartedLerp = false;
        }
    }
}
