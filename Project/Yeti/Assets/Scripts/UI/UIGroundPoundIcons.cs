using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGroundPoundIcons : MonoBehaviour
{
    private PlayerView playerView;

    [SerializeField] private Image groundPoundEnabledImage;
    [SerializeField] private Image groundPoundDisabledImage;

    private void Start() 
    {
        playerView = GameManager.Instance.PlayerView;
    }
    

    // Update is called once per frame
    void Update()
    {
        UpdateGroundPoundIconState();
    }

    private void UpdateGroundPoundIconState()
    {
        groundPoundEnabledImage.enabled = playerView.GetPlayerMovement.CanGroundPound;
    }
}
