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
            Debug.Log($"Selected item: {selection}");
            // Tắt canvas hiện tại
            gameObject.SetActive(false);
        }
    }
}
