using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PlayerComponent
{
    [SerializeField] private Transform playerMesh;
    private Animator playerAnimator;

    private int lastDirectionX;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerAnimator = GetComponentInChildren<Animator>();
        playerView.GetPlayerMovement.PlayerJumped += PlayerJumped;
        playerView.GetPlayerMovement.PlayerGroundPounding += PlayerGroundPounding;
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

        if(directionX != lastDirectionX)
        {
            facingDirection = playerMesh.eulerAngles;

            if(playerView.GetPlayerController2D.ObjectOrientation == 0)
                facingDirection.y = directionX == 1? 90:270;
            else if(playerView.GetPlayerController2D.ObjectOrientation == 180)
                facingDirection.y = directionX == 1? 270:90;
            
            playerMesh.eulerAngles = facingDirection;
        }

        lastDirectionX = directionX;
    }

    private void UpdatePlayerSpeed()
    {
        int movementSpeed = Mathf.RoundToInt(playerView.GetPlayerMovement.DirectionalInput.x);        
        playerAnimator.SetInteger("movementSpeed",Mathf.Abs(movementSpeed));        
    }

    private void UpdatePlayerGroundedState()
    {
        bool aboutToLand = playerView.GetPlayerController2D.AboutToLand;
        bool isGrounded = playerView.GetPlayerController2D.Collisions.below;

        playerAnimator.SetBool("isGrounded",isGrounded);
        playerAnimator.SetBool("aboutToLand",aboutToLand);
    }

    private void PlayerJumped()
    {
        playerAnimator.SetTrigger("Jump");
    }

    private void PlayerGroundPounding()
    {
        playerAnimator.SetTrigger("GroundPound");
    }

    public void DisableAnimations()
    {
        playerAnimator.enabled = false;
    }

    public void EnableAnimations()
    {
        playerAnimator.enabled = true;
    }
}
