using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    [SerializeField] private ParticleSystem extinguisherParticles;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!extinguisherParticles.isEmitting)
            {
                extinguisherParticles.Play();
            }
        }
        else
        {
            if (extinguisherParticles.isEmitting)
            {
                extinguisherParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

}
