using UnityEngine;
using System.Collections.Generic;
using Obi;

public class FlameableHouse : MonoBehaviour
{
    [SerializeField] private List<GameObject> houseParts = new List<GameObject>(); // Danh sách các phần của ngôi nhà
    [SerializeField] private GameObject pointPrefab; // Prefab để gắn vào RandomPointOnSurface
    public int numberOfPointsEachPart;
    [SerializeField] public Vector3 rotationOffset = Vector3.zero; 

    void Start()
    {
        // Lấy tất cả các phần (child) của ngôi nhà
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child != transform) // Bỏ qua chính object cha (ngôi nhà)
            {
                houseParts.Add(child.gameObject);
            }
        }

        // Gán script RandomPointOnSurface vào từng phần
        AddRandomPointOnSurfaceToParts();
    }

    void AddRandomPointOnSurfaceToParts()
    {
        foreach (GameObject part in houseParts)
        {
            // Kiểm tra nếu part chưa có script RandomPointOnSurface
            RandomPointOnSurface randomPointScript = part.GetComponent<RandomPointOnSurface>();
            if (randomPointScript == null)
            {
                // Thêm script RandomPointOnSurface vào part
                randomPointScript = part.AddComponent<RandomPointOnSurface>();
            }

            // Kiểm tra nếu part chưa có collider
            ObiCollider collider = part.GetComponent<ObiCollider>();
            if (collider == null)
            {
                part.AddComponent<ObiCollider>();
            }

            // Gắn prefab vào script RandomPointOnSurface
            if (pointPrefab != null)
            {
                randomPointScript.pointPrefab = pointPrefab;
                randomPointScript.numberOfPoints = numberOfPointsEachPart; 
                randomPointScript.rotationOffset = rotationOffset; 
            }
        }

        Debug.Log("RandomPointOnSurface script has been added to all house parts and prefab has been assigned.");
    }
}