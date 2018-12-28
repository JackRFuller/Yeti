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
    [SerializeField] private float groundPoundSpeed = 70f;
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
    

    protected override void Start()
    {
        base.Start();

        controller2D = playerView.GetPlayerController2D;
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
        if(controller2D == null)
            controller2D = playerView.GetPlayerController2D;

        if(!isGroundPounding)
        {
            CalculateVelocity();
            HandleWallSliding();
        }

        controller2D.Move (velocity * Time.deltaTime, directionalInput);

		if (controller2D.Collisions.above || controller2D.Collisions.below)
        {
            jumpCount = 0;

			if (controller2D.Collisions.slidingDownMaxSlope) {
				velocity.y += controller2D.Collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} 
            else 
            {
                //Triggers Dynamic Platform if this is the first time grounded since ground pound
                if(isGroundPounding)
                {
                    if(targetPlatform != null)
                        targetPlatform.TriggerDynamicPlatformBehaviours(this.transform);

                    isGroundPounding = false;
                }

				velocity.y = 0;
			}
		} 

        if(!controller2D.Collisions.below)
        {
             StartCoroutine(WaitForGroundedState());
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
        directionalInput = input;
    }

    public void InitiateGroundPound()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 10,collisionMask);
	    Debug.DrawRay(transform.position, -Vector2.up * 10,Color.green);

        if(hit)
        {
            targetPlatform = hit.collider.GetComponent<DynamicPlatform>();

            float distanceFromPlatform = Vector3.Distance(transform.position, hit.point);
            Debug.Log(distanceFromPlatform);

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

    public void OnJumpInputDown()
    {
		if (wallSliding) {
			if (wallDirX == directionalInput.x) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
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

    void HandleWallSliding() 
    {
		wallDirX = (controller2D.Collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller2D.Collisions.left || controller2D.Collisions.right) && !controller2D.Collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideMaxSpeed) {
				velocity.y = -wallSlideMaxSpeed;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}
		}
	}

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,(controller2D.Collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
}