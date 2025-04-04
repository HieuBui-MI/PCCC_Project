using UnityEngine;
using UnityEngine.UI;

public class MenuItemS : MonoBehaviour
{
    public Image itemImage; // Hình ảnh của item

    private void Awake()
    {
        // Tìm GameObject con tên "background" và lấy Image component từ nó
        Transform backgroundTransform = transform.Find("BgW");
        if (backgroundTransform != null)
        {
            itemImage = backgroundTransform.GetComponent<Image>();
            if (itemImage == null)
            {
                Debug.LogError("No Image component found on 'background' GameObject!");
            }
        }
        else
        {
            Debug.LogError("No child GameObject named 'background' found!");
        }
    }

    public void Select()
    {
        if (itemImage != null)
        {
            Color color = itemImage.color;
            color.a = 1f; // Đặt mức độ trong suốt cao nhất (alpha = 1)
            itemImage.color = color;
        }
    }

    public void Deselect()
    {
        if (itemImage != null)
        {
            Color color = itemImage.color;
            color.a = 0.5f; // Đặt mức độ trong suốt thấp hơn (alpha = 0.5)
            itemImage.color = color;
        }
    }
}
