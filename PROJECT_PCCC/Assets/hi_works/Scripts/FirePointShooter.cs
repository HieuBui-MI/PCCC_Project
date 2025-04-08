using StarterAssets;
using UnityEngine;
using Obi;

public class FirePointShooter : MonoBehaviour
{
    [Header("Obi Settings")]
    private ObiEmitter obiEmitterComponent;
    private float targetSpeed = 0f;
    private float speedIncreaseRate = 10f;
    private float speedDecreaseRate = 20f;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f; // Tốc độ ban đầu của projectile
    public float maxProjectileSpeed = 25f; // Tốc độ tối đa của projectile
    public float fireRate = 0.2f;
    public float destroyTime = 3f;
    private float nextFireTime = 0f;

    [SerializeField] private Transform playerCameraRoot;


    /////////////////////////////////////////
    [SerializeField] private GameObject obiEmitter;

    void Awake()
    {
        obiEmitter = transform.Find("Obi Emitter")?.gameObject;
        firePoint = transform.Find("FirePoint")?.transform;

        if (obiEmitter != null)
        {
            obiEmitterComponent = obiEmitter.GetComponent<ObiEmitter>();
            if (obiEmitterComponent != null)
            {
                obiEmitterComponent.speed = 0f;
            }
        }
    }

    void Update()
    {

        // Bắn như bình thường
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        ControlObiEmitter();
        ControlProjectileSpeed();
    }


    void Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0) && firePoint != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                // rb.AddForce(firePoint.rotation * Vector3.up * projectileSpeed, ForceMode.VelocityChange);
                rb.AddForce(firePoint.up * projectileSpeed, ForceMode.VelocityChange);

            }
            Destroy(projectile, destroyTime);
        }
    }

    void ControlObiEmitter()
    {
        if (obiEmitterComponent == null) return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            targetSpeed = 25f;
            obiEmitterComponent.speed = Mathf.MoveTowards(
                obiEmitterComponent.speed,
                targetSpeed,
                speedIncreaseRate * Time.deltaTime
            );
        }
        else
        {
            targetSpeed = 0f;
            obiEmitterComponent.speed = Mathf.MoveTowards(
                obiEmitterComponent.speed,
                targetSpeed,
                speedDecreaseRate * Time.deltaTime
            );
        }
    }

    void ControlProjectileSpeed()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // Tăng dần tốc độ của projectile khi giữ chuột trái
            projectileSpeed = Mathf.MoveTowards(
                projectileSpeed,
                maxProjectileSpeed,
                speedIncreaseRate * Time.deltaTime
            );
        }
        else
        {
            // Giảm dần tốc độ của projectile khi thả chuột trái
            projectileSpeed = Mathf.MoveTowards(
                projectileSpeed,
                0f,
                speedDecreaseRate * Time.deltaTime
            );
        }
    }
}