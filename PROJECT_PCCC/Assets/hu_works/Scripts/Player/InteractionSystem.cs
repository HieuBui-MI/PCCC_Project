using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    public GameObject playerCameraRoot;
    public float reachDistance;
    private GameObject marker;
    [SerializeField] private GameObject markerPrefab;

    private void Awake()
    {
        if (playerCameraRoot == null)
        {
            playerCameraRoot = transform.parent.Find("PlayerCameraRoot")?.gameObject;
        }
    }

    private void Update()
    {
        SetTargetObject();
    }

    private void SetTargetObject()
    {
        Vector3 origin = playerCameraRoot.transform.position;
        Vector3 direction = playerCameraRoot.transform.forward;

        // Vẽ ray trong Scene View để debug
        Debug.DrawRay(origin, direction * reachDistance, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, reachDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Kiểm tra xem đối tượng có phải là Interactable không
            Interactable interactable = hitObject.GetComponent<Interactable>();
            if (interactable == null)
            {
                RemoveMarker();
                targetObject = null;
                return;
            }

            // Nếu đối tượng mới khác với targetObject hiện tại, cập nhật targetObject
            if (hitObject != targetObject)
            {
                RemoveMarker();
                targetObject = hitObject;
                ApplyMarker(targetObject);
            }
        }
        else
        {
            RemoveMarker();
            targetObject = null;
        }
    }

    private void ApplyMarker(GameObject obj)
    {
        if (markerPrefab == null)
        {
            Debug.LogWarning("Marker prefab is not assigned!");
            return;
        }

        // Instantiate marker nếu chưa tồn tại
        if (marker == null)
        {
            marker = Instantiate(markerPrefab);
        }

        // Đặt vị trí marker dựa trên bounds của đối tượng
        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null)
        {
            Bounds bounds = objCollider.bounds;
            Vector3 markerPosition = bounds.center + Vector3.up * bounds.extents.y + Vector3.up * 0.3f; // Chính giữa phía trên
            marker.transform.position = markerPosition;
            marker.transform.SetParent(obj.transform); 
        }
    }

    private void RemoveMarker()
    {
        if (marker != null)
        {
            Destroy(marker);
            marker = null;
        }
    }

    public void Interact()
    {
        if (targetObject == null) return;

        Interactable interactable = targetObject.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.InteractCase(transform.parent.gameObject);
        }
    }
}