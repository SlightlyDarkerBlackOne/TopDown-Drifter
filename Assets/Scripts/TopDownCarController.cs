using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopDownCarController : MonoBehaviour
{
    [Header("Car setting")]
    public float driftFactor;
    public float accelerationFactor = 30.0f;

    public float turnFactor = 3.5f;
    public float maxSpeed;

    [Header("UI")]
    public TextMeshProUGUI carSpeedText;

    float accelerationInput = 0;
    float steeringInput = 0;

    float rotationAngle = 0;

    float velocityVsUp = 0;

    Rigidbody2D rb;

    Vector2 engineForceVector;

    public static TopDownCarController Instance;

    // Start is called before the first frame update
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        if (Instance == this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }
    private void Update() {
        carSpeedText.SetText(Math.Round((rb.velocity.magnitude * 7)).ToString() + "km/h");
    }
    private void FixedUpdate() {
        ApplyEngineForce();

        ApplySteering();
    }

    private void ApplyEngineForce() {
        velocityVsUp = Vector2.Dot(transform.up, rb.velocity);

        if(velocityVsUp > maxSpeed && accelerationInput > 0) {
            return;
        }

        if (velocityVsUp < - maxSpeed * 0.4f && accelerationInput < 0) {
            return;
        }

        if(rb.velocity.sqrMagnitude > maxSpeed *maxSpeed && accelerationInput > 0) {
            return;
        }

        if (accelerationInput == 0) {
            rb.drag = Mathf.Lerp(rb.drag, 3.0f, Time.fixedDeltaTime * 3);
        } else {
            rb.drag = 0.81f;
        }
        if(IsTireScreeching(out float lateralVelocity, out bool isBraking)) {
            rb.drag = Mathf.Lerp(rb.drag, 3.0f, Time.fixedDeltaTime * 3);
        } else {
            rb.drag = 0.81f;
        }
        engineForceVector = transform.up * accelerationInput * accelerationFactor;

        rb.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ApplySteering() {
        float minSpeedBeforeAllowTurningFactor = rb.velocity.magnitude / 8;
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        rb.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity() {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    float GetLateralVelocity() {
        return Vector2.Dot(transform.right, rb.velocity);
    }
    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking) {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if(accelerationInput < 0 && velocityVsUp > 0) {
            isBraking = true;
            return true;
        }
        if(Mathf.Abs(GetLateralVelocity()) > 4.0f){
            return true;
        }
        return false;
    }

    public void SetInputVector(Vector2 inputVector) {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public float GetVelocityMagnitude() {
        return rb.velocity.magnitude;
    }
}
