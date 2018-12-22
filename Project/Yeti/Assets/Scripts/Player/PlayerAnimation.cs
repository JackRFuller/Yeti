using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PlayerComponent
{
    [SerializeField] private Transform playerMesh;
    private Animator playerAnimator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerAnimator = GetComponentInChildren<Animator>();
        playerView.GetPlayerMovement.PlayerJumped += PlayerJumped;
    }    

    private void Update()
    {
        UpdatePlayerSpeed();
        UpdatePlayerGroundedState();
        UpdatePlayerMeshFacingDirection();
    }

    private void UpdatePlayerMeshFacingDirection()
    {
        Vector3 facingDirection = Vector3.zero;
        int directionX = playerView.GetPlayerController2D.Collisions.faceDir;

        facingDirection.y = directionX == 1? 90:270;

        playerMesh.eulerAngles = facingDirection;
    }

    private void UpdatePlayerSpeed()
    {
        int movementSpeed = Mathf.RoundToInt(playerView.GetPlayerMovement.DirectionalInput.x);        
        playerAnimator.SetInteger("movementSpeed",Mathf.Abs(movementSpeed));        
    }

    private void UpdatePlayerGroundedState()
    {
        bool isGrounded = playerView.GetPlayerController2D.Collisions.below;
        playerAnimator.SetBool("isGrounded",isGrounded);
    }

    private void PlayerJumped()
    {
        playerAnimator.SetTrigger("Jump");
    }
}
