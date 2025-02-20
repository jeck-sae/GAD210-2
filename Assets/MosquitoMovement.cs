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

    [SerializeField, DisableInEditorMode] float currentSpeed;

    [SerializeField] float sensitivity = 1;
    [SerializeField] float boostSensitivityMultiplier = 0.2f;
    
    [SerializeField] GameObject cameraParent;

    [SerializeField] GameObject sting;    
    Rigidbody rb;

    private bool boosting;
    private bool canBoost = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = baseSpeed;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = cameraParent.transform.forward * currentSpeed;

    }


    private void Update()
    {
        if(canBoost && Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(Boost());


        float currentSensitivity = sensitivity * (boosting ? boostSensitivityMultiplier : 1);

        cameraParent.transform.rotation *= Quaternion.Euler(new Vector3(-Input.mousePositionDelta.y * currentSensitivity, 0));
        transform.rotation *= Quaternion.Euler(new Vector3(0, Input.mousePositionDelta.x * currentSensitivity));
        
    }


    IEnumerator Boost()
    {
        canBoost = false;
        boosting = true;
        currentSpeed = boostSpeed;
        yield return new WaitForSeconds(boostDuration);
        currentSpeed = baseSpeed;
        boosting = false;
        yield return new WaitForSeconds(boostCooldown);
        canBoost = true;
    }
}
