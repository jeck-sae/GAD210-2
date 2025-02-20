using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class Target : MonoBehaviour
{
    [SerializeField] TargetData targetData;

    private string objectName;
    private int objectScore;
    private Renderer objectColor;
    private ParticleSystem objectEffect;

    private BoxCollider box;

    private void Start()
    {

        box = GetComponent<BoxCollider>();
        box.isTrigger = true;


        objectName = targetData.targetName;
        objectScore = targetData.targetScore;

        objectColor = GetComponent<Renderer>();
        objectColor.material.color = targetData.targetColor;
        objectEffect = targetData.targetEffect;

    }
}
