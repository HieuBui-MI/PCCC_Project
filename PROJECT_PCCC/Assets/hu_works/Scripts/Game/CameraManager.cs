using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject playerFollowCamera;
    GameObject vehicleFollowCamera;
    private void Awake()
    {
        playerFollowCamera = transform.Find("PlayerFollowCamera").gameObject;
        vehicleFollowCamera = transform.Find("VehicleFollowCamera").gameObject;
    }
    public void ChangeCameraRoot_Player_Vehicle(GameObject player, GameObject vehicle)
    {
        if (playerFollowCamera.activeSelf == true && vehicleFollowCamera.activeSelf == false)
        {
            // Chuyển từ chế độ người chơi sang chế độ lái xe
            playerFollowCamera.SetActive(false);
            vehicleFollowCamera.SetActive(true);

            // Đặt player làm con của vehicle và ẩn player
            player.transform.SetParent(vehicle.transform);
            player.SetActive(false); // Ẩn người chơi
        }
        else if (playerFollowCamera.activeSelf == false && vehicleFollowCamera.activeSelf == true)
        {
            // Thoát khỏi xe và mở lại player
            foreach (Transform item in vehicle.transform)
            {
                if (item.name == player.name)
                {
                    player.transform.SetParent(GameObject.Find("Ground").transform); 
                    item.gameObject.SetActive(true); 
                    break;
                }
            }
            
            // Đặt lại các giá trị
            vehicle.GetComponent<CarController>().driver = null; 
            playerFollowCamera.SetActive(true);
            vehicleFollowCamera.SetActive(false);

            player.GetComponentInChildren<PlayerScript>().isDriving = false;
            player.GetComponentInChildren<PlayerScript>().vehicle = null;
        }
    }
}
