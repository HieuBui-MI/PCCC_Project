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
                // Tìm MeshRenderer hoặc Material của đối tượng và đổi màu sang đỏ
                MeshRenderer meshRenderer = hitCollider.transform.Find("Sphere").GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.material.color = Color.red;
                    hitCollider.transform.Find("Sphere").localScale = new Vector3(1f, 1f, 1f); // Thay đổi kích thước của Sphere
                }
                Debug.Log("Total Flame Points: " + hitColliders.Length);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
