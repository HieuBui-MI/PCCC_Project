using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject inActiveEquipments = null;
    public GameObject activeEquipment = null;

    private PlayerScript playerScript;


    private void Awake()
    {
        if (inActiveEquipments == null)
        {
            inActiveEquipments = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "InActiveEquipments").gameObject;
        }

        if (activeEquipment == null)
        {
            activeEquipment = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "ActiveEquipment").gameObject;
        }

        if (playerScript == null)
        {
            playerScript = GetComponent<PlayerScript>();
        }
    }

    public void SwitchTool(Tool.ToolType toolType)
    {
        playerScript.ResetToolStates(); // Đặt lại trạng thái công cụ trước khi chuyển đổi

        // Kích hoạt công cụ tương ứng
        switch (toolType)
        {
            case Tool.ToolType.FireAxe:
                playerScript.isUsingFireAxe = true;
                UpdateActiveTool();
                break;
            case Tool.ToolType.Ladder:
                playerScript.isCarryingLadder = true;
                UpdateActiveTool();
                break;
            case Tool.ToolType.Bucket:
                playerScript.isCarryingBucket = true;
                UpdateActiveTool();
                break;
            case Tool.ToolType.FireHose:
                playerScript.isHoldingFireHose = true;
                UpdateActiveTool();
                break;
            case Tool.ToolType.FireExtinguisher:
                playerScript.isHoldingFireExtinguisher = true;
                UpdateActiveTool();
                break;
        }

        // Cập nhật trạng thái người chơi
        playerScript.isPlayerHoldingTool = playerScript.isUsingFireAxe 
        || playerScript.isCarryingLadder || playerScript.isCarryingBucket 
        || playerScript.isHoldingFireHose || playerScript.isHoldingFireExtinguisher;
    }

    void UpdateActiveTool()
    {
        // Đặt tất cả các công cụ trong activeEquipment về inActiveEquipments
        foreach (Transform tool in activeEquipment.transform)
        {
            tool.SetParent(inActiveEquipments.transform);
            tool.gameObject.SetActive(false);
        }

        // Đặt công cụ đang sử dụng về activeEquipment
        foreach (Transform tool in inActiveEquipments.transform)
        {
            if (playerScript.isUsingFireAxe == true && tool.GetComponent<Tool>().toolType == Tool.ToolType.FireAxe ||
                playerScript.isCarryingLadder == true && tool.GetComponent<Tool>().toolType == Tool.ToolType.Ladder ||
                playerScript.isCarryingBucket == true && tool.GetComponent<Tool>().toolType == Tool.ToolType.Bucket ||
                playerScript.isHoldingFireHose == true && tool.GetComponent<Tool>().toolType == Tool.ToolType.FireHose ||
                playerScript.isHoldingFireExtinguisher == true && tool.GetComponent<Tool>().toolType == Tool.ToolType.FireExtinguisher)
            {
                tool.gameObject.SetActive(true);
                tool.SetParent(activeEquipment.transform);
            }
        }
    }
}