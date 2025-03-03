using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoostCooldownDisplayer : MonoBehaviour
{
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] Image progressImage;
    MosquitoMovement mosquitoMovement;
    [SerializeField] Color readyColor;
    [SerializeField] Color cooldownColor;

    void Start()
    {
        mosquitoMovement = FindAnyObjectByType<MosquitoMovement>();
        mosquitoMovement.OnBoostStart += OnBoostStart;
        progressImage.color = readyColor;
    }

    private void OnBoostStart(float duration, float cooldown)
    {
        StartCoroutine(UpdateCooldown(duration, cooldown));
    }

    private IEnumerator UpdateCooldown(float duration, float cooldown)
    {
        progressImage.color = cooldownColor;
        scrollbar.size = 1;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            scrollbar.size = 1 - time / duration;
            yield return null;
        }
        time = 0;
        while (time < cooldown)
        {
            time += Time.deltaTime;
            scrollbar.size = time / cooldown;
            yield return null;
        }
        progressImage.color = readyColor;
    }

}
