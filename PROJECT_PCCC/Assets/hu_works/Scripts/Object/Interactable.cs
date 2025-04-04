using StarterAssets;
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
        Climbable,
        Take,
    }
    [SerializeField] private InteractableType type = InteractableType.None;

    public enum CarriableType
    {
        None,
        Object,
        Victim
    }
    [SerializeField] private CarriableType carriableType = CarriableType.None;

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
                if (carriableType == CarriableType.Victim)
                {
                    CarryVictim(player);
                }

                if (carriableType == CarriableType.Object)
                {
                    CarryObject(player);
                }
                break;
            case InteractableType.Putable:
                PutVictim(player);
                break;
            case InteractableType.Climbable:
                Climb(player);
                break;
            case InteractableType.Take:
                Debug.Log($"Interacted with {type}");
                break;
            default:
                Debug.Log($"Interacted with {type}");
                break;
        }
    }

    private void Broken(GameObject player)
    {
        Transform brokenPart = transform.Find("Broken");
        Transform normalPart = transform.Find("Normal");

        if (brokenPart != null) brokenPart.gameObject.SetActive(true);
        if (normalPart != null) normalPart.gameObject.SetActive(false);
    }

    private void DriveVehicle(GameObject player)
    {
        PlayerScript playerScript = player.GetComponentInChildren<PlayerScript>();
        if (playerScript == null) return;

        playerScript.isDriving = true;
        playerScript.vehicle = this.gameObject;

        CarController carController = transform.parent?.parent?.GetComponent<CarController>();
        if (carController != null)
        {
            carController.driver = player;
            carController.ChangeFollowCamera();
        }
    }

    private void CarryVictim(GameObject player)
    {
        PlayerScript playerScript = player.GetComponentInChildren<PlayerScript>();
        if (playerScript == null) return;

        playerScript.isPlayerCarryingAVictim = true;
        playerScript.carriedVictim = this.gameObject;

        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.SetActive(false);
    }

    private void PutVictim(GameObject player)
    {
        PlayerScript playerScript = player.GetComponentInChildren<PlayerScript>();
        if (playerScript == null || playerScript.carriedVictim == null) return;

        GameObject carriedVictim = playerScript.carriedVictim;
        carriedVictim.SetActive(true);

        Stretcher stretcher = GetComponent<Stretcher>();
        if (stretcher != null)
        {
            stretcher.PutVictimInStretcher(carriedVictim);
        }

        playerScript.carriedVictim = null;
        playerScript.isPlayerCarryingAVictim = false;
    }

    void CarryObject(GameObject player)
    {
        if (player.GetComponentInChildren<PlayerScript>().carriedObject == null)
        {
            player.GetComponentInChildren<PlayerScript>().carriedObject = this.gameObject;
            player.GetComponentInChildren<PlacementSystem>().prevCarriedObjectPosition = this.transform.position;
        }
    }
    private void Climb(GameObject player)
    {
        PlayerScript playerScript = player.GetComponentInChildren<PlayerScript>();
        Animator animator = player.GetComponentInChildren<Animator>();
        if (playerScript == null) return;

        // Justify the player position to the ladder position
        Vector3 playerCurrentPosition = player.transform.position;
        player.transform.position = new Vector3(playerCurrentPosition.x, playerCurrentPosition.y + 0.5f, playerCurrentPosition.z);

        // Set the player to climb the ladder
        playerScript.isPlayerClimbing = true;
        if (animator != null)
        {
            animator.SetTrigger("Climb");
        }
    }
}