using UnityEngine;
using System.Collections.Generic; // Thêm namespace để sử dụng List

public class SpiralObjectSpawn : MonoBehaviour
{
    [SerializeField] private GameObject spherePrefab; // Prefab của sphere
    [SerializeField] private float distance = 1f; // Khoảng cách giữa các điểm trong lưới vuông

    private Vector3 planeCenter; // Tọa độ trung tâm của mặt phẳng
    private List<GameObject> pointObjects = new List<GameObject>(); // Danh sách lưu các sphere đã tạo
    private float planeArea; // Diện tích của mặt phẳng
    private int numberOfSpheres; // Số lượng sphere cần tạo (tự động tính toán)

    void Start()
    {
        // Tính toán điểm giữa của mặt phẳng
        CalculatePlaneCenter();

        // Tính toán diện tích mặt phẳng và số lượng điểm
        CalculatePlaneArea();

        // Tạo các sphere theo hình xoắn ốc
        SpawnSpiral();
    }

    void CalculatePlaneCenter()
    {
        // Lấy collider của mặt phẳng (nếu có)
        Collider planeCollider = GetComponent<Collider>();
        if (planeCollider != null)
        {
            planeCenter = planeCollider.bounds.center; // Tâm của collider
        }
        else
        {
            // Nếu không có collider, sử dụng vị trí của object
            planeCenter = transform.position;
        }
    }

    void CalculatePlaneArea()
    {
        // Lấy collider của mặt phẳng (nếu có)
        Collider planeCollider = GetComponent<Collider>();
        if (planeCollider != null)
        {
            // Tính diện tích dựa trên kích thước của collider
            Vector3 size = planeCollider.bounds.size;
            planeArea = size.x * size.z; // Diện tích mặt phẳng (giả sử là hình chữ nhật)
        }
        else
        {
            // Nếu không có collider, sử dụng diện tích mặc định
            planeArea = 10f * 10f; // Ví dụ: 10x10 đơn vị
        }

        // Tính toán số lượng điểm dựa trên diện tích và khoảng cách giữa các điểm
        numberOfSpheres = Mathf.FloorToInt(planeArea / (distance * distance));
    }

    void SpawnSpiral()
    {
        int totalPoints = 0; // Đếm tổng số điểm đã tạo
        int x = 0, z = 0; // Bắt đầu từ trung tâm
        int dx = 0, dz = -1; // Hướng di chuyển ban đầu (lên trên)

        int maxSteps = Mathf.CeilToInt(Mathf.Sqrt(numberOfSpheres)); // Số bước tối đa trong một vòng xoắn

        for (int i = 0; i < maxSteps * maxSteps; i++)
        {
            // Kiểm tra nếu đã đạt đủ số lượng sphere
            if (totalPoints >= numberOfSpheres)
                return;

            // Tính toán vị trí của sphere
            float posX = planeCenter.x + x * distance;
            float posZ = planeCenter.z + z * distance;
            float posY = planeCenter.y; // Giữ nguyên tọa độ Y (trên mặt phẳng)

            Vector3 spherePosition = new Vector3(posX, posY, posZ);

            // Tạo sphere tại vị trí đã tính toán
            GameObject sphere = Instantiate(spherePrefab, spherePosition, Quaternion.identity);

            // Thêm sphere vào danh sách
            pointObjects.Add(sphere);

            // Tăng tổng số điểm đã tạo
            totalPoints++;

            // Di chuyển đến vị trí tiếp theo trong xoắn ốc
            if (x == z || (x < 0 && x == -z) || (x > 0 && x == 1 - z))
            {
                // Đổi hướng di chuyển
                int temp = dx;
                dx = -dz;
                dz = temp;
            }

            x += dx;
            z += dz;
        }
    }
}