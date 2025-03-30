using UnityEngine;

public class WaterCollisionHandler : MonoBehaviour
{
    public LayerMask HittableLayers; 
    public GameObject spawnPrefab;  

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & HittableLayers) != 0)
        {
            // Spawn prefab tại vị trí va chạm
            if (spawnPrefab != null)
            {
                Vector3 spawnPosition = collision.contacts[0].point; // Lấy vị trí va chạm
                Quaternion spawnRotation = Quaternion.identity; // Hoặc sử dụng rotation khác nếu cần
                GameObject spawnedObject = Instantiate(spawnPrefab, spawnPosition, spawnRotation);

                // Xóa prefab sau 3 giây
                Destroy(spawnedObject, 1f);
            }
        }
        else
        {
            Debug.Log("Hit something else!");
        }

        Destroy(gameObject);
    }
}
