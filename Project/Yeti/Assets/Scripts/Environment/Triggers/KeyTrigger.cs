using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : MonoBehaviour
{
    private Collider2D keyCollider;
    private SpriteRenderer objectSprite;

    private void Start()
    {
        keyCollider = GetComponent<Collider2D>();
        objectSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void TurnOffKey()
    {
        keyCollider.enabled = false;
        objectSprite.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            PlayerView playerView = other.GetComponent<PlayerView>();
            playerView.GetPlayerInventory.AddKey();

            TurnOffKey();
        }
    }
}
