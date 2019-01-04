using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private int numberOfKeysNeeded = 1;

    private PlayerView playerView;

    private SpriteRenderer doorlockSprite;
    private Collider2D doorCollider;
    private Animator doorAnimator; 

    private void Start()
    {
        doorCollider = GetComponent<Collider2D>();
        doorAnimator = GetComponent<Animator>();
        doorlockSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OpenDoor()
    {
        doorCollider.enabled = false;
        doorlockSprite.enabled = false;

        doorAnimator.SetTrigger("Open");
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            if(playerView == null)
                playerView = other.GetComponent<PlayerView>();
            
            //Check number of keys
            if(playerView.GetPlayerInventory.NumberOfHeldKeys == numberOfKeysNeeded)
            {
                playerView.GetPlayerInventory.RemoveKeys(numberOfKeysNeeded);

                //Trigger Door
                OpenDoor();
            }
        }
    }
}
