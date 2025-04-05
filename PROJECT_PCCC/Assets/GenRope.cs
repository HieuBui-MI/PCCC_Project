using UnityEngine;
using System.Collections;

public class GenRope : MonoBehaviour
{
    private FireHydrant fireHydrant;
    private bool hasLoggedPositions = false; // Biến cờ để kiểm tra trạng thái
    public GameObject parentRope;
    public GameObject ropePrefab;

    void Update()
    {
        if (fireHydrant.isConnectedToFireTruck && !hasLoggedPositions)
        {
            // Lấy vị trí của GameObject gắn FireHydrant
            Vector3 fireHydrantPosition = fireHydrant.gameObject.transform.position;
            Debug.Log("FireHydrant Position: " + fireHydrantPosition);

            // Lấy vị trí của GameObject được kết nối
            Vector3 Position2 = fireHydrant.objConnectedTo.transform.position;
            Debug.Log("FireTruck Position: " + Position2);

            CreateRope();

            // Đánh dấu đã thực thi
            hasLoggedPositions = true;
        }
        else if (!fireHydrant.isConnectedToFireTruck)
        {
            Debug.Log("FireHydrant is not connected to the fire truck.");
        }
    }

    void Start()
    {
        // Lấy Component FireHydrant từ GameObject hiện tại
        fireHydrant = GetComponent<FireHydrant>();
    }

    public void CreateRope()
    {
        // Lấy vị trí của FireHydrant và đối tượng được kết nối
        Vector3 fireHydrantPosition = fireHydrant.gameObject.transform.position;
        Vector3 connectedObjectPosition = fireHydrant.objConnectedTo.transform.position;

        // Tính toán vị trí trung điểm giữa hai tọa độ
        Vector3 middlePosition = (fireHydrantPosition + connectedObjectPosition) / 2;

        // Tạo một đối tượng Rope từ prefab tại vị trí trung điểm
        GameObject rope = Instantiate(ropePrefab, middlePosition, Quaternion.identity);

        // Đặt Rope làm con của parentRope
        if (parentRope != null)
        {
            rope.transform.SetParent(parentRope.transform);
        }

        // Bắt đầu Coroutine để delay 3 giây trước khi đặt vị trí cho S1 và S2
        StartCoroutine(SetRopePositionsWithDelay(rope, fireHydrantPosition, connectedObjectPosition));
    }

    private IEnumerator SetRopePositionsWithDelay(GameObject rope, Vector3 fireHydrantPosition, Vector3 connectedObjectPosition)
    {
        // Chờ 3 giây
        yield return new WaitForSeconds(3f);

        // Tìm các GameObject con S1 và S2 trong ropePrefab
        Transform s1 = rope.transform.Find("S1");
        Transform s2 = rope.transform.Find("S2");

        if (s1 != null)
        {
            s1.position = fireHydrantPosition; // Đặt vị trí của S1
        }
        else
        {
            Debug.LogWarning("S1 not found in ropePrefab.");
        }

        if (s2 != null)
        {
            s2.position = connectedObjectPosition; // Đặt vị trí của S2
        }
        else
        {
            Debug.LogWarning("S2 not found in ropePrefab.");
        }

        Debug.Log("Positions of S1 and S2 set after delay.");
    }
}
