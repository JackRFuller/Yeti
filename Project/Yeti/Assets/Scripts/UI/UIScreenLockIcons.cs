using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenLockIcons : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image lockIconImage;
    [SerializeField] private Image lockCooldownImage;
    [SerializeField] private Sprite[] lockStateSprites;

    [SerializeField] private LerpingClasses.FloatLerp lerpingAttributes;

    private int cameraLockState = 0; //0 = free, 1 = locked;

    private void Start()
    {
        GameManager.Instance.PlayerView.GetPlayerInput.ToggleCameraLockState += ToggleCameraStateUI;
        lockIconImage.sprite = lockStateSprites[cameraLockState];
    }

    private void ToggleCameraStateUI()
    {
        cameraLockState = cameraLockState == 0? cameraLockState = 1: cameraLockState = 0;
        lockIconImage.sprite = lockStateSprites[cameraLockState];
        lockCooldownImage.fillAmount = 0;

        lerpingAttributes.timeStartedLerping = Time.time;
        lerpingAttributes.startValue = 0;
        lerpingAttributes.targetValue = 1;
        lerpingAttributes.hasStartedLerp = true;
    }

    private void Update()
    {
        ShowLockCooldownState();
    }

    private void ShowLockCooldownState()
    {
        if(!lerpingAttributes.hasStartedLerp)
            return;

        float percenatgeComplete = lerpingAttributes.ReturnPercentageComplete();
        float newValue = Mathf.Lerp(lerpingAttributes.startValue,lerpingAttributes.targetValue,percenatgeComplete);
        lockCooldownImage.fillAmount = newValue;

        if(percenatgeComplete >= 1.0f)
        {
            lerpingAttributes.hasStartedLerp = false;
        }
    }
}
