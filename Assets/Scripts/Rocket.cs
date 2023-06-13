using System;
using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Rocket : MonoBehaviour
{
    [SerializeField]
    float rcsThrust = 100f,
                           mainThrust = 100f;
    [SerializeField]
    AudioClip mainEngine,
                               levelCompleted,
                               obstacleHit;
    [SerializeField]
    ParticleSystem mainEngineParticles,
                                    levelCompletedParticles,
                                    obstacleHitParticles;
    [SerializeField] float invokeWaitTime = 2.5f;

    bool collisionsDisabled = false,
         isTransitioning = false;

    Rigidbody rigidBody;
    AudioSource audioSource;

    private GazePoint gazePoint;
    GazeAware gazeAware = null;
    public TobiiSettings Settings;

    // Use this for initialization
    void Start()
    {
        TobiiAPI.Start(Settings);
        gazeAware = GetComponent<GazeAware>();

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        gazePoint = TobiiAPI.GetGazePoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {

            //gazePoint = TobiiAPI.GetGazePoint();


            //Debug.Log("Has gaze focus true");

           // Debug.Log("Has gaze focus valid");
            // Note: Values can be negative if the user looks outside the game view.
            Debug.Log("Gaze point on Screen (X,Y): " + gazePoint.Screen.x + ", " + gazePoint.Screen.y);
            RespondToThrustInput();
            RespondToRotateInput();

        }

        if (Debug.isDebugBuild)
        {
            RespondtoDebugInput();
        }
    }

    private void RespondtoDebugInput()
    {
        if (Input.GetKey(KeyCode.C))
            collisionsDisabled = !collisionsDisabled;
        if (Input.GetKey(KeyCode.L))
            LoadNextLevel();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsDisabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // DO NOTHING
                break;
            case "Finish":
                StartSuccessTransition();
                break;
            default:
                StartDeathTransition();
                break;
        }
    }

    private void StartDeathTransition()
    {
        isTransitioning = true;
        audioSource.Stop();
        rigidBody.constraints = RigidbodyConstraints.None;
        audioSource.PlayOneShot(obstacleHit);
        obstacleHitParticles.Play();
        Invoke("LoadFirstLevel", invokeWaitTime);
    }

    private void StartSuccessTransition()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(levelCompleted);
        levelCompletedParticles.Play();
        Invoke("LoadNextLevel", invokeWaitTime);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int nextBuildIndex = (currentBuildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextBuildIndex);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
        if (!mainEngineParticles.isPlaying)
            mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {

        // Stop any spin we had
        rigidBody.angularVelocity = Vector3.zero;
        gazePoint = TobiiAPI.GetGazePoint();

        // If we're not pressing A and D at the same time but one of them is being pressed then...
        if (!(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            float rotationThrustThisFrame = rcsThrust * Time.deltaTime;
            // ...we check if we're pressing the A key to rotate left
            //if (Input.GetKey(KeyCode.A))
            //{
            //    transform.Rotate(Vector3.forward * rotationThrustThisFrame);

            //    // ...if not we check if we're pressing the D key to rotate right
            //}
            //else if (Input.GetKey(KeyCode.D))
            //{
            //    transform.Rotate(-Vector3.forward * rotationThrustThisFrame);
            //}

            HeadPose headPose = TobiiAPI.GetHeadPose();

            if (headPose.Position.x < 0 && gazePoint.IsValid && gazePoint.IsRecent() == true && gazePoint.Screen.x <= 200)
            {
                Debug.Log("HeadPose Position (X,Y,Z): " + headPose.Position.x + ", " + headPose.Position.y + ", " + headPose.Position.z);
                Debug.Log("Rotating left");
                transform.Rotate(Vector3.forward * rotationThrustThisFrame);
            }
            else if (headPose.Position.x> 0 && gazePoint.IsValid && gazePoint.IsRecent() == true && gazePoint.Screen.x > 200)
            {
                Debug.Log("Rotating right");
                transform.Rotate(-Vector3.forward * rotationThrustThisFrame);
            }

            //// ...we check if we're pressing the A key to rotate left
            //if (Input.GetKey(KeyCode.A)) {
            //    transform.Rotate(Vector3.forward * rotationThrustThisFrame);

            //// ...if not we check if we're pressing the D key to rotate right
            //} else if (Input.GetKey(KeyCode.D)) {
            //    transform.Rotate(-Vector3.forward * rotationThrustThisFrame);
            //}
        }

    }
}
