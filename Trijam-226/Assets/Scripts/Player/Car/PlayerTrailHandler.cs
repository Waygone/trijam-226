using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailHandler : MonoBehaviour
{
    PlayerController playerController;
    TrailRenderer trailRenderer;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();

        trailRenderer = GetComponentInChildren<TrailRenderer>();

        trailRenderer.emitting = false;
    }

    private void Update()
    {        
        trailRenderer.emitting = playerController.IsTireScreeching(out float latVel, out bool isBraking);
    }
}
