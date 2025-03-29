using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject Equipments = null;
    private PlayerScript playerScript;


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

        playerScript = GetComponent<PlayerScript>();
        if (playerScript == null)
        {
            Debug.LogError("PlayerScript component not found on this GameObject!");
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
        playerScript.isUsingAxe = false;
        playerScript.isCarryingLadder = false;
        playerScript.isCarryingBucket = false;
        playerScript.isCarryingRope = false;

        // Kích hoạt công cụ tương ứng
        switch (toolName)
        {
            case "Axe":
                playerScript.isUsingAxe = true;
                break;
            case "Ladder":
                playerScript.isCarryingLadder = true;
                break;
            case "Bucket":
                playerScript.isCarryingBucket = true;
                break;
            case "Rope":
                playerScript.isCarryingRope = true;
                break;
        }

        // Cập nhật trạng thái người chơi
        playerScript.isPlayerHoldingTool = playerScript.isUsingAxe || playerScript.isCarryingLadder || playerScript.isCarryingBucket || playerScript.isCarryingRope;
    }

    void UpdateActiveTool()
    {
        if (Equipments == null) return;

        foreach (Transform child in Equipments.transform)
        {
            // Kích hoạt công cụ dựa trên trạng thái
            child.gameObject.SetActive(child.GetComponent<Tool>().toolName == "FireAxe" && playerScript.isUsingAxe ||
                                       child.GetComponent<Tool>().toolName == "Ladder" && playerScript.isCarryingLadder ||
                                       child.GetComponent<Tool>().toolName == "Bucket" && playerScript.isCarryingBucket ||
                                       child.GetComponent<Tool>().toolName == "Rope" && playerScript.isCarryingRope);
        }
    }
}