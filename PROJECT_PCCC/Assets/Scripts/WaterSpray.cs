using UnityEngine;

public class WaterSpray : MonoBehaviour
{
    public ParticleSystem[] waterParticles; // Mảng chứa nhiều Particle Systems

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Khi nhấn giữ chuột trái
        {
            StartSpraying();
        }
        else if (Input.GetMouseButtonUp(0)) // Khi thả chuột trái
        {
            StopSpraying();
        }
    }

    void StartSpraying()
    {
        foreach (ParticleSystem ps in waterParticles)
        {
            if (!ps.isPlaying)
            {
                ps.Play();
            }
        }
    }

    void StopSpraying()
    {
        foreach (ParticleSystem ps in waterParticles)
        {
            if (ps.isPlaying)
            {
                ps.Stop();
            }
        }
    }
}
