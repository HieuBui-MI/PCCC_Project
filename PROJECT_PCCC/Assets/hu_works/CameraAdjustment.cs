using UnityEngine;
using Unity.Cinemachine;

public class CameraAdjustment : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera; // Camera 1
    [SerializeField] private GameObject playerFollowCamera; // Camera 2
    private CinemachineCameraOffset cameraOffset; // Tham chiếu tới Cinemachine Camera Offset
    private Vector3 previousOffset; // Biến lưu trữ offset

    void Start()
    {
        // Lấy component CinemachineCameraOffset từ mainCamera
        cameraOffset = playerFollowCamera.GetComponent<CinemachineCameraOffset>();
        if (cameraOffset == null)
        {
            Debug.LogError("CinemachineCameraOffset component not found on mainCamera!");
        }
        previousOffset = cameraOffset.Offset; // Lưu trữ offset ban đầu
        Debug.Log("Initial Offset: " + previousOffset); // In ra offset ban đầu
    }

    void Update()
    {
        AdjustCameraPosition();
        
    }

    private void AdjustCameraPosition()
{
    if (cameraOffset == null) return;

    // Lấy vị trí và hướng từ playerFollowCamera đến mainCamera
    Vector3 origin = playerFollowCamera.transform.position;
    Vector3 direction = mainCamera.transform.position - playerFollowCamera.transform.position;

    // Hiển thị tia raycast trong Scene View
    Debug.DrawRay(origin, direction, Color.red);

    // Loại trừ layer "Player"
    int playerLayer = LayerMask.NameToLayer("Player");
    int layerMask = ~(1 << playerLayer); // Loại trừ layer "Player"

    // Bắn tia raycast
    if (Physics.Raycast(origin, direction, out RaycastHit hit, 3f, layerMask))
    {
        // Tính toán vị trí gần hơn một chút so với hit.point
        float offsetDistance = 0.5f; // Khoảng cách gần hơn (điều chỉnh giá trị này theo ý muốn)
        Vector3 closerPoint = Vector3.Lerp(hit.point, playerFollowCamera.transform.position, offsetDistance);

        // Tính toán offset mới
        Vector3 hitOffset = closerPoint - playerFollowCamera.transform.position;
        cameraOffset.Offset = hitOffset; // Cập nhật offset
        previousOffset = hitOffset; // Lưu trữ offset hiện tại
    }
    else
    {
        // Nếu không có va chạm, giữ nguyên offset hiện tại
        cameraOffset.Offset = previousOffset; // Đặt offset theo hướng ban đầu
    }
}
}