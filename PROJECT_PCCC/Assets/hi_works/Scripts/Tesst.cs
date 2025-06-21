using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootForce = 10f;
    public float shootInterval = 0.5f; // thời gian giữa mỗi phát bắn

    void Start()
    {
        InvokeRepeating(nameof(Shoot), 0f, shootInterval);
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(firePoint.forward * shootForce, ForceMode.Impulse);
            }

            // Huỷ viên đạn sau 3 giây
            Destroy(projectile, 3f);
        }
    }
}
