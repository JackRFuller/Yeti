using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController
{
    protected float maxSlopeAngle = 80;

	private bool aboutToLand; //Used to determine whether the player is falling or not
    private CollisionInfo collisionInfo;   

	public bool AboutToLand { get {return aboutToLand;}}
	public CollisionInfo Collisions { get {return collisionInfo;}}

    protected override void Start()
    {
        base.Start();
        collisionInfo.faceDir = 1;
    }

	public void Move(Vector2 moveAmount, bool standingOnPlatform) {
		Move (moveAmount);
	}

	public void Move(Vector2 moveAmount) 
	{
		UpdateRayCastOrigins ();

		collisionInfo.Reset ();
		collisionInfo.moveAmountOld = moveAmount;	
		
		if (moveAmount.x != 0) {
			collisionInfo.faceDir = (int)Mathf.Sign(moveAmount.x);
		}

		if(moveAmount.x != 0)
			HorizontalCollisions(ref moveAmount);

		
		if (moveAmount.y != 0)
			VerticalCollisions (ref moveAmount);
		

		transform.Translate (moveAmount);	
	}

    void HorizontalCollisions(ref Vector2 moveAmount) {
		float directionX = collisionInfo.faceDir;
		float rayLength = Mathf.Abs (moveAmount.x) + skinWidth;

		if (Mathf.Abs(moveAmount.x) < skinWidth) {
			rayLength = 2*skinWidth;
		}

		for (int i = 0; i < horizontalRayCount; i ++) 
		{
			Vector2 rayOrigin = Vector2.zero;

			if(ObjectOrientation == 0)
				rayOrigin = (directionX == -1)?rayCastOrigins.bottomLeft:rayCastOrigins.bottomRight;

			if(ObjectOrientation == 180)
				rayOrigin = (directionX == -1)?rayCastOrigins.bottomRight:rayCastOrigins.bottomLeft;
			
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX,Color.red);			

			if (hit)
			{
				if (hit.distance == 0) {
					continue;
				}	

				moveAmount.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;					

				collisionInfo.left = directionX == -1;
				collisionInfo.right = directionX == 1;	
			}			
		}
	}

	void VerticalCollisions(ref Vector2 moveAmount) 
	{
		float directionY = Mathf.Sign (moveAmount.y); //Checks movement direction
		float rayLength = Mathf.Abs (moveAmount.y) + skinWidth;

		Vector2 rayOriginDirection = Vector2.zero;
		Vector2 rayDirection = Vector2.zero;

		if(ObjectOrientation == 0)
		{
			rayOriginDirection = Vector2.right;
			rayDirection = Vector2.up;
		}
		else if(ObjectOrientation == 180)
		{
			rayOriginDirection = Vector2.left;
			rayDirection = Vector2.down;
		}

		for (int i = 0; i < verticalRayCount; i ++) 
		{
			Vector2 rayOrigin = (directionY == -1)?rayCastOrigins.bottomLeft:rayCastOrigins.topLeft;
			rayOrigin += rayOriginDirection * (verticalRaySpacing * i + moveAmount.x);			
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection * directionY, rayLength, collisionMask);			

			Debug.DrawRay(rayOrigin, Vector2.up * directionY,Color.blue);

			aboutToLand = (hit)? false: true;
			
			if (hit)
			{	
				collisionInfo.below = directionY == -1;
				collisionInfo.above = directionY == 1;
			
				moveAmount.y = (hit.distance - skinWidth) * directionY;						
				rayLength = hit.distance;
			}
		}
	}   

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public bool slidingDownMaxSlope;

        public float slopeAngle, slopeAngleOld;
        public Vector2 slopeNormal;
        public Vector2 moveAmountOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
