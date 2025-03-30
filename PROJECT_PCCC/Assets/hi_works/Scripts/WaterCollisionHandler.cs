using UnityEngine;

public class WaterCollisionHandler : MonoBehaviour
{
    public LayerMask HittableLayers; 
    public GameObject spawnPrefab;  

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & HittableLayers) != 0)
        {
            // Spawn prefab tại vị trí va chạm
            if (spawnPrefab != null)
            {
                Vector3 spawnPosition = other.ClosestPoint(transform.position); // Lấy vị trí gần nhất
                Quaternion spawnRotation = Quaternion.identity; // Hoặc sử dụng rotation khác nếu cần
                GameObject spawnedObject = Instantiate(spawnPrefab, spawnPosition, spawnRotation);

                // Xóa prefab sau 3 giây
                Destroy(spawnedObject, 1f);
            }
        }
    }
}