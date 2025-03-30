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

    void Update()
    {
        HandleToolSwitching();
    }

    void HandleToolSwitching()
    {
        // Hỗ trợ chuyển đổi nhiều công cụ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchTool("FireAxe");
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
            SwitchTool("FireHose");
        }
    }

    void SwitchTool(string toolName)
    {
        playerScript.ResetToolStates(); // Đặt lại trạng thái công cụ trước khi chuyển đổi

        // Kích hoạt công cụ tương ứng
        switch (toolName)
        {
            case "FireAxe":
                playerScript.isUsingFireAxe = true;
                UpdateActiveTool();
                break;
            case "Ladder":
                playerScript.isCarryingLadder = true;
                UpdateActiveTool();
                break;
            case "Bucket":
                playerScript.isCarryingBucket = true;
                UpdateActiveTool();
                break;
            case "FireHose":
                playerScript.isHoldingFireHose = true;
                UpdateActiveTool();
                break;
        }

        // Cập nhật trạng thái người chơi
        playerScript.isPlayerHoldingTool = playerScript.isUsingFireAxe || playerScript.isCarryingLadder || playerScript.isCarryingBucket || playerScript.isHoldingFireHose;
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
            if (playerScript.isUsingFireAxe == true && tool.GetComponent<Tool>().toolName == "FireAxe" ||
                playerScript.isCarryingLadder == true && tool.GetComponent<Tool>().toolName == "Ladder" ||
                playerScript.isCarryingBucket == true && tool.GetComponent<Tool>().toolName == "Bucket" ||
                playerScript.isHoldingFireHose == true && tool.GetComponent<Tool>().toolName == "FireHose")
            {
                tool.gameObject.SetActive(true);
                tool.SetParent(activeEquipment.transform);
            }
        }
    }
}