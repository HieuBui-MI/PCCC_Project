using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        None,
        Pickup,
        Door,
        Drivable,
        Breakable
    }

    [SerializeField] private InteractableType type = InteractableType.None;
    public void InteractCase(GameObject player)
    {
        switch (type)
        {
            case InteractableType.Breakable:
                Broken(player);
                break;
            case InteractableType.Drivable:
                DriveVehicle(player);
                break;
            default:
                Debug.Log($"Interacted with {type}");
                break;
        }
    }

    void Broken(GameObject player)
    {
        Transform brokenPart = transform.Find("Broken");
        Transform normalPart = transform.Find("Normal");

        if (brokenPart != null) brokenPart.gameObject.SetActive(true);
        if (normalPart != null) normalPart.gameObject.SetActive(false);
    }

    void DriveVehicle(GameObject player)
    {
        PlayerScript playerScript = player.GetComponentInChildren<PlayerScript>();
        playerScript.isDriving = true;
        playerScript.vehicle = this.gameObject;
        GetComponent<CarController>().driver = player;
        GetComponent<CarController>().ChangeFollowCamera();
    }
}
