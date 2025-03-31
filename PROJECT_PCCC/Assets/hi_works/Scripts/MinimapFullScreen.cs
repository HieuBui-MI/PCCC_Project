using UnityEngine;

public class MinimapFullScreen : MonoBehaviour
{
    [SerializeField]
    public GameObject minimapFullScreenPanel; // Panel chứa minimap full-screen
    public Camera minimapCameraFull;         // Camera minimap full-screen
    public GameObject mainCamera;
    public GameObject cameraFollow;

    [Header("Zoom Settings")]
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 50f;

    [Header("Drag Settings")]
    public float dragSpeed = 0.5f;
    private Vector3 dragOrigin;
    private bool isMinimapActive = false;

    void Update()
    {
        // Nhấn M để bật/tắt minimap full-screen
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMinimapActive = !isMinimapActive;
            minimapFullScreenPanel.SetActive(isMinimapActive);
            minimapCameraFull.gameObject.SetActive(isMinimapActive);

            if (isMinimapActive)
            {
                minimapCameraFull.gameObject.SetActive(true);
                cameraFollow.SetActive(false);
            }
            else
            {
                minimapCameraFull.gameObject.SetActive(false);
                cameraFollow.SetActive(true);
            }


            cameraFollow.SetActive(!isMinimapActive); // Tắt camera follow khi mở minimap full-screen
        }

        if (isMinimapActive)
        {
            HandleZoom();
            HandleDrag();
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            minimapCameraFull.orthographicSize -= scroll * zoomSpeed;
            minimapCameraFull.orthographicSize = Mathf.Clamp(minimapCameraFull.orthographicSize, minZoom, maxZoom);
        }
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0)) // Khi nhấn chuột trái
        {
            dragOrigin = minimapCameraFull.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0)) // Khi giữ chuột trái để kéo
        {
            Vector3 difference = dragOrigin - minimapCameraFull.ScreenToWorldPoint(Input.mousePosition);
            minimapCameraFull.transform.position += new Vector3(difference.x, 0, difference.z);
        }
    }
}
