using UnityEngine;

public class GasTank : MonoBehaviour
{
    private bool isExploded = false;
    private float explosionRadius = 5f;
    private float explosionForce = 700f;
    private float explosionDamage = 50f;
    private float explosionDelay = 0.5f; // Delay before explosion in seconds

    [SerializeField] private GameObject explosionEffectPrefab; // Prefab Particle System

    private void Update()
    {
        // Kiểm tra nếu chưa phát nổ thì mới gọi TriggerExplosion
        if (!isExploded)
        {
            Invoke("TriggerExplosion", 5f);
        }
    }

    private void TriggerExplosion()
    {
        if (isExploded) return; // Nếu đã phát nổ, không làm gì cả

        isExploded = true;
        Explode();
    }

    private void Explode()
    {
        // Spawn the explosion effect
        float effectDuration = 0f; // Thời gian tồn tại của hiệu ứng nổ
        if (explosionEffectPrefab != null)
        {
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            explosionEffect.transform.SetParent(transform); // Đặt prefab làm con của GameObject hiện tại

            // Lấy thời gian tồn tại của Particle System (nếu có)
            ParticleSystem particleSystem = explosionEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                effectDuration = particleSystem.main.duration;
            }

            Destroy(explosionEffect, effectDuration);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Destroy the gas tank object after the effect duration
        Destroy(gameObject, effectDuration);
    }
}