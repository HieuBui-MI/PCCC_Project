using UnityEngine;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour, IDragHandler, IScrollHandler
{
    [SerializeField] private RectTransform mapRect;     // Ảnh bản đồ
    [SerializeField] private RectTransform maskRect;    // Khung hiển thị bản đồ
    [SerializeField] private float dragSpeed = 1f;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 3f;

    private Vector2 mapSize;
    private Vector2 maskSize;

    private void Start()
    {
        if (mapRect == null || maskRect == null)
        {
            Debug.LogError("MapRect or MaskRect is not assigned!");
            enabled = false;
            return;
        }

        UpdateMapAndMaskSizes();
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragMap(eventData.delta);
    }

    public void OnScroll(PointerEventData eventData)
    {
        ZoomMap(eventData.scrollDelta.y);
    }

    private void DragMap(Vector2 delta)
    {
        mapRect.anchoredPosition += delta * dragSpeed;
        ClampMapPosition();
    }

    private void ZoomMap(float scrollDelta)
    {
        float newScale = Mathf.Clamp(mapRect.localScale.x + scrollDelta * zoomSpeed, minZoom, maxZoom);
        mapRect.localScale = Vector3.one * newScale;
        UpdateMapAndMaskSizes();
        ClampMapPosition();
    }

private void ClampMapPosition()
{
    Vector2 pos = mapRect.anchoredPosition;

    // Giới hạn X
    if (mapSize.x > maskSize.x)
    {
        float minX = (maskSize.x - mapSize.x) * 0.5f;
        float maxX = -minX;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
    }
    else
    {
        pos.x = 0f; // Canh giữa nếu nhỏ hơn
    }

    // Giới hạn Y
    if (mapSize.y > maskSize.y)
    {
        float minY = (maskSize.y - mapSize.y) * 0.5f;
        float maxY = -minY;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
    }
    else
    {
        pos.y = 0f; // Canh giữa nếu nhỏ hơn
    }

    mapRect.anchoredPosition = pos;
}


    private void UpdateMapAndMaskSizes()
    {
        mapSize = mapRect.rect.size * mapRect.localScale;
        maskSize = maskRect.rect.size;
    }
}
