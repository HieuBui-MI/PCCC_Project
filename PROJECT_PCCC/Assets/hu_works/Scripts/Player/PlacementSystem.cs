using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public GameObject playerCameraRoot;
    public float reachDistance;
    public Vector3 hitPosition;
    public Vector3 prevCarriedObjectPosition;
    public LayerMask raycastLayerMask;
    private void Awake()
    {
        if (playerCameraRoot == null)
        {
            playerCameraRoot = transform.parent.Find("PlayerCameraRoot")?.gameObject;
        }
    }
    void Start()
    {

    }

    void Update()
    {
        SetTargetPosition();
        HollowPosition();
        PlaceDownObj();
        CancelPlacement();
    }

    private void SetTargetPosition()
    {
        Vector3 origin = playerCameraRoot.transform.position;
        Vector3 direction = playerCameraRoot.transform.forward;

        // Debug.DrawRay(origin, direction * reachDistance, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, reachDistance, raycastLayerMask))
        {
            hitPosition = hit.point;
        }
    }

    public void HollowPosition()
    {
        if (GetComponent<PlayerScript>().carriedObject == null) return;
        GetComponent<PlayerScript>().carriedObject.transform.position = GetComponent<PlacementSystem>().hitPosition;
    }

    public void PlaceDownObj()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GetComponent<PlayerScript>().carriedObject != null)
            {
                GetComponent<PlayerScript>().carriedObject.transform.position = GetComponent<PlacementSystem>().hitPosition;
                GetComponent<PlayerScript>().carriedObject = null;
                GetComponent<PlayerScript>().isPlayerCarryingObject = false;
            }
        }
    }

    public void CancelPlacement()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (GetComponent<PlayerScript>().carriedObject != null)
            {
                GetComponent<PlayerScript>().carriedObject.transform.position = prevCarriedObjectPosition;
                GetComponent<PlayerScript>().carriedObject = null;
                GetComponent<PlayerScript>().isPlayerCarryingObject = false;
            }
        }
    }
}
