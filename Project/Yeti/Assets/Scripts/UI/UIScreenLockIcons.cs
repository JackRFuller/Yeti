using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenLockIcons : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image lockIconImage; 
    [SerializeField] private Image lockCooldownImage;
  
    private float cameraLockTimerLength;

    private void Start()
    {
        GameManager.Instance.PlayerCameraView.GetCameraMovement.CameraLocked += StartCameraLockUICooldown;
        GameManager.Instance.PlayerCameraView.GetCameraMovement.CameraTimerRunning += ShowLockCooldownProgress;
        
      
        lockCooldownImage.sprite = lockIconImage.sprite;
        lockCooldownImage.fillAmount = 0;

        lockCooldownImage.enabled = false;
        lockIconImage.enabled = false;
    }

    private void StartCameraLockUICooldown(float timerLength)
    {
        cameraLockTimerLength = timerLength;

        lockCooldownImage.fillAmount = 0;

        lockCooldownImage.enabled = true;
        lockIconImage.enabled = true;
    }

    private void ShowLockCooldownProgress(float cameraLockTimer)
    {
        float percentageComplete = cameraLockTimer / cameraLockTimerLength;
        lockCooldownImage.fillAmount = percentageComplete;

        if(percentageComplete >= 1.0f)
        {
            lockCooldownImage.enabled = false;
            lockIconImage.enabled = false;
        }
    }
}
