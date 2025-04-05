using StarterAssets;
using UnityEngine;

public class FirePointShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float fireRate = 0.2f;
    public float destroyTime = 3f;
    private float nextFireTime = 0f;

    /////////////////////////////////////////
    [SerializeField] private GameObject obiEmitter;

    void Awake()
    {
        obiEmitter = transform.Find("Obi Emitter")?.gameObject;
        firePoint = transform.Find("FirePoint")?.transform;
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        ToggleObiEmitter();
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0) && firePoint != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.linearVelocity = firePoint.up * projectileSpeed;
            }
            Destroy(projectile, destroyTime);
        }
    }

    void ToggleObiEmitter()
    {
        if (obiEmitter != null)
        {
            obiEmitter.SetActive(Input.GetKey(KeyCode.Mouse0));
        }
    }
}
