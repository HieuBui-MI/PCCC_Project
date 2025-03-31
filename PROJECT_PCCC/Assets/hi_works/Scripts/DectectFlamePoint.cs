using UnityEngine;

public class DectectFlame : MonoBehaviour
{
    public float detectionRadius = 5f; 
    public LayerMask detectableLayers; 

    void Start()
    {
        // Thực hiện phát hiện một lần khi script được kích hoạt
        DetectFlamePoints();
    }

    void DetectFlamePoints()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectableLayers);

        foreach (Collider hitCollider in hitColliders)
        {
            FlamePoint flamePoint = hitCollider.GetComponent<FlamePoint>();
            if (flamePoint != null)
            {
                Destroy(flamePoint.gameObject); // Xóa đối tượng FlamePoint nếu phát hiện
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
