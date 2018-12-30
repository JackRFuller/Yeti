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

        playerView.FreezePlayer += DisableAnimations;
        playerView.UnFreezePlayer += EnableAnimations;
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
            facingDirection = Vector2.zero;
            facingDirection.y = directionX == 1? 90:270;            
            playerMesh.localEulerAngles = facingDirection;
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
