using UnityEngine;

public class FirePointShooter : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab của viên đạn (hoặc giọt nước)
    public Transform firePoint; // Vị trí bắn
    public float projectileSpeed = 0f; // Tốc độ bay ban đầu
    public float maxProjectileSpeed = 20f; // Tốc độ tối đa
    public float speedIncreaseRate = 5f; // Tốc độ tăng dần
    public float speedDecreaseRate = 5f; // Tốc độ giảm dần
    public float fireRate = 0.2f; // Tốc độ bắn (giây)
    public float destroyTime = 3f; // Thời gian hủy sau khi bắn

    private float nextFireTime = 0f;

    void Update()
    {
        // Kiểm tra nếu nhấn giữ chuột trái
        if (Input.GetMouseButton(0))
        {
            // Tăng dần tốc độ
            projectileSpeed = Mathf.Min(projectileSpeed + speedIncreaseRate * Time.deltaTime, maxProjectileSpeed);

            // Bắn nếu đủ thời gian
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            // Giảm dần tốc độ khi thả chuột
            projectileSpeed = Mathf.Max(projectileSpeed - speedDecreaseRate * Time.deltaTime, 0f);
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
