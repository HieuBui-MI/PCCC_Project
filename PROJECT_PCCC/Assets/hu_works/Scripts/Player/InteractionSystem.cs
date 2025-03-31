using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    public GameObject playerCameraRoot;
    [SerializeField] private float reachDistance = 5f;
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
        OutlineObject();
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
            GameObject hitObjectRoot = hitObject.transform.root.gameObject;

            if (hitObjectRoot.GetComponent<Interactable>() == null)
            {
                return;
            }

            if (hitObjectRoot != targetObject)
            {
                RemoveMarker();
                targetObject = hitObjectRoot;
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
        Vector3 markerPosition = bounds.center + Vector3.up * bounds.extents.y + Vector3.up * 0.2f; // Chính giữa phía trên
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

    void OutlineObject()
    {
        if (previousTargetObject != targetObject)
        {
            if (previousTargetObject != null)
            {
                Outline previousOutline = previousTargetObject.transform.Find("Body/CarFrame").GetComponent<Outline>();
                if (previousOutline != null)
                {
                    previousOutline.enabled = false;
                }
            }

            if (targetObject != null)
            {
                Outline currentOutline = targetObject.transform.Find("Body/CarFrame").GetComponent<Outline>();
                if (currentOutline != null)
                {
                    currentOutline.enabled = true;
                }
            }

            previousTargetObject = targetObject;
        }
    }
}