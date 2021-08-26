using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform cameraTarget;

    public float cameraSpeed;

    public GameObject weatherEffect;
    private GameObject instantiatedEffect;

    //Defines an instance of the weather effect and changes its position in the FixedUpdate function the same as the camera
    private void Start() {
        cameraTarget = TopDownCarController.Instance.gameObject.transform;
        //instantiatedEffect = Instantiate(weatherEffect, transform.position, Quaternion.identity);
    }

    private void FixedUpdate() {
        if (cameraTarget != null) {
            var newPos = Vector2.Lerp(transform.position, cameraTarget.position,
                Time.deltaTime * cameraSpeed);

            var vect3 = new Vector3(newPos.x, newPos.y, -10f);


            transform.position = vect3;

            //instantiatedEffect.transform.position = transform.position;
        }
    }
}
