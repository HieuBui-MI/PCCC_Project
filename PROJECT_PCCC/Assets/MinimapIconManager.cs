using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinimapIconManager : MonoBehaviour
{
    public GameObject iconPrefab; // UI Image prefab
    public Transform canvasMarker; // Canvas chứa icon
    public string targetLayer = "Vehicle"; // Layer của các GameObject cần tạo icon
    public GameObject Ground;

    private Dictionary<Transform, GameObject> iconDict = new();

    void Start()
    {
        if (Ground == null)
        {
            Debug.LogError("Ground is not assigned!");
            return;
        }

        // Lấy tất cả GameObject con của Ground có layer "Vehicle"
        GameObject[] vehicles = Ground.GetComponentsInChildren<Transform>(true) // Lấy tất cả Transform con
            .Where(t => t.gameObject.layer == LayerMask.NameToLayer(targetLayer) && t.parent == Ground.transform) // Chỉ lấy GameObject con trực tiếp của Ground
            .Select(t => t.gameObject) // Chuyển từ Transform sang GameObject
            .ToArray();

        foreach (GameObject vehicle in vehicles)
        {
            // Instantiate icon và gán nó làm con của canvasMarker
            GameObject icon = Instantiate(iconPrefab, canvasMarker);

            // Đặt vị trí icon tại vị trí của vehicle, nhưng trục y +20
            Vector3 iconPosition = vehicle.transform.position + new Vector3(0, 20, 0);
            icon.transform.position = iconPosition;

            // Lưu vào dictionary
            iconDict[vehicle.transform] = icon;
        }
    }

    void Update()
    {
        foreach (var pair in iconDict)
        {
            Transform target = pair.Key;
            GameObject iconObj = pair.Value;

            if (target == null || iconObj == null)
                continue;

            // Cập nhật vị trí icon theo vị trí của target, với trục y +20
            Vector3 iconPosition = target.position + new Vector3(0, 20, 0);
            iconObj.transform.position = iconPosition;
        }
    }
}
