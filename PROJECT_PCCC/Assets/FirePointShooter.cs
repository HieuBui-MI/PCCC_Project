using UnityEngine;

public class FirePointShooter : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab của viên đạn (hoặc giọt nước)
    public Transform firePoint; // Vị trí bắn
    public float projectileSpeed = 10f; // Tốc độ bay
    public float fireRate = 0.2f; // Tốc độ bắn (giây)
    public float destroyTime = 3f; // Thời gian hủy sau khi bắn

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) // Nhấn chuột trái để bắn
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Tạo viên đạn tại FirePoint
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Kiểm tra nếu projectilePrefab chưa có Collider, thì thêm vào
        if (projectile.GetComponent<SphereCollider>() == null)
        {
            projectile.AddComponent<SphereCollider>().isTrigger = false; // Thêm Collider
        }

        // Kiểm tra nếu chưa có Rigidbody thì thêm vào
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = projectile.AddComponent<Rigidbody>();
        }
        rb.useGravity = true; // Không bị rơi xuống
        rb.linearVelocity = firePoint.forward * projectileSpeed; // Bắn theo hướng FirePoint

        // Hủy viên đạn sau destroyTime giây
        Destroy(projectile, destroyTime);
    }
}
