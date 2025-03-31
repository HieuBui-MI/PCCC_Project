using Unity.VisualScripting;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private GameObject playerCameraRoot;
    [SerializeField] private GameObject aimingPoint;
    [SerializeField] private LayerMask hittableLayers; // LayerMask để chỉ định các layer có thể bị hit

    private void Awake()
    {
        playerCameraRoot = transform.parent.Find("PlayerCameraRoot").gameObject;
    }

    void Update()
    {
        SetAimingTargetPostion();
    }

    void SetAimingTargetPostion()
    {
        Vector3 origin = playerCameraRoot.transform.position;
        Vector3 direction = playerCameraRoot.transform.forward;

        // Sử dụng layerMask để bỏ qua các layer không cần thiết
        if (Physics.Raycast(origin, direction, out RaycastHit hit, GetComponent<InteractionSystem>().reachDistance, hittableLayers))
        {
            // Nếu có hit, đặt aimingPoint tại vị trí hit
            aimingPoint.transform.position = hit.point;
        }
        else
        {
            // Nếu không có hit, đặt aimingPoint ở vị trí mặc định
            aimingPoint.transform.position = origin + direction * GetComponent<InteractionSystem>().reachDistance;
        }
    }
}