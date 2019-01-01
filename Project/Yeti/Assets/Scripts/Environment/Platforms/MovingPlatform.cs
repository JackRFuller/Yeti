using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 0.5f;

    [SerializeField] private Vector3 positionA;
    [SerializeField] private Vector3 positionB;
    private int positionIndex;

    private Vector3Lerp lerpingAttributes;
    private PlayerView playerView;

    // Start is called before the first frame update
    void Start()
    {
        lerpingAttributes = new Vector3Lerp();
        lerpingAttributes.lerpSpeed = movementSpeed;

        positionIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(lerpingAttributes.hasStartedLerp)
            MovePlatform();
    }

    public void TriggerMovingPlatform(Transform player)
    {
         if(!playerView)
            playerView = player.GetComponent<PlayerView>();
        
        //Determine where the player is in relation to the moving platform  
        float platformRotation = Extensions.ReturnObjectOrientation(transform);
        Vector3 targetPosition = positionIndex == 0? positionB:positionA;     

        if(platformRotation == 0 || platformRotation == 180)
        {
            bool playerIsAbove = player.position.y > transform.position.y?true:false;
            if(playerIsAbove && transform.position.y > targetPosition.y || !playerIsAbove && transform.position.y < targetPosition.y)
            {
               InitiatePlatformMovement();               
            }
        } 
        if(platformRotation == 90 || platformRotation == 270)
        {
            bool playerIsOnRightSide = player.position.x > transform.position.x?true:false;
           
            if(playerIsOnRightSide && transform.position.x > targetPosition.x || !playerIsOnRightSide && targetPosition.x > transform.position.x)
            {
                InitiatePlatformMovement();
            }
        }      
    }

    private void InitiatePlatformMovement()
    {
        playerView.GetPlayerMovement.SetNewPlayerParent(transform);

        lerpingAttributes.startValue = transform.position;
        lerpingAttributes.targetValue = positionIndex == 0? positionB:positionA;
        lerpingAttributes.timeStartedLerping = Time.time;
        lerpingAttributes.hasStartedLerp = true;
        playerView.LockPlayer();
    }

    private void MovePlatform()
    {
        float percentageComplete = lerpingAttributes.ReturnPercentageComplete();
        Vector3 newPosition = lerpingAttributes.ReturnLerpProgress(percentageComplete);

        transform.position = newPosition;

        if(percentageComplete >= 1.0f)
        {
            lerpingAttributes.hasStartedLerp = false;
            playerView.GetPlayerMovement.SetPlayerToOriginalParent();
            playerView.UnlockPlayer();
            positionIndex = positionIndex ==1? positionIndex = 0: positionIndex = 1;
        }
    }


    #region EditorMethods - called from Editor Buttons

    public void SetStartingPosition()
    {
        positionA = transform.position;
    }

    public void SetTargetPosition()
    {
        positionB = transform.position;
    }

    public void SetToStartingPosition()
    {
        transform.position = positionA;
    }

    #endregion
}
