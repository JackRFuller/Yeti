using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraComponent : MonoBehaviour
{
    protected PlayerCameraView cameraView;

    protected virtual void Start()
    {
        cameraView = GetComponent<PlayerCameraView>();
    }
}
