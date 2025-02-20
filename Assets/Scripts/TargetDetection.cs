using UnityEngine;
using System.IO;

public class TargetDetection : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {

        if (!gameObject.activeSelf)
        {
            return;
        }

        if(other.CompareTag("Target"))
        {
            Debug.Log("Target Found" + other.gameObject.name);
        }
    }

    private void ApplyEffects()
    {
        // apply any required effects

        // increase score 
    }

    private void OnTriggerExit(Collider other)
    {
        //any exit effects 
    }


}
