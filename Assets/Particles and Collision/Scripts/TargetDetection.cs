using UnityEngine;
using System.IO;
using System;

public class TargetDetection : MonoBehaviour
{
    public Action OnTargetDetected;

    private void OnTriggerEnter(Collider other)
    {
        Target target = other.GetComponent<Target>();

        if (!gameObject.activeSelf)
        {
            return;
        }

        if(other.CompareTag("Target"))
        {
            if(target != null && target.CanBeStung())
            {
                Debug.Log("Target Found" + other.gameObject.name);

               
                target.MarkAsStrung();

                OnTargetDetected?.Invoke();

                Renderer targetRenderer = other.GetComponent<Renderer>();

                if (targetRenderer != null)
                {
                    targetRenderer.material.color = Color.white;
                   
                }

                Debug.Log("target has already been stung" + gameObject.name);
            }
            
        }
    }

    
}
