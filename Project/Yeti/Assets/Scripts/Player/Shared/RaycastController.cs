using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
   [SerializeField] protected LayerMask collisionMask;
   protected Bounds bounds;
   private float objectOrientation;

   protected const float skinWidth = .015f;
   private const float distBetweenRays = .25f;

   protected int horizontalRayCount = 4;
   protected int verticalRayCount = 4;

   protected float horizontalRaySpacing;
   protected float verticalRaySpacing;

   private BoxCollider2D objectCollider;

   public RayCastOrigins rayCastOrigins;
   public float ObjectOrientation { get {return objectOrientation;}}


   protected virtual void Awake()
   {
       objectCollider = GetComponent<BoxCollider2D>();
   }

   protected virtual void Start()
   {
       CalculateRaySpacing();
   }

   protected void UpdateRayCastOrigins()
   {
       bounds = objectCollider.bounds;
       bounds.Expand(skinWidth * -2);

       SetRayCastOrigins();
   }

   public void PlayerOrientationUpdated()
   {
       CalculateRaySpacing();
       SetRayCastOrigins();
   }

   public void CalculateRaySpacing()
   {
       Bounds bounds = objectCollider.bounds;
       bounds.Expand(skinWidth * -2);      

       horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
	   verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

       float orientation = GetObjectOrientation();

        if (orientation == 0 || orientation == 180)
        {
            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }
        if (orientation == 90 || orientation == 270)
        {
            horizontalRaySpacing = bounds.size.x / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
        }
   }

   #region RayCastOriginPositions()
   
   private void SetRayCastOrigins()
   {
       objectOrientation = GetObjectOrientation();

        if(objectOrientation == 0)
        {
            rayCastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            rayCastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            rayCastOrigins.topLeft = new Vector2(bounds.min.x,bounds.max.y);
            rayCastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }
        else if(objectOrientation == 180)
        {
            rayCastOrigins.bottomLeft = new Vector2(bounds.max.x, bounds.max.y);
            rayCastOrigins.bottomRight = new Vector2(bounds.min.x, bounds.max.y);
            rayCastOrigins.topLeft = new Vector2(bounds.min.x, bounds.min.y);
            rayCastOrigins.topRight = new Vector2(bounds.max.x, bounds.min.y);
        }
        else if(objectOrientation == 90)
        {
            rayCastOrigins.bottomLeft = new Vector2(bounds.max.x, bounds.min.y);
            rayCastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.max.y);
            rayCastOrigins.topLeft = new Vector2(bounds.min.x, bounds.min.y);
            rayCastOrigins.topRight = new Vector2(bounds.min.x, bounds.max.y);
        }
        else if(objectOrientation == 270)
        {
            rayCastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.max.y);
            rayCastOrigins.bottomRight = new Vector2(bounds.min.x, bounds.min.y);
            rayCastOrigins.topLeft = new Vector2(bounds.max.x, bounds.max.y);
            rayCastOrigins.topRight = new Vector2(bounds.max.x, bounds.min.y);
        }
   }

   #endregion

   public struct RayCastOrigins
   {
       public Vector2 topLeft, topRight;
       public Vector2 bottomLeft, bottomRight;
   }

   public float GetObjectOrientation()
   {
       float objectOrientation = Mathf.Abs(transform.eulerAngles.z);       
       return objectOrientation;
   }
}
