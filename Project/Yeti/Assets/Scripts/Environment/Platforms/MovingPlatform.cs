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

    [Header("Path Elements")]
    [SerializeField] private Sprite pathSprite;
    private GameObject path;

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

    public void CreatePath()
    {
        //Create path holder and name
        if(path)
        {
            DestroyImmediate(path);
        }
        path = new GameObject();
        path.transform.parent = transform.parent;
        path.name = this.gameObject.name + " Path";

        //Create Travel Point
        for (int i = 0; i < 2; i++)
        {
            GameObject travelPoint = new GameObject();
           
            travelPoint.name = "Travel Point " + i.ToString();
            travelPoint.AddComponent<SpriteRenderer>();
            travelPoint.GetComponent<SpriteRenderer>().sprite = pathSprite;
            travelPoint.transform.localScale = new Vector3(40,40,1);
            Vector3 travelPointPosition = i == 0?positionA:positionB;
            travelPointPosition.z = 1;
            travelPoint.transform.position = travelPointPosition;

            travelPoint.transform.parent = path.transform;
        }

        GameObject travelLine = new GameObject();
        travelLine.name = "Travel Line";
        travelLine.AddComponent<SpriteRenderer>();
        travelLine.GetComponent<SpriteRenderer>().sprite = pathSprite;

        Vector3 travelLinePosition = (positionB + positionA) * 0.5f;
        travelLinePosition.z = 1;
        travelLine.transform.position = travelLinePosition;

        Vector3 travelLineSize = Vector3.zero;

        if(positionA.y == positionB.y)
        {
            travelLineSize.y = 7;
            travelLineSize.x = Vector3.Distance(positionA,positionB) * 100;
        }
        else if(positionB.x == positionA.x)
        {
            travelLineSize.y = Vector3.Distance(positionA,positionB) * 100;
            travelLineSize.x = 7;
        }

        travelLine.transform.localScale = travelLineSize;
        travelLine.transform.parent = path.transform;

    }

    #endregion
}
