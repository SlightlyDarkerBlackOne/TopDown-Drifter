using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelParticleHandler : MonoBehaviour
{
    float particleEmmisionRate = 0;

    TopDownCarController topDownCarController;

    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule emissionModule;
    private void Awake() {
        topDownCarController = GetComponentInParent<TopDownCarController>();

        particleSystemSmoke = GetComponent<ParticleSystem>();

        emissionModule = particleSystemSmoke.emission;

        emissionModule.rateOverTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        particleEmmisionRate = Mathf.Lerp(particleEmmisionRate, 0, Time.deltaTime * 5);
        emissionModule.rateOverTime = particleEmmisionRate;

        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking)) {
            if (isBraking) {
                particleEmmisionRate = 30;
            } else {
                particleEmmisionRate = Mathf.Abs(lateralVelocity) * 2;
            }
        }
    }
}
