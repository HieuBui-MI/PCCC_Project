using UnityEngine;

public class WaterCollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu đối tượng va chạm có tag là "Cube"
        if (collision.gameObject.tag == "Cube")
        {
            Debug.Log("Hit Cube!");
            Destroy(gameObject); // Hủy viên đạn ngay lập tức
        }
        else
        {
            Debug.Log("Hit something else!");
            Destroy(gameObject); // Hủy viên đạn ngay lập tức
        }
    }
}
