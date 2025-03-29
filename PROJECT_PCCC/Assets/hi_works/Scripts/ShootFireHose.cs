using UnityEngine;
using Obi; // Đảm bảo bạn đã thêm thư viện Obi vào dự án của mình

public class ShootFireHose : MonoBehaviour
{
    [SerializeField] public GameObject ObiEmitter;
    private ObiEmitter obiEmitterComponent;
    private float targetSpeed = 0f; // Giá trị mục tiêu của speed
    private float speedIncreaseRate = 10f; // Tốc độ tăng của speed
    private float speedDecreaseRate = 20f; // Tốc độ giảm của speed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ObiEmitter != null)
        {
            // Kiểm tra và lấy component ObiEmitter
            obiEmitterComponent = ObiEmitter.GetComponent<ObiEmitter>();
            if (obiEmitterComponent != null)
            {
                Debug.Log("ObiEmitter contains the 'ObiEmitter' component.");
                obiEmitterComponent.speed = 0f; // Đặt speed ban đầu là 0
            }
            else
            {
                Debug.LogWarning("ObiEmitter does not contain the 'ObiEmitter' component.");
            }
        }
        else
        {
            Debug.LogError("ObiEmitter GameObject is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (obiEmitterComponent != null)
        {
            // Kiểm tra trạng thái chuột trái
            if (Input.GetMouseButton(0)) // Nhấn giữ chuột trái
            {
                targetSpeed = 25f; // Đặt mục tiêu speed là 25
                obiEmitterComponent.speed = Mathf.MoveTowards(obiEmitterComponent.speed, targetSpeed, speedIncreaseRate * Time.deltaTime);
            }
            else // Thả chuột trái
            {
                targetSpeed = 0f; // Đặt mục tiêu speed là 0
                obiEmitterComponent.speed = Mathf.MoveTowards(obiEmitterComponent.speed, targetSpeed, speedDecreaseRate * Time.deltaTime);
            }
        }
    }
}
