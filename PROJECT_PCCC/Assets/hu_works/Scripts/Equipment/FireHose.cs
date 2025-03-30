using UnityEngine;

public class FireHose : MonoBehaviour
{
    [SerializeField] private GameObject spherePrefab; // Prefab của sphere
    [SerializeField] private float fireRate = 0.5f; // Tốc độ bắn (thời gian giữa các lần bắn)
    [SerializeField] private float fireForce = 10f; // Lực bắn của sphere
    [SerializeField] private Transform firePoint; // Vị trí bắn (nơi sphere được tạo ra)

    private float fireTimer; // Bộ đếm thời gian để kiểm soát tốc độ bắn

    void Update()
    {
        // Tăng bộ đếm thời gian
        fireTimer += Time.deltaTime;

        // Kiểm tra nếu đã đến lúc bắn
        if (fireTimer >= fireRate)
        {
            Fire(); // Gọi hàm bắn
            fireTimer = 0f; // Đặt lại bộ đếm thời gian
        }
    }

    void Fire()
    {
        if (spherePrefab != null && firePoint != null)
        {
            // Tạo sphere tại vị trí firePoint
            GameObject sphere = Instantiate(spherePrefab, firePoint.position, firePoint.rotation);

            // Thêm lực đẩy theo hướng Y của firePoint
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(firePoint.up * fireForce, ForceMode.Impulse);
            }
        }
        else
        {
            Debug.LogWarning("SpherePrefab or FirePoint is not assigned!");
        }
    }
}