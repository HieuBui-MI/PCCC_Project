using UnityEngine;

public class DectectFlame : MonoBehaviour
{
    public float detectionRadius = 5f; 
    public LayerMask detectableLayers; 
    public float waterDamgage = 10f; // Giá trị thiệt hại từ nước

    void Start()
    {
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
                flamePoint.DescreaseDegreeOfCombustion(waterDamgage); // Giảm độ cháy của điểm lửa
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
