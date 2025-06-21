using StarterAssets;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public Transform playerCameraRoot;
    public float reachDistance;
    public Vector3 hitPosition;
    [SerializeField] private Vector3 prevCarriedObjectPosition;
    [SerializeField] private Vector3 prevCarriedObjectRotation;
    public Vector3 cloneObjectPosition;
    public LayerMask raycastLayerMask;
    [SerializeField] private GameObject cloneObject;
    private bool previousIsInPlaceingMode = false;
    private PlayerState playerScript;
    [SerializeField] private GameObject prevCarriedObjectParrent;
    private DetectorSystem detectorSystem;

    ////////////////////////////////////////
    public Material red; // Màu đỏ
    public Material green; // Màu xanh lá cây

    private void Awake()
    {
        playerCameraRoot = transform.parent.Find("PlayerCameraRoot");
        playerScript = GetComponent<PlayerState>();
        detectorSystem = GetComponent<DetectorSystem>();
    }

    private void Update()
    {
        PickupObject();
        PickupVictim();
        ObjectPlaceAction();
        HandlePutVictim();
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
        bool currentIsInPlaceingMode = playerScript.isInCarryState;

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

    private void PickupObject()
    {
        if (Input.GetKeyDown(KeyCode.C) && !playerScript.isInCarryState)
        {
            if (playerScript == null || playerScript.carriedObject != null || playerScript.carriedVictim != null) return;
            if (!detectorSystem.TargetObject.GetComponent<PlacableObj>()) return;
            if (detectorSystem.TargetObject.GetComponent<PlacableObj>().carriableType != PlacableObj.CarriableType.Object) return;

            prevCarriedObjectPosition = detectorSystem.TargetObject.transform.position;
            prevCarriedObjectRotation = detectorSystem.TargetObject.transform.eulerAngles;

            playerScript.carriedObject = detectorSystem.TargetObject;

            Rigidbody rb = playerScript.carriedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            MeshCollider[] meshColliders = playerScript.carriedObject.GetComponentsInChildren<MeshCollider>();
            foreach (MeshCollider meshCollider in meshColliders)
            {
                meshCollider.isTrigger = true;
            }

            prevCarriedObjectParrent = detectorSystem.TargetObject.transform.parent.gameObject;
            playerScript.carriedObject.transform.SetParent(GetComponent<InventorySystem>().carrySlot.transform);
            playerScript.carriedObject.transform.localPosition = new Vector3(1.152f, -0.067f, -0.295f);
            playerScript.carriedObject.transform.localRotation = new Quaternion(0.372680098f, -0.640431643f, -0.557825327f, -0.373882234f);
        }
    }

    public void ObjectPlaceAction()
    {
        if (Input.GetKeyDown(KeyCode.C) && playerScript.isInCarryState)
        {
            if (playerScript.carriedObject == null) return;
            if (!cloneObject.GetComponent<CloneObject>() || !cloneObject.GetComponent<CloneObject>().validPlaceState) return;

            PlaceObjectAtPosition(playerScript.carriedObject, hitPosition, cloneObject.transform.rotation);

            BackToPrevParrent();
            playerScript.carriedObject = null;
            playerScript.isPlayerCarryingObject = false;

            DestroyCloneObject();
        }
    }

    private void HandleCancelPlacement()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerScript.carriedObject != null)
        {
            PlaceObjectAtPosition(playerScript.carriedObject, prevCarriedObjectPosition, Quaternion.Euler(prevCarriedObjectRotation));

            BackToPrevParrent();
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

    private void PickupVictim()
    {
        if (Input.GetKeyDown(KeyCode.C) && !playerScript.isInCarryState)
        {
            if (playerScript == null || (playerScript.carriedVictim != null) || playerScript.carriedObject != null) return;
            if (detectorSystem.TargetObject.GetComponent<PlacableObj>().carriableType != PlacableObj.CarriableType.Victim) return;
            
            playerScript.isPlayerCarryingAVictim = true;
            playerScript.carriedVictim = detectorSystem.TargetObject;
            playerScript.carriedVictim.transform.SetParent(GetComponent<InventorySystem>().carrySlot.transform);
            playerScript.carriedVictim.transform.localPosition = Vector3.zero;
            playerScript.carriedVictim.transform.localRotation = Quaternion.identity;
            playerScript.carriedVictim.SetActive(false);
        }
    }

    private void HandlePutVictim()
    {
        if (Input.GetKeyDown(KeyCode.C) && playerScript.isInCarryState)
        {
            if (playerScript == null || playerScript.carriedVictim == null) return;

            Stretcher stretcher = detectorSystem.TargetObject.GetComponent<Stretcher>();
            if (stretcher != null && stretcher.isOcupied == false)
            {
                playerScript.carriedVictim.SetActive(true);
                stretcher.PutVictimInStretcher(playerScript.carriedVictim);
                playerScript.carriedVictim = null;
                playerScript.isPlayerCarryingAVictim = false;
            }
            else
            {
                Debug.Log("Stretcher is occupied or not found.");
            }
        }

    }

    public void BackToPrevParrent()
    {
        if (prevCarriedObjectParrent != null)
        {
            playerScript.carriedObject.transform.SetParent(prevCarriedObjectParrent.transform);
            prevCarriedObjectParrent = null;
        }
    }
}