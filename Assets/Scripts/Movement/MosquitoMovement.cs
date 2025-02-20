using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] float sensitivity = 1;
    [SerializeField] float boostSensitivityMultiplier = 0.2f;
    
    [SerializeField] GameObject cameraParent;
    [SerializeField] GameObject sting;    
    Rigidbody rb;

    [SerializeField, DisableInEditorMode] float currentSpeed;
    private bool boosting;
    private bool boostOnCooldown;
    private bool stunned;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = baseSpeed;
    }

    private void FixedUpdate()
    {
        if(!stunned)
            rb.linearVelocity = cameraParent.transform.forward * currentSpeed;

    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !boostOnCooldown && !stunned)
            StartCoroutine(Boost());


        float currentSensitivity = sensitivity * (boosting ? boostSensitivityMultiplier : 1);

        cameraParent.transform.rotation *= Quaternion.Euler(new Vector3(-Input.mousePositionDelta.y * currentSensitivity, 0));
        transform.rotation *= Quaternion.Euler(new Vector3(0, Input.mousePositionDelta.x * currentSensitivity));
        
    }


    IEnumerator Boost()
    {
        boostOnCooldown = true;
        boosting = true;
        sting.gameObject.SetActive(true);
        currentSpeed = boostSpeed;
        
        yield return new WaitForSeconds(boostDuration);
        
        boosting = false;
        sting.gameObject.SetActive(false);
        currentSpeed = baseSpeed;
        
        yield return new WaitForSeconds(boostCooldown);
        
        boostOnCooldown = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        //if(stung) return;
        if (!stunned)
        {
            stunned = true;
            float duration = boosting ? stunDurationWhileBoosting : stunDuration;
            StartCoroutine(Stun(duration));

            var collisionDirection = transform.position - collision.contacts[0].point;
            collisionDirection.y = (collisionDirection.y * bounceYmultiplier);
            collisionDirection = collisionDirection.normalized + Vector3.up * bounceYadd;

            rb.angularVelocity = Vector3.zero;
            rb.AddForce(collisionDirection.normalized * bounceForce);
        }

    }

    IEnumerator Stun(float duration)
    {
        rb.useGravity = true;

        yield return new WaitForSeconds(duration);
        
        currentSpeed = baseSpeed;
        rb.useGravity = false;
        stunned = false;

    }

}
