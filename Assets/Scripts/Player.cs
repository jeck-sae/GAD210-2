using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float healthRegenPerSting;

    [SerializeField] Scrollbar healthScrollbar;

    private void Awake()
    {
        GetComponentInChildren<TargetDetection>(true).OnTargetDetected += OnStinging;
        health = maxHealth;
    }
    
    private void Update()
    {
        health -= Time.deltaTime;
        UpdateUI();
        if (health <= 0)
            GameManager.Instance.GoodEnding();
    }

    protected void OnStinging()
    {
        health = Mathf.Clamp(health + healthRegenPerSting, 0, maxHealth);
        UpdateUI();
    }

    void UpdateUI()
    {
        healthScrollbar.size = health/maxHealth;
    }
}
