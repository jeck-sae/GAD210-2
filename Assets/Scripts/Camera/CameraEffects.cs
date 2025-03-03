using System.Collections;
using UnityEngine;

public class CameraEffects : Singleton<CameraEffects>
{
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    IEnumerator ShakeCoroutine(float duration, float intensity)
    {
        Vector3 originalRotation = mainCamera.transform.localEulerAngles;
        //Vector3 originalPosition = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-intensity, intensity);
            float y = Random.Range(-intensity, intensity);
            mainCamera.transform.localEulerAngles = originalRotation + new Vector3(x, y, 0);
            //mainCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localEulerAngles = originalRotation;
        //mainCamera.transform.localPosition = originalPosition;
    }

    public void ChangeFOV(float targetFOV, float duration)
    {
        StartCoroutine(ChangeFOVCoroutine(targetFOV, duration));
    }

    IEnumerator ChangeFOVCoroutine(float targetFOV, float duration)
    {
        float startFOV = mainCamera.fieldOfView;
        float time = 0;

        while (time < 1f)
        {
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, time);
            time += Time.deltaTime * duration;
            yield return null;
        }

        mainCamera.fieldOfView = targetFOV;
    }

}
