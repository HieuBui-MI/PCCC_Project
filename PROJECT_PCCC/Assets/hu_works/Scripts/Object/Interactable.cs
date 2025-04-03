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
        Climbable
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
            case InteractableType.Climbable:
                Climb(player);
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

    private void Climb(GameObject player)
    {
        PlayerScript playerScript = player.GetComponentInChildren<PlayerScript>();
        Animator animator = player.GetComponentInChildren<Animator>();
        if (playerScript == null) return;
        playerScript.isPlayerClimbing = true;
        if (animator != null)
        {
            animator.SetTrigger("Climb");
        }

    }
}