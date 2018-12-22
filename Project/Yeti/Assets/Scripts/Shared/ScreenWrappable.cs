using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrappable : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;

    private Camera playerCamera;

    private bool isWrappingX = false;
    private bool isWrappingY = false;

    private float screenHeight;
    private float screenWidth;

	private bool hasInitScreenWrap = false;

    // Start is called before the first frame update
    protected void Start()
    {
        GameManager.Instance.PlayerView.GetPlayerInput.ToggleCameraLockState += InitScreenWrap;        
    }

	private void InitScreenWrap()
	{
		if(!hasInitScreenWrap)
		{
			renderers = GetComponentsInChildren<Renderer>();

			playerCamera = GameManager.Instance.PlayerCameraView.Camera;

			Vector3 screenBottomLeft = playerCamera.ViewportToWorldPoint(new Vector3(0,0,transform.position.z));
			Vector3 screenTopRight = playerCamera.ViewportToWorldPoint(new Vector3(1,1,transform.position.z));

			screenWidth = screenTopRight.x - screenBottomLeft.z;
			screenHeight = screenTopRight.y - screenBottomLeft.y;

			hasInitScreenWrap = true;
		}
	}

    // Update is called once per frame
    void Update()
    {
        ScreenWrap();
    }

    private void ScreenWrap()
    {
		if(!hasInitScreenWrap)
			return;

        // If all parts of the object are invisible we wrap it around
		foreach(var renderer in renderers)
		{		
			if(renderer.isVisible)
			{
				isWrappingX = false;
				isWrappingY = false;				
			}			
		}

		// If we're already wrapping on both axes there is nothing to do
		if(isWrappingX && isWrappingY) {
			return;
		}	
	
		Vector3 newPosition = transform.position;
		Vector3 viewportPosition = playerCamera.WorldToViewportPoint(transform.position);
	
		if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
		{
			Vector3 newWorldPosition = viewportPosition.x > 1? playerCamera.ViewportToWorldPoint(new Vector3(0.01f,0,0)):playerCamera.ViewportToWorldPoint(new Vector3(0.99f,0,0));
			newPosition.x = newWorldPosition.x;
			isWrappingX = true;
		}
		
		// Wrap it is off screen along the y-axis and is not being wrapped already
		if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
		{
			Vector3 newWorldPosition = viewportPosition.y > 1? playerCamera.ViewportToWorldPoint(new Vector3(0,0.01f,0)): playerCamera.ViewportToWorldPoint(new Vector3(0,0.99f,0));
			newPosition.y = newWorldPosition.y;
			isWrappingY = true;
		}
		
		//Apply new position
		transform.position = newPosition;
    }
}
