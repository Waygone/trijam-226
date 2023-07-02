using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region VARIABLES
    [Header("Movement")]
    public float acceleration = 8.0f;
    public float maxSpeed = 60.0f;
    public float turnSpeed = 4.0f;
    //Don't put above 1
    public float drift = 0.95f;
    public float dragForce = 3.0f;

    [Header("Trails")]
    public float trailTurnTreshold = 0.8f;



    float accelInput = 0;
    float turningInput = 0;


    float rotationAngle = 0;
    float velocityUp = 0;
    
    Rigidbody2D rb;
    #endregion 

    #region CALLBACK
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        AccelerationForce();
        ApplyFriction();
        SteeringForce();
    }
    #endregion


    #region MOVEMENT
    private void AccelerationForce()
    {
        //Calculate forward with direction of velocity
        velocityUp = Vector2.Dot(transform.up, rb.velocity);

        //Limit speed
        if (velocityUp > maxSpeed && acceleration > 0) return;

        //Limit reverse to 50% of top speed
        if(velocityUp < -maxSpeed * 0.5f && accelInput > 0) return;

        //Limit directional speed while accelerating
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelInput > 0) return;

        //Drag if it has no acceleration
        if (accelInput == 0)
            rb.drag = Mathf.Lerp(rb.drag, 3.0f, Time.fixedDeltaTime * dragForce);
        else rb.drag = 0;

        Vector2 forceVector = transform.up * accelInput * acceleration;

        rb.AddForce(forceVector, ForceMode2D.Force);

    }

    private void SteeringForce()
    {
        //Don't turn if it isn't accelerating
        float minSpeedBeforeTurning = rb.velocity.magnitude / 8;
        minSpeedBeforeTurning = Mathf.Clamp01(minSpeedBeforeTurning);

        rotationAngle -= turningInput * (turnSpeed * 10 /acceleration) * minSpeedBeforeTurning;

        rb.MoveRotation(rotationAngle);
    }
    public void ApplyFriction()
    {
        Vector2 forwardVel = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVel = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = (forwardVel + rightVel) * drift;
    }
    #endregion

    #region HANDLERS

    public float GetLateralVelocity()
    {
        //How fast is going "right"
        return Vector2.Dot(transform.right, rb.velocity);
    }

    public bool IsTireScreeching(out float latVel)
    {
        latVel = GetLateralVelocity();
        bool isTrailing = false;

        if((accelInput < trailTurnTreshold && velocityUp > -trailTurnTreshold) || (accelInput > -trailTurnTreshold && velocityUp < trailTurnTreshold))
            isTrailing = true;
        if (Mathf.Abs(GetLateralVelocity()) > trailTurnTreshold)
            isTrailing = true;

        return isTrailing;
    }

    #endregion


    #region INPUT

    public void SetInputVector(Vector2 inputVector)
    {
        turningInput = inputVector.x;
        accelInput = inputVector.y;
    }

    #endregion
}
