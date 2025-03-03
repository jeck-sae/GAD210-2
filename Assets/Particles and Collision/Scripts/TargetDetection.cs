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

                Renderer renderer = GetComponent<Renderer>();

                Material objectMaterial = renderer.material;

                if (renderer != null)
                {
                    objectMaterial.SetFloat("Temperature", 100f);
                    Debug.Log("color changed");

                }

                Debug.Log("target has already been stung" + gameObject.name);
            }
            
        }
    }

    
}
