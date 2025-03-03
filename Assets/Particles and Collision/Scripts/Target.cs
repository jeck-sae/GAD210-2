using UnityEngine;


public class Target : MonoBehaviour
{
    [SerializeField] TargetData targetData;

    private string objectName;
    private int objectScore;
    private Renderer objectColor;
    [SerializeField] ParticleSystem stingEffect;

    private bool canBeStung = true;


    public void Start()
    {


        objectName = targetData.targetName;
        objectScore = targetData.targetScore;





        Renderer renderer = GetComponent<Renderer>();

        Material objectMaterial = renderer.material;

        objectMaterial.SetFloat("Temperature", 100f);
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
