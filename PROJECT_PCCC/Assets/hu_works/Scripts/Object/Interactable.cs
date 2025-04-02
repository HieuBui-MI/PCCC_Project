using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        None,
        Carriable,
        Putable,
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
            case InteractableType.Carriable:
                CarryVictim(player);
                break;
            case InteractableType.Putable:
                PutVictim(player);
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
        transform.parent.parent.GetComponent<CarController>().driver = player;
        transform.parent.parent.GetComponent<CarController>().ChangeFollowCamera();
    }

    void CarryVictim(GameObject player)
    {
        if (player.GetComponentInChildren<PlayerScript>() != null)
        {
            player.GetComponentInChildren<PlayerScript>().isPlayerCarryingAVictim = true;
            player.GetComponentInChildren<PlayerScript>().carriedVictim = this.gameObject;
            this.transform.SetParent(player.transform);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            this.gameObject.SetActive(false);
        }
    }

    void PutVictim(GameObject player)
    {
        if (player.GetComponentInChildren<PlayerScript>() == null || player.GetComponentInChildren<PlayerScript>().carriedVictim == null) return;
        foreach (Transform child in player.transform)
        {
            if (child.gameObject.name == player.GetComponentInChildren<PlayerScript>().carriedVictim.name)
            {
                child.gameObject.SetActive(true);
                this.GetComponent<Stretcher>().PutVictimInStretcher(child.gameObject);
                player.GetComponentInChildren<PlayerScript>().carriedVictim = null;
                player.GetComponentInChildren<PlayerScript>().isPlayerCarryingAVictim = false;
            }
        }
    }
}
