using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
   [SerializeField] protected LayerMask collisionMask;

   protected const float skinWidth = .015f;
   private const float distBetweenRays = .25f;

   protected int horizontalRayCount = 4;
   protected int verticalRayCount = 4;

   protected float horizontalRaySpacing;
   protected float verticalRaySpacing;

   private BoxCollider2D objectCollider;

   public RayCastOrigins rayCastOrigins;

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
       Bounds bounds = objectCollider.bounds;
       bounds.Expand(skinWidth * -2);

       SetRayCastOriginsIfPlayerIs0Degrees(bounds);
   }

   protected void CalculateRaySpacing()
   {
       Bounds bounds = objectCollider.bounds;
       bounds.Expand(skinWidth * -2);

       float boundsWidth = bounds.size.x;
       float boundsHeight = bounds.size.y;

       horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
	   verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

       horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
       verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
   }

   #region RayCastOriginPositions()
   
   private void SetRayCastOriginsIfPlayerIs0Degrees(Bounds bounds)
   {
       rayCastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
       rayCastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
       rayCastOrigins.topLeft = new Vector2(bounds.min.x,bounds.max.y);
       rayCastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
   }

   #endregion

   public struct RayCastOrigins
   {
       public Vector2 topLeft, topRight;
       public Vector2 bottomLeft, bottomRight;
   }
}
