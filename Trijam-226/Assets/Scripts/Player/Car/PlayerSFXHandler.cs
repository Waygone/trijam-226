using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSFXHandler : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource screechingAS;
    public AudioSource accelerationAS;
    public AudioSource hitAS;

    float accelPitch = 0.5f;
    float screechPitch = 0.5f;

    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        UpdateEngine();
        UpdateScreeching();
    }

    void UpdateEngine()
    {
        //Get velocity magnitude from the player controller
        float velMagnitude = playerController.GetVelocityMagnitude();

        //Increase volume according to velocity
        float desiredAccelVol = velMagnitude * 0.1f;
        
        //Limit the volume
        desiredAccelVol = Mathf.Clamp(desiredAccelVol, 0.1f, 1.0f);


        //Apply the volume
        accelerationAS.volume = Mathf.Lerp(accelerationAS.volume, desiredAccelVol, Time.deltaTime * 5);

        //Change pitch for variation
        accelPitch = velMagnitude * 0.5f;
        accelPitch = Mathf.Clamp(accelPitch, 0.5f, 2f);
        accelerationAS.pitch = Mathf.Lerp(accelerationAS.pitch, accelPitch, Time.deltaTime * 5);
    }

    void UpdateScreeching()
    {
        //If is leaving a trail
        if (playerController.IsTireScreeching(out float lv, out bool isBraking))
        {
            //If is braking
            if(isBraking)
            {
                //Change volume and pitch accordingly
                screechingAS.volume = Mathf.Lerp(screechingAS.volume, 0.5f, Time.deltaTime * 10);
                screechingAS.volume = Mathf.Clamp(screechingAS.volume, 0.0f, 0.5f);

                screechPitch = Mathf.Lerp(screechPitch, 1.2f, Time.deltaTime * 10);
                screechingAS.pitch = Mathf.Lerp(screechingAS.pitch, screechPitch, Time.deltaTime * 10);
            }
            //If lateral velocity is is high enough
            else
            {
                //Change volume and pitch accordingly
                screechingAS.volume = Mathf.Abs(lv) * 0.03f;
                screechingAS.volume = Mathf.Clamp(screechingAS.volume, 0.0f, 0.8f);

                screechPitch = Mathf.Abs(lv) * 0.02f;
                screechingAS.pitch = Mathf.Lerp(screechingAS.pitch, screechPitch, Time.deltaTime * 10);
            }
        }
        else screechingAS.volume = Mathf.Lerp(screechingAS.volume, 0.0f, Time.deltaTime * 10);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float relativeVelocity = collision.relativeVelocity.magnitude;

        float vol = relativeVelocity * 0.1f;

        hitAS.pitch = Random.Range(0.9f,1.1f);
        hitAS.volume = vol;

        if (!hitAS.isPlaying)
            hitAS.Play();
    }
}
