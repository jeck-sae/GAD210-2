using UnityEngine;


public class Target : MonoBehaviour
{
    [SerializeField] TargetData targetData;

    private string objectName;
    private int objectScore;
    private Material targetMaterial;
    [SerializeField] ParticleSystem stingEffect;
    MeshRenderer meshRenderer;

    private bool canBeStung = true;


    public void Start()
    {

        objectName = targetData.targetName;
        objectScore = targetData.targetScore;
       

    }

    

    public bool CanBeStung()
    {
        
        return canBeStung;
    }

    public void MarkAsStrung()
    {

        stingEffect.Play();

        ChangeMaterial();

        canBeStung = false;

    }

    public void ChangeMaterial()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        targetMaterial = targetData.hitMaterial;
        targetData.hitMaterial.mainTextureScale = targetMaterial.mainTextureScale;

        meshRenderer.material.SetFloat("_Temperature", 95f);
    }
}
