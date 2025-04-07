using UnityEngine;

public class ParticleScrt : MonoBehaviour
{
    public float damage = 1f;
    void OnParticleCollision(GameObject other)
    {
        virtualSc vir = other.GetComponent<virtualSc>();
        if (vir != null)
        {
            vir.DealDamage(damage);
        }
    }
}
