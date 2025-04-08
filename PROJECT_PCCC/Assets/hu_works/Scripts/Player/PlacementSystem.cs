using StarterAssets;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public Transform playerCameraRoot;
    public float reachDistance;
    public Vector3 hitPosition;
    public Vector3 prevCarriedObjectPosition;
    public Vector3 prevCarriedObjectRotation;
    public Vector3 cloneObjectPosition;
    public LayerMask raycastLayerMask;
    private GameObject cloneObject;
    private bool previousIsInPlaceingMode = false;
    private PlayerScript playerScript;
    ////////////////////////////////////////
    public Material red; // Màu đỏ
    public Material green; // Màu xanh lá cây

    private void Awake()
    {
        playerCameraRoot = transform.parent.Find("PlayerCameraRoot");
        playerScript = GetComponent<PlayerScript>();
    }

    private void Update()
    {
        SetTargetPosition();
        UpdateCloneObjectPosition();
        HandleCancelPlacement();
        HandleRotateCarriedObject();
        TrackPlacementModeChange();
    }

    private void SetTargetPosition()
    {
        Vector3 origin = playerCameraRoot.position;
        Vector3 direction = playerCameraRoot.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, reachDistance, raycastLayerMask))
        {
            hitPosition = hit.point;
        }
        else
        {
            hitPosition = origin + direction * reachDistance;

            if (Physics.Raycast(hitPosition, Vector3.down, out RaycastHit groundHit, Mathf.Infinity, raycastLayerMask))
            {
                hitPosition = groundHit.point;
            }
        }
    }

    private void TrackPlacementModeChange()
    {
        bool currentIsInPlaceingMode = playerScript.isInPlacingMode;

        if (!previousIsInPlaceingMode && currentIsInPlaceingMode)
        {
            if (playerScript.carriedObject != null)
            {
                GetComponent<Animator>().SetTrigger("triggerPlacingMode");
                CreateCloneObject(playerScript.carriedObject);
            }
        }
        previousIsInPlaceingMode = currentIsInPlaceingMode;
    }

    private void CreateCloneObject(GameObject carriedObject)
    {
        cloneObject = Instantiate(carriedObject);
        cloneObject.transform.rotation = Quaternion.identity;

        // Remove unnecessary components
        RemoveComponent<Interactable>(cloneObject);

        // Set all MeshColliders to trigger
        SetMeshCollidersToTrigger(cloneObject);

        // Configure Rigidbody
        ConfigureRigidbody(cloneObject);

        // Thêm script CloneObject và truyền materials
        CloneObject cloneObjectScript = cloneObject.AddComponent<CloneObject>();
        cloneObjectScript.red = red;
        cloneObjectScript.green = green;
    }

    private void UpdateCloneObjectPosition()
    {
        if (cloneObject == null) return;
        cloneObject.transform.position = hitPosition;
    }

    public void PlaceDownObj()
    {
        if (!playerScript.isInPlacingMode || playerScript.carriedObject == null) return;
        if (cloneObject.GetComponentInChildren<Renderer>().material == red) return;

        PlaceObjectAtPosition(playerScript.carriedObject, hitPosition, cloneObject.transform.rotation);

        playerScript.carriedObject.GetComponent<Interactable>().BackToPrevParrent();
        playerScript.carriedObject = null;
        playerScript.isPlayerCarryingObject = false;

        DestroyCloneObject();
    }

    private void HandleCancelPlacement()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerScript.carriedObject != null)
        {
            PlaceObjectAtPosition(playerScript.carriedObject, prevCarriedObjectPosition, Quaternion.Euler(prevCarriedObjectRotation));

            playerScript.carriedObject.GetComponent<Interactable>().BackToPrevParrent();
            playerScript.carriedObject = null;
            playerScript.isPlayerCarryingObject = false;

            DestroyCloneObject();
        }
    }

    private void HandleRotateCarriedObject()
    {
        if (cloneObject != null && Input.GetMouseButton(1))
        {
            cloneObject.transform.Rotate(Vector3.up, 90f * Time.deltaTime);
        }
    }

    private void DestroyCloneObject()
    {
        if (cloneObject != null)
        {
            Destroy(cloneObject);
            cloneObject = null;
        }
    }

    private void PlaceObjectAtPosition(GameObject obj, Vector3 position, Quaternion rotation)
    {
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        PlacedObjectState(obj);
    }

    private void PlacedObjectState(GameObject obj)
    {
        MeshCollider[] meshColliders = obj.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider meshCollider in meshColliders)
        {
            meshCollider.isTrigger = false;
        }
    }

    private void RemoveComponent<T>(GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component != null)
        {
            Destroy(component);
        }
    }

    private void SetMeshCollidersToTrigger(GameObject obj)
    {
        MeshCollider[] meshColliders = obj.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider meshCollider in meshColliders)
        {
            meshCollider.isTrigger = true;
        }
    }

    private void ConfigureRigidbody(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}