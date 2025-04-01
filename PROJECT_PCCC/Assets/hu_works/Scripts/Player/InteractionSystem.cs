using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    public GameObject playerCameraRoot;
    public float reachDistance = 5f;
    private GameObject marker;
    [SerializeField] private GameObject markerPrefab;
    private GameObject previousTargetObject;

    private void Awake()
    {
        if (playerCameraRoot == null)
        {
            playerCameraRoot = transform.parent.Find("PlayerCameraRoot").gameObject;
        }
    }

    void Update()
    {
        SetTargetObject();
    }

    void SetTargetObject()
    {
        Vector3 origin = playerCameraRoot.transform.position;
        Vector3 direction = playerCameraRoot.transform.forward;
        // Vẽ ray trong Scene View để debug
        Debug.DrawRay(origin, direction * reachDistance, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, reachDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.GetComponent<Interactable>() == null)
            {
                return;
            }

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

    void ApplyMarker(GameObject obj)
    {
        if (markerPrefab == null)
        {
            Debug.LogWarning("Marker prefab is not assigned!");
            return;
        }

        if (marker == null)
        {
            marker = Instantiate(markerPrefab);
        }

        Bounds bounds = obj.GetComponent<Collider>().bounds;
        Vector3 markerPosition = bounds.center + Vector3.up * bounds.extents.y + Vector3.up * 0.3f; // Chính giữa phía trên
        marker.transform.position = markerPosition;
    }

    void RemoveMarker()
    {
        if (marker != null)
        {
            Destroy(marker);
            marker = null;
        }
    }

    public void Interact()
    {
        if (targetObject != null)
        {
            Interactable interactable = targetObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.InteractCase(transform.parent.gameObject);
            }
        }
    }
}