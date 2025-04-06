using StarterAssets;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public Transform playerCameraRoot;
    public float reachDistance;
    public Vector3 hitPosition;
    //////////////////////////////////////////////////////////
    public Vector3 prevCarriedObjectPosition;
    public Vector3 prevCarriedObjectRotation;
    public Vector3 cloneObjectPosition;
    public LayerMask raycastLayerMask;
    private StarterAssetsInputs inputs;
    private void Awake()
    {
        playerCameraRoot = transform.parent.Find("PlayerCameraRoot");
    }
    void Start()
    {

    }

    void Update()
    {
        SetTargetPosition();
        HollowPosition();
        CancelPlacement();
        RotateCarriedObject();
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

    private void RotateCarriedObject()
    {
        // Kiểm tra nếu người chơi đang mang một đối tượng
        if (GetComponent<PlayerScript>().carriedObject != null)
        {
            // Kiểm tra nếu chuột phải được nhấn
            if (Input.GetMouseButton(1)) // Chuột phải
            {
                // Xoay đối tượng quanh trục Y
                GetComponent<PlayerScript>().carriedObject.transform.Rotate(Vector3.up, 90f * Time.deltaTime);
            }
        }
    }

    public void HollowPosition()
    {
        if (GetComponent<PlayerScript>().carriedObject == null) return;

        GameObject clonedObject = Instantiate(GetComponent<PlayerScript>().carriedObject);
        clonedObject.transform.position = GetComponent<PlacementSystem>().hitPosition;
        Destroy(clonedObject, 0.1f);
    }

    public void PlaceDownObj()
    {
        if (GetComponent<PlayerScript>().isInPlaceingMode)
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GetComponent<PlayerScript>().carriedObject != null)
            {
                GetComponent<PlayerScript>().carriedObject.transform.position = prevCarriedObjectPosition;
                GetComponent<PlayerScript>().carriedObject.transform.rotation = Quaternion.Euler(prevCarriedObjectRotation);
                GetComponent<PlayerScript>().carriedObject = null;
                GetComponent<PlayerScript>().isPlayerCarryingObject = false;
            }
        }
    }
}
