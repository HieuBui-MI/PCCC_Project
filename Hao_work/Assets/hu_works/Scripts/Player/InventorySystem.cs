using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject Equipments = null;
    public bool isPlayerHoldingTool = false;
    public bool isUsingAxe = false;
    public bool isCarryingLadder = false;
    public bool isCarryingBucket = false;
    public bool isCarryingRope = false;

    private void Awake()
    {
        // Kiểm tra null để tránh lỗi
        var equipmentTransform = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Equipments");
        if (equipmentTransform != null)
        {
            Equipments = equipmentTransform.gameObject;
        }
        else
        {
            Debug.LogWarning("Equipments object not found!");
        }
    }

    void Update()
    {
        HandleToolSwitching();
        UpdateActiveTool();
    }

    void HandleToolSwitching()
    {
        // Hỗ trợ chuyển đổi nhiều công cụ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchTool("Axe");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchTool("Ladder");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchTool("Bucket");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchTool("Rope");
        }
    }

    void SwitchTool(string toolName)
    {
        // Reset trạng thái tất cả công cụ
        isUsingAxe = false;
        isCarryingLadder = false;
        isCarryingBucket = false;
        isCarryingRope = false;

        // Kích hoạt công cụ tương ứng
        switch (toolName)
        {
            case "Axe":
                isUsingAxe = true;
                break;
            case "Ladder":
                isCarryingLadder = true;
                break;
            case "Bucket":
                isCarryingBucket = true;
                break;
            case "Rope":
                isCarryingRope = true;
                break;
        }

        // Cập nhật trạng thái người chơi
        isPlayerHoldingTool = isUsingAxe || isCarryingLadder || isCarryingBucket || isCarryingRope;
    }

    void UpdateActiveTool()
    {
        if (Equipments == null) return;

        foreach (Transform child in Equipments.transform)
        {
            // Kích hoạt công cụ dựa trên trạng thái
            child.gameObject.SetActive(child.GetComponent<Tool>().toolName == "FireAxe" && isUsingAxe ||
                                       child.GetComponent<Tool>().toolName == "Ladder" && isCarryingLadder ||
                                       child.GetComponent<Tool>().toolName == "Bucket" && isCarryingBucket ||
                                       child.GetComponent<Tool>().toolName == "Rope" && isCarryingRope);
        }
    }
}