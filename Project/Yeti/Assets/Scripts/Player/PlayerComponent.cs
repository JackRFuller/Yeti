using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    protected PlayerView playerView;

    protected virtual void Start()
    {
        playerView = GetComponent<PlayerView>();
    }    
}
