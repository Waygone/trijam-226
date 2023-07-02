using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmokeHandler : MonoBehaviour
{
    float emissionRate = 0f;

    PlayerController playerController;

    ParticleSystem smokeParticles;
    ParticleSystem.EmissionModule emissionModule;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();

        smokeParticles = GetComponent<ParticleSystem>();

        emissionModule = smokeParticles.emission;

        emissionModule.rateOverTime = emissionRate;
    }

    private void Update()
    {
        emissionRate = Mathf.Lerp(emissionRate, 0f, Time.deltaTime * 3);
        emissionModule.rateOverTime = emissionRate;

        if (playerController.IsTireScreeching(out float lv))
        {
            emissionRate = Random.Range(20,30);
        }
        else emissionRate = Random.Range(1, 3);
    }
}
