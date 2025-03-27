using System.Collections.Generic;
using UnityEngine;

public class RandomPointOnSurface : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private List<Vector3> randomPoints = new List<Vector3>(); // Danh sách lưu các điểm ngẫu nhiên
    private List<GameObject> pointObjects = new List<GameObject>(); // Danh sách lưu các GameObject được tạo

    [SerializeField] private int numberOfPoints = 10; // Số lượng điểm ngẫu nhiên cần tạo
    [SerializeField] private GameObject pointPrefab; // Prefab để tạo tại các điểm ngẫu nhiên

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        triangles = mesh.triangles;

        // Tạo các điểm ngẫu nhiên trên bề mặt mesh
        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 randomPoint = GetRandomPointOnMesh();
            Vector3 worldPoint = transform.TransformPoint(randomPoint); // Chuyển đổi sang tọa độ thế giới
            randomPoints.Add(worldPoint); // Lưu điểm vào danh sách

            // Tạo GameObject tại điểm ngẫu nhiên
            CreatePointObject(worldPoint);
        }
    }

    Vector3 GetRandomPointOnMesh()
    {
        // Chọn một tam giác ngẫu nhiên
        int triangleIndex = Random.Range(0, triangles.Length / 3) * 3;

        // Lấy ba đỉnh của tam giác
        Vector3 v0 = vertices[triangles[triangleIndex]];
        Vector3 v1 = vertices[triangles[triangleIndex + 1]];
        Vector3 v2 = vertices[triangles[triangleIndex + 2]];

        // Chọn ngẫu nhiên một điểm trong tam giác
        return RandomPointInTriangle(v0, v1, v2);
    }

    Vector3 RandomPointInTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        float r1 = Mathf.Sqrt(Random.value);
        float r2 = Random.value;

        Vector3 point = (1 - r1) * v0 + (r1 * (1 - r2)) * v1 + (r1 * r2) * v2;
        return point;
    }

    void CreatePointObject(Vector3 position)
    {
        if (pointPrefab != null)
        {
            GameObject pointObject = Instantiate(pointPrefab, position, Quaternion.identity); // Tạo GameObject tại vị trí
            pointObjects.Add(pointObject); // Lưu GameObject vào danh sách
        }
        else
        {
            Debug.LogWarning("Point prefab is not assigned!");
        }
    }
}