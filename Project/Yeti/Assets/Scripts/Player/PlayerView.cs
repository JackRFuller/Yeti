using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerView : MonoBehaviour
{
    private PlayerInput playerInput;
    private Controller2D playerController2D;
    private PlayerMovement playerMovement;
    private PlayerAnimation playerAnimation;

    public PlayerInput GetPlayerInput { get {return playerInput;}}
    public Controller2D GetPlayerController2D {get {return playerController2D;}}
    public PlayerMovement GetPlayerMovement {get {return playerMovement;}}
    public PlayerAnimation GetPlayerAnimation { get {return playerAnimation;}}

    private int numberOfTimesFrozen;
    private int numberOfTimesUnFrozen;
    public event Action FreezePlayer;
    public event Action UnFreezePlayer;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerController2D = GetComponent<Controller2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void LockPlayer()
    {
        numberOfTimesFrozen++;

        if(FreezePlayer != null)
            FreezePlayer();
    }

    public void UnlockPlayer()
    {
        numberOfTimesUnFrozen++;

        if(numberOfTimesUnFrozen == numberOfTimesFrozen)
            StartCoroutine(CoolDownBeforeBeingAbleToMoveAgain());
    }

     IEnumerator CoolDownBeforeBeingAbleToMoveAgain()
    {
        yield return new WaitForSeconds(0.1f);
         if(UnFreezePlayer != null)
            UnFreezePlayer();

        numberOfTimesFrozen = 0;
        numberOfTimesUnFrozen = 0;       
    }


}
