using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

<<<<<<< HEAD
    [SerializeField] float rcsThrust = 100f,
                           mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;
=======
    Rigidbody rigidBody;
>>>>>>> a7bf533ff4921c462886a87ea1b4d4fa351218fe

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
<<<<<<< HEAD
        audioSource = GetComponent<AudioSource>();
=======
>>>>>>> a7bf533ff4921c462886a87ea1b4d4fa351218fe
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
    }

    private void Thrust() {
        
        // Take Manual Control of Rotation
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) {
<<<<<<< HEAD
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
                audioSource.Play();
        } else {
            audioSource.Stop();
=======
            rigidBody.AddRelativeForce(Vector3.up);
>>>>>>> a7bf533ff4921c462886a87ea1b4d4fa351218fe
        }

        // Release Manual Control of Rotation
        rigidBody.freezeRotation = false;
    }

    private void Rotate() {

        // If we're not pressing A and D at the same time but one of them is being pressed then...
        if (!(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))) {
            float rotationThrustThisFrame = rcsThrust * Time.deltaTime;

            // ...we check if we're pressing the A key to rotate left
            if (Input.GetKey(KeyCode.A)) {
                transform.Rotate(Vector3.forward * rotationThrustThisFrame);
            
            // ...if not we check if we're pressing the D key to rotate right
            } else if (Input.GetKey(KeyCode.D)) {
                transform.Rotate(-Vector3.forward * rotationThrustThisFrame);
            }
        }
        
    }
}
