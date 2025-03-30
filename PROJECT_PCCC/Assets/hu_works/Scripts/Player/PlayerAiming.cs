using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private GameObject playerCameraRoot;
    [SerializeField] private bool isAiming;
    [SerializeField] private GameObject aimingPoint;
    [SerializeField] private LayerMask hittableLayers; // LayerMask để chỉ định các layer có thể bị hit

    private void Awake()
    {
        playerCameraRoot = GameObject.Find("PlayerCameraRoot");
    }

    void Update()
    {
        if (isAiming)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            Vector3 targetRotation = playerCameraRoot.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotation.x, targetRotation.y, currentRotation.z);
        }
        SetAimingTargetPostion();
    }

    void SetAimingTargetPostion()
    {
        Vector3 origin = playerCameraRoot.transform.position;
        Vector3 direction = playerCameraRoot.transform.forward;

        // Sử dụng layerMask để bỏ qua các layer không cần thiết
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 1000f, hittableLayers))
        {
            aimingPoint.transform.position = hit.point;
        }
    }
}