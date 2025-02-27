using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MosquitoMovement : MonoBehaviour
{

    [SerializeField] float baseSpeed = 10;
    [SerializeField] float boostSpeed = 30;
    [SerializeField] float boostDuration = 3;
    [SerializeField] float boostCooldown = 3;

    [SerializeField] Camera mainCamera;
    [SerializeField] float boostFOV = 90; // Increased FOV during boost
    [SerializeField] float normalFOV = 60; // Default FOV
    [SerializeField] float fovLerpSpeed = 5f; // How fast FOV transitions

    [SerializeField] float cameraShakeIntensity = 0.1f;
    [SerializeField] float cameraShakeDuration = 0.2f; 

    [SerializeField] float bounceForce = 100;
    [SerializeField] float stunDuration = 0.35f;
    [SerializeField] float bounceForceWhileBoosting = 100;
    [SerializeField] float stunDurationWhileBoosting = 1f;

    [SerializeField] float bounceYmultiplier = .1f;
    [SerializeField] float bounceYadd = 1;
    [SerializeField] float stingDuration = 1;

    [SerializeField] float sensitivity = 1;
    [SerializeField] float boostSensitivityMultiplier = 0.2f;
    
    [SerializeField] GameObject cameraParent;
    [SerializeField] GameObject stinger;    
    Rigidbody rb;

    [SerializeField, DisableInEditorMode] float currentSpeed;
    private bool boosting;
    private bool boostOnCooldown;
    private bool stunned;

    private float cameraRotationX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = baseSpeed;
        StartCoroutine(FixStartRotation());
        GetComponentInChildren<TargetDetection>(true).OnTargetDetected += OnTargetDetected;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        } 
    }

    private void FixedUpdate()
    {
        //handle movement
        if(!stunned)
            rb.linearVelocity = cameraParent.transform.forward * currentSpeed;
    }

    private void Update()
    {
        //boost if pressing space and in the right conditions
        if(Input.GetKeyDown(KeyCode.Space) && !boostOnCooldown && !stunned)
            StartCoroutine(Boost());

        //reduce camera sensitivity while boosting
        float currentSensitivity = sensitivity * (boosting ? boostSensitivityMultiplier : 1);

        //clamp the X rotation of the camera (to avoid getting upside down)
        cameraRotationX -= Input.mousePositionDelta.y * currentSensitivity;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);
        cameraParent.transform.localEulerAngles = Vector3.right * cameraRotationX;

        //rotate camera Y
        transform.rotation *= Quaternion.Euler(new Vector3(0, Input.mousePositionDelta.x * currentSensitivity));
    }

    private void OnCollisionEnter(Collision collision)
    {
        BounceAndStun(collision.contacts[0].point);
    }

    IEnumerator FixStartRotation()
    {
        //resets camera rotation after 2 frames
        //because the game would start facing down
        yield return null;
        yield return null;
        cameraRotationX = 0;
    }

    IEnumerator Boost()
    {
        //increase speed and enable stinger
        boostOnCooldown = true;
        boosting = true;
        stinger.gameObject.SetActive(true);
        currentSpeed = boostSpeed;

        StartCoroutine(ChangeFOV(boostFOV));

        StartCoroutine(CameraShake(cameraShakeDuration, cameraShakeIntensity)); 

        yield return new WaitForSeconds(boostDuration);

        //reset speed and disable stinger
        boosting = false;
        stinger.gameObject.SetActive(false);
        currentSpeed = baseSpeed;

        StartCoroutine(ChangeFOV(normalFOV)); 

        yield return new WaitForSeconds(boostCooldown);

        //allow boosting again
        boostOnCooldown = false;
    }

    IEnumerator ChangeFOV(float targetFOV)
    {
        float startFOV = mainCamera.fieldOfView;
        float time = 0;

        while (time < 1f)
        {
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, time);
            time += Time.deltaTime * fovLerpSpeed;
            yield return null;
        }

        mainCamera.fieldOfView = targetFOV;
    }

    IEnumerator CameraShake(float duration, float intensity)
    {
        Vector3 originalPosition = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-intensity, intensity);
            float y = Random.Range(-intensity, intensity);
            mainCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPosition;
    } 
    void BounceAndStun(Vector3 collisionPoint)
    {
        //stun lasts longer if boosting
        float duration = boosting ? stunDurationWhileBoosting : stunDuration;
        float force = boosting ? bounceForceWhileBoosting : bounceForce;
        StartCoroutine(Stun(duration));

        //bounce away from collision point
        var collisionDirection = transform.position - collisionPoint;
        collisionDirection.y = (collisionDirection.y * bounceYmultiplier);
        collisionDirection = collisionDirection.normalized + Vector3.up * bounceYadd;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(collisionDirection.normalized * force);
    }

    IEnumerator Stun(float duration)
    {
        //stop movement and enable gravity
        boosting = false;
        stunned = true;
        rb.useGravity = true;

        yield return new WaitForSeconds(duration);

        //resetting speed in case it was boosting
        //and the effect was still active
        currentSpeed = baseSpeed; 
        rb.useGravity = false;
        stunned = false;
    }

    private void OnTargetDetected()
    {
        StartCoroutine(Sting());
    }

    IEnumerator Sting()
    {
        //freeze movement for a bit
        var before = rb.constraints;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(stingDuration);
        rb.constraints = before;
    }

}
