using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatform : MonoBehaviour
{
    private bool canRotate;
    private bool canMove;

    private RotatingPlatform rotatingPlatform;
    private MovingPlatform movingPlatform;

    private void Start()
    {
        canRotate = rotatingPlatform = GetComponent<RotatingPlatform>();
        canMove = movingPlatform = GetComponent<MovingPlatform>();
    }

    public void TriggerDynamicPlatformBehaviours(Transform player)
    {
        if(canRotate)
            rotatingPlatform.TriggerRotation(player);

        if(canMove)
            movingPlatform.TriggerMovingPlatform(player);
    }

    
}
