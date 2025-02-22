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
        
        yield return new WaitForSeconds(boostDuration);

        //reset speed and disable stinger
        boosting = false;
        stinger.gameObject.SetActive(false);
        currentSpeed = baseSpeed;
        
        yield return new WaitForSeconds(boostCooldown);

        //allow boosting again
        boostOnCooldown = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        //if pressing E, act as if you hit a stingable target
        //(for debugging, will implement targeting system later)
        if (!stunned && boosting && Input.GetKey(KeyCode.E))
        {
            StartCoroutine(Sting());
            return;
        }

        //if not stinging, bounce away
        BounceAndStun(collision.contacts[0].point);
    }



    void BounceAndStun(Vector3 collisionPoint)
    {
        //stun lasts longer if boosting
        float duration = boosting ? stunDurationWhileBoosting : stunDuration;
        StartCoroutine(Stun(duration));

        //bounce away from collision point
        var collisionDirection = transform.position - collisionPoint;
        collisionDirection.y = (collisionDirection.y * bounceYmultiplier);
        collisionDirection = collisionDirection.normalized + Vector3.up * bounceYadd;

        rb.angularVelocity = Vector3.zero;
        rb.AddForce(collisionDirection.normalized * bounceForce);

    }

    IEnumerator Sting()
    {
        Debug.Log("Stinging");

        //freeze movement for a bit
        var before = rb.constraints;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(stingDuration);
        rb.constraints = before;
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

}
