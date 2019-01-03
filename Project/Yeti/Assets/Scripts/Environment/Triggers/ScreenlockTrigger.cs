using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenlockTrigger : MonoBehaviour
{
    [SerializeField] private float screenLockTime = 5;
    [SerializeField] private SpriteRenderer spriteLockIcon;

    private PlayerCameraMovement cameraMovement;
    private bool screenLockEnabled;
   

    // Start is called before the first frame update
    void Start()
    {
        cameraMovement = GameManager.Instance.PlayerCameraView.GetCameraMovement;
        cameraMovement.CameraTimerRunning += RunDownTimerToDisableScreenLock;
    }

    private void RunDownTimerToDisableScreenLock(float cameraTimer)
    {
        if(cameraTimer >= screenLockTime)
        {
           spriteLockIcon.enabled = true;
           screenLockEnabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.tag == "Player")
       {
           if(!screenLockEnabled)
           {
                cameraMovement.LockCamera(screenLockTime);
                spriteLockIcon.enabled = false;
                screenLockEnabled = true;
           }
       }
    }


}
