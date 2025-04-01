using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        None,
        Carry,
        Door,
        Drivable,
        Breakable,
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
            case InteractableType.Carry:
                CarryVictim(player);
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

    void CarryVictim(GameObject player)
    {
        Debug.Log("Carrying victim");
        if(player.GetComponentInChildren<PlayerScript>() != null)
        {
            player.GetComponentInChildren<PlayerScript>().isPlayerCarryingAVictim = true;
        }
        else
        {
            Debug.LogWarning("Player script not found in children!");
        }
    }
}
