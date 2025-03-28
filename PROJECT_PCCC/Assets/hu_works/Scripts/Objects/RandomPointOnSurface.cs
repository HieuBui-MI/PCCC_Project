using System.Collections.Generic;
using UnityEngine;

public class RandomPointOnSurface : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector3[] normals; // Pháp tuyến của các đỉnh
    private List<Vector3> randomPoints = new List<Vector3>(); // Danh sách lưu các điểm ngẫu nhiên
    private List<GameObject> pointObjects = new List<GameObject>(); // Danh sách lưu các GameObject được tạo

    [SerializeField] private int numberOfPoints = 10; // Số lượng điểm ngẫu nhiên cần tạo
    [SerializeField] private GameObject pointPrefab; // Prefab để tạo tại các điểm ngẫu nhiên
    [SerializeField] public Vector3 rotationOffset = Vector3.zero; // Biến để kiểm soát độ nghiêng

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        triangles = mesh.triangles;
        normals = mesh.normals; // Lấy pháp tuyến của các đỉnh

        // Tạo các điểm ngẫu nhiên trên bề mặt mesh
        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 randomPoint = GetRandomPointOnMesh(out Vector3 normal); // Lấy điểm và pháp tuyến
            Vector3 worldPoint = transform.TransformPoint(randomPoint); // Chuyển đổi sang tọa độ thế giới
            Vector3 worldNormal = transform.TransformDirection(normal); // Chuyển đổi pháp tuyến sang tọa độ thế giới
            randomPoints.Add(worldPoint); // Lưu điểm vào danh sách

            // Tạo GameObject tại điểm ngẫu nhiên với hướng vuông góc và độ nghiêng
            CreatePointObject(worldPoint, worldNormal);
        }
    }

    Vector3 GetRandomPointOnMesh(out Vector3 normal)
    {
        // Chọn một tam giác ngẫu nhiên
        int triangleIndex = Random.Range(0, triangles.Length / 3) * 3;

        // Lấy ba đỉnh của tam giác
        Vector3 v0 = vertices[triangles[triangleIndex]];
        Vector3 v1 = vertices[triangles[triangleIndex + 1]];
        Vector3 v2 = vertices[triangles[triangleIndex + 2]];

        // Lấy pháp tuyến của ba đỉnh
        Vector3 n0 = normals[triangles[triangleIndex]];
        Vector3 n1 = normals[triangles[triangleIndex + 1]];
        Vector3 n2 = normals[triangles[triangleIndex + 2]];

        // Chọn ngẫu nhiên một điểm trong tam giác
        Vector3 point = RandomPointInTriangle(v0, v1, v2);

        // Tính pháp tuyến tại điểm ngẫu nhiên bằng cách nội suy
        float r1 = Mathf.Sqrt(Random.value);
        float r2 = Random.value;
        normal = (1 - r1) * n0 + (r1 * (1 - r2)) * n1 + (r1 * r2) * n2;
        normal.Normalize(); // Chuẩn hóa pháp tuyến

        return point;
    }

    Vector3 RandomPointInTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        float r1 = Mathf.Sqrt(Random.value);
        float r2 = Random.value;

        Vector3 point = (1 - r1) * v0 + (r1 * (1 - r2)) * v1 + (r1 * r2) * v2;
        return point;
    }

    void CreatePointObject(Vector3 position, Vector3 normal)
    {
        if (pointPrefab != null)
        {
            // Tạo đối tượng tại vị trí và đặt hướng theo pháp tuyến với độ nghiêng
            Quaternion rotation = Quaternion.LookRotation(normal) * Quaternion.Euler(rotationOffset);
            GameObject pointObject = Instantiate(pointPrefab, position, rotation);
            pointObject.transform.parent = GameObject.Find("GameManager").transform.Find("FlamePoints"); // Đặt cha cho GameObject
            pointObjects.Add(pointObject); 
        }
        else
        {
            Debug.LogWarning("Point prefab is not assigned!");
        }
    }
}