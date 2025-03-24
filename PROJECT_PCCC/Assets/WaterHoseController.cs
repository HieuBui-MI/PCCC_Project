using UnityEngine;

public class WaterHoseController : MonoBehaviour
{
    public float emissionRate = 300f;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emissionModule;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        emissionModule = ps.emission;
        emissionModule.rateOverTime = 0f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            emissionModule.rateOverTime = emissionRate;
        }
        else
        {
            emissionModule.rateOverTime = 0f;
        }
    }
}