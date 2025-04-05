using UnityEngine;

public class ToolWheelSelector : MonoBehaviour
{
    public Vector2 mousePosition;
    public float currentAngle;
    public int selection;
    private int previousSelection;
    public GameObject[] menuItems;
    private MenuItemS menuItemScripts;
    private MenuItemS previousmenuItemScripts;
    private GameObject player;
    [SerializeField] private InventorySystem inventorySystem;
    private int previousSelectedItemIndex;

    private void Awake()
    {
        player = GameObject.Find("Player");
        inventorySystem = player.GetComponentInChildren<InventorySystem>();
    }
    void Update()
    {
        // Tính toán góc và xác định mục được chọn
        mousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        currentAngle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        currentAngle = (currentAngle + 360) % 360;
        selection = (int)currentAngle / 45;

        if (selection != previousSelection)
        {
            // Bỏ chọn mục trước đó
            previousmenuItemScripts = menuItems[previousSelection].GetComponent<MenuItemS>();
            previousmenuItemScripts.Deselect();

            // Cập nhật mục được chọn
            previousSelection = selection;
            menuItemScripts = menuItems[selection].GetComponent<MenuItemS>();
            menuItemScripts.Select();
        }

        // Kiểm tra nếu nhấn chuột trái
        if (Input.GetMouseButtonDown(0))
        {
            if (previousSelectedItemIndex == selection) return;
            switch (selection)
            {
                case 0:
                    inventorySystem.SwitchTool(Tool.ToolType.FireAxe);
                    previousSelectedItemIndex = 0;
                    break;
                case 1:
                    inventorySystem.SwitchTool(Tool.ToolType.Ladder);
                    previousSelectedItemIndex = 1;
                    break;
                case 2:
                    inventorySystem.SwitchTool(Tool.ToolType.Bucket);
                    previousSelectedItemIndex = 2;
                    break;
                case 3:
                    inventorySystem.SwitchTool(Tool.ToolType.FireHose);
                    previousSelectedItemIndex = 3;
                    break;
                case 4:
                    inventorySystem.SwitchTool(Tool.ToolType.FireExtinguisher);
                    previousSelectedItemIndex = 4;
                    break;
                case 5:

                    previousSelectedItemIndex = 5;
                    break;
                case 6:
                    previousSelectedItemIndex = 6;
                    break;
                case 7:
                    previousSelectedItemIndex = 7;
                    break;
            }
        }
    }
}
