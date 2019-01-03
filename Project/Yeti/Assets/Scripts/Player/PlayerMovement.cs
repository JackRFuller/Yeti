using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : PlayerComponent
{  
    private Controller2D controller2D;

    public event Action PlayerJumped;
    public event Action PlayerGroundPounding;

    [Header("Movement Attributes")]
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private float maxJumpHeight = 4;
    [SerializeField] private float minJumpHeight = 1;
    [SerializeField] private float timeToJumpApex = .4f;
    [SerializeField] private Vector2 wallJumpClimb;
    [SerializeField] private Vector2 wallJumpOff;
    [SerializeField] private Vector2 wallLeap;   
    [SerializeField] private float wallSlideMaxSpeed = 3;
    [SerializeField] private float wallStickTime = .25f;
    [SerializeField] private float groundPoundSpeed = 5f;
    [SerializeField] private LayerMask collisionMask;
    
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float jumpCount;
    private const float maxJumpCount = 2;
    
    private Vector3 velocity;
    public Vector3 Velocity { get {return velocity;}}
    private float velocityXSmoothing;

    private Vector2 directionalInput;
    public Vector2 DirectionalInput {get{return directionalInput;}}
    private bool wallSliding;
    private int wallDirX;
    private float timeToWallUnstick;

    private bool checkForLastFrameState;
    private bool groundedStateLastFrame = true;

    private bool isGroundPounding;    

    private DynamicPlatform targetPlatform;
    private Vector3 hitPoint;
    private bool canGroundPound;
    public bool CanGroundPound {get{return canGroundPound;}}

    private Transform originalParent;

    private MovementState movementState = MovementState.Free;
    private enum MovementState
    {
        Free,
        Frozen,
    }
    

    protected override void Start()
    {
        base.Start();

        controller2D = playerView.GetPlayerController2D;

        playerView.FreezePlayer += FreezePlayerMovement;
        playerView.UnFreezePlayer += UnFreezePlayerMovement;

        originalParent = transform.parent;

        SetJumpValues();        
    }

    private void SetJumpValues()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex,2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        jumpCount = 0;
    }

    private void Update()
    {
        if(movementState == MovementState.Free)
        {
            if(!isGroundPounding)
            {
                CalculateVelocity();                
                CheckIfPlayerCanGroundPound();
            }       

            controller2D.Move (velocity * Time.deltaTime);

            if (controller2D.Collisions.above || controller2D.Collisions.below)
            {
                if(controller2D.Collisions.below)
                {
                    CheckAndTriggerGroundPoundOnPlatform();
                }
                
                velocity.y = 0;
                jumpCount = 0;
            } 

            if(!controller2D.Collisions.below)
            {
                StartCoroutine(WaitForGroundedState());
            }
        }
    }

    private void CheckAndTriggerGroundPoundOnPlatform()
    {
        if(isGroundPounding)
        {
            playerView.LockPlayer();
            //Set Player To HitPoint Position                
            Vector3 newPosition = hitPoint;
            if(controller2D.ObjectOrientation == 0)
                newPosition.y += 0.75f;
            if(controller2D.ObjectOrientation == 180)
                newPosition.y -= 0.75f;
            if(controller2D.ObjectOrientation == 90)
                newPosition.x -= 0.75f;
            if(controller2D.ObjectOrientation == 270)
                newPosition.x += 0.75f;

            transform.position = newPosition;
            

            if(targetPlatform != null)   
            {                
                targetPlatform.TriggerDynamicPlatformBehaviours(this.transform);                
            }   
            else
            {
               playerView.UnlockPlayer();
            }
            
            isGroundPounding = false;
        }
    }

    //TODO - Improve logic behind waiting
    private IEnumerator WaitForGroundedState()
    {
        yield return new WaitForSeconds(0.2f);
        groundedStateLastFrame = controller2D.Collisions.below;
    }
    
    public void SetDirectionalInput(Vector2 input)
    {
        float playerOrientation = controller2D.ObjectOrientation;

        Vector3 tempInput = input;

        if(playerOrientation == 180)
        {
            tempInput.x = -input.x;
        }
        else if(playerOrientation == 90)
        {
            tempInput.x = input.y;
        }
        else if(playerOrientation == 270)
        {
            tempInput.x = -input.y;
        }

        directionalInput = tempInput;
    }

    //TODO - Used for UI purposes, needs to be tidied up
    private void CheckIfPlayerCanGroundPound()
    {
        Vector2 rayDirection = Vector2.zero;       

        if(controller2D.ObjectOrientation == 0)
            rayDirection = Vector2.down;
        if(controller2D.ObjectOrientation == 180)
            rayDirection = Vector2.up;
        if(controller2D.ObjectOrientation == 90)
            rayDirection = Vector2.right;
        if(controller2D.ObjectOrientation == 270)
            rayDirection = Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 10,collisionMask);

        if(hit)
        { 
            float distanceFromPlatform = Vector3.Distance(transform.position, hit.point);          

            if(distanceFromPlatform > 3)
            {
                canGroundPound = true;
            }
            else
            {
                canGroundPound = false;
            }
        }
        else
        {
             canGroundPound = true;
        }	   
    }

    public void InitiateGroundPound()
    {
        if(isGroundPounding)
            return;

        Vector2 rayDirection = Vector2.zero;       

        if(controller2D.ObjectOrientation == 0)
            rayDirection = Vector2.down;
        if(controller2D.ObjectOrientation == 180)
            rayDirection = Vector2.up;
        if(controller2D.ObjectOrientation == 90)
            rayDirection = Vector2.right;
        if(controller2D.ObjectOrientation == 270)
            rayDirection = Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 10,collisionMask);
	    Debug.DrawRay(transform.position, rayDirection * 10,Color.magenta,1);

        if(hit)
        {
            targetPlatform = hit.collider.GetComponent<DynamicPlatform>();
            hitPoint = hit.point;
            float distanceFromPlatform = Vector3.Distance(transform.position, hit.point);          

            if(distanceFromPlatform > 3)
            {
                isGroundPounding = true;
                velocity = Vector3.zero;
                PlayerGroundPounding();
            }
        }
        else
        {
            isGroundPounding = true;
            velocity = Vector3.zero;
            PlayerGroundPounding();
        }
    }

    ///Called from PlayerAnimationEvent
    public void GroundPound()
    {        
        velocity.y = -groundPoundSpeed;
    }

    private void CheckDistanceFromTargetPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 10,collisionMask);
	    Debug.DrawRay(transform.position, -Vector2.up * 10,Color.green);

        float distanceFromPlatform = Vector3.Distance(transform.position,hit.point);
      
        if(distanceFromPlatform < 2f)
        {                    
            isGroundPounding = false;
        }
    }

    public void OnJumpInputDown()
    {		
		if (groundedStateLastFrame || jumpCount < maxJumpCount) 
        {           
			if (controller2D.Collisions.slidingDownMaxSlope) {
				if (directionalInput.x != -Mathf.Sign (controller2D.Collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller2D.Collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller2D.Collisions.slopeNormal.x;
				}
			} 
            else 
            {
                jumpCount++;
                velocity.y = maxJumpVelocity;
                PlayerJumped(); //Used for triggering animations 
			}
		}
	}

    public void OnJumpInputUp()
    {
        if(velocity.y > minJumpVelocity)
            velocity.y = minJumpVelocity;
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,(controller2D.Collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne); 
        velocity.y += gravity * Time.deltaTime;       
    }

    public void SetNewPlayerParent(Transform newParent)
    {
        transform.parent = newParent;
    }

    public void SetPlayerToOriginalParent()
    {
        transform.parent = originalParent;
    }

    public void SnapPlayerOrientation()
    {
        float playerRotation = transform.eulerAngles.z;
        playerRotation = playerRotation / 10;
        playerRotation = Mathf.Round(playerRotation);
        playerRotation*= 10;

        Vector3 newRot = Vector3.zero;
        newRot.z = playerRotation;
        transform.eulerAngles = newRot;         
    }

    public void FreezePlayerMovement()
    {
        canGroundPound = false;
        movementState = MovementState.Frozen;      
        velocity = Vector3.zero;
        controller2D.Move(velocity * Time.deltaTime);
    }

    public void UnFreezePlayerMovement()
    {
        controller2D.PlayerOrientationUpdated();  
         movementState = MovementState.Free; 
    }   
}