using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class Target : MonoBehaviour
{
    [SerializeField] TargetData targetData;

    private string objectName;
    private int objectScore;
    private Renderer objectColor;
    [SerializeField] ParticleSystem stingEffect;

    private bool canBeStung = true;

    private BoxCollider box;

    public void Start()
    {

        box = GetComponent<BoxCollider>();


        objectName = targetData.targetName;
        objectScore = targetData.targetScore;

        objectColor = GetComponent<Renderer>();
        objectColor.material.color = targetData.targetColor;
      

    }

    

    public bool CanBeStung()
    {
        
        return canBeStung;
    }

    public void MarkAsStrung()
    {

        stingEffect.Play();

        canBeStung = false;

        

    }
}
