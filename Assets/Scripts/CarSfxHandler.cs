using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSfxHandler : MonoBehaviour
{
    [Header("Audio sources")]
    public AudioSource tiresScreachingAS;
    public AudioSource engineAS;
    public AudioSource carHitAS;

    TopDownCarController topDownCarController;
    // Start is called before the first frame update
    void Start()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateTiresScreechingSFX();
    }

    private void UpdateEngineSFX() {
        float velocityMagnitude = topDownCarController.GetVelocityMagnitude();
    }

    private void UpdateTiresScreechingSFX() {
        throw new NotImplementedException();
    }
}
