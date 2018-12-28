using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerView playerView;

    // Start is called before the first frame update
    void Start()
    {
        playerView = transform.parent.GetComponent<PlayerView>();
    }

    public void GroundPoundAnimationEvent()
    {
        playerView.GetPlayerMovement.GroundPound();
    }
}
