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
        bool currentIsInPlaceingMode = playerScript.isInPlaceingMode;

        if (!previousIsInPlaceingMode && currentIsInPlaceingMode)
        {
            if (playerScript.carriedObject != null)
            {
                CreateCloneObject(playerScript.carriedObject);
            }
        }

        previousIsInPlaceingMode = currentIsInPlaceingMode;
    }

    private void CreateCloneObject(GameObject carriedObject)
    {
        cloneObject = Instantiate(carriedObject);
        cloneObject.transform.rotation = Quaternion.identity;

        // Remove Interactable component if it exists
        Interactable interactableComponent = cloneObject.GetComponent<Interactable>();
        if (interactableComponent != null)
        {
            Destroy(interactableComponent);
        }

        // Xử lý tất cả các MeshCollider trong cloneObject và các child của nó
        MeshCollider[] meshColliders = cloneObject.GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider meshCollider in meshColliders)
        {
            meshCollider.isTrigger = true;
        }

        // Xử lý Rigidbody
        Rigidbody rb = cloneObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private void UpdateCloneObjectPosition()
    {
        if (cloneObject == null) return;
        cloneObject.transform.position = hitPosition;
    }

    public void PlaceDownObj()
    {
        if (!playerScript.isInPlaceingMode || playerScript.carriedObject == null) return;

        PlaceObjectAtPosition(playerScript.carriedObject, hitPosition, cloneObject.transform.rotation);

        playerScript.carriedObject = null;
        playerScript.isPlayerCarryingObject = false;

        DestroyCloneObject();
    }

    private void HandleCancelPlacement()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerScript.carriedObject != null)
        {
            PlaceObjectAtPosition(playerScript.carriedObject, prevCarriedObjectPosition, Quaternion.Euler(prevCarriedObjectRotation));

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
    }
}