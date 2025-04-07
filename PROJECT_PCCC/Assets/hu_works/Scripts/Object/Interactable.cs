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
        Connectable,
    }
    public InteractableType type = InteractableType.None;

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
                HandleBroken();
                break;
            case InteractableType.Drivable:
                HandleDriveVehicle(player);
                break;
            case InteractableType.Carriable:
                HandleCarriable(player);
                break;
            case InteractableType.Putable:
                HandlePutVictim(player);
                break;
            case InteractableType.Climbable:
                HandleClimb(player);
                break;
            case InteractableType.Connectable:
                HandleConnectObject(player);
                break;
            default:
                Debug.Log($"Interacted with {type}");
                break;
        }
    }

    public void HandleBroken()
    {
        Transform brokenPart = transform.Find("Broken");
        Transform normalPart = transform.Find("Normal");

        if (brokenPart != null) brokenPart.gameObject.SetActive(true);
        if (normalPart != null) normalPart.gameObject.SetActive(false);
    }

    private void HandleDriveVehicle(GameObject player)
    {
        PlayerScript playerScript = player.GetComponentInChildren<PlayerScript>();
        if (playerScript == null) return;

        playerScript.isPlayerDriving = true;
        playerScript.vehicle = this.gameObject;

        CarController carController = transform.parent?.parent?.GetComponent<CarController>();
        if (carController != null)
        {
            carController.driver = player;
            carController.ChangeFollowCamera();
        }
    }

    private void HandleCarriable(GameObject player)
    {
        if (carriableType == CarriableType.Victim)
        {
            CarryVictim(player);
        }
        else if (carriableType == CarriableType.Object)
        {
            CarryObject(player);
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

    private void HandlePutVictim(GameObject player)
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

    private void CarryObject(GameObject player)
    {
        PlayerScript playerScript = player.GetComponentInChildren<PlayerScript>();
        if (playerScript == null || playerScript.carriedObject != null) return;

        playerScript.carriedObject = this.gameObject;
        player.GetComponentInChildren<PlacementSystem>().prevCarriedObjectPosition = this.transform.position;
    }

    private void HandleClimb(GameObject player)
    {
        PlayerScript playerScript = player.GetComponentInChildren<PlayerScript>();
        Animator animator = player.GetComponentInChildren<Animator>();
        if (playerScript == null) return;

        Vector3 playerCurrentPosition = player.transform.position;
        player.transform.position = new Vector3(playerCurrentPosition.x, playerCurrentPosition.y + 0.5f, playerCurrentPosition.z);

        playerScript.isPlayerClimbing = true;
        animator?.SetTrigger("Climb");
    }

    private void HandleConnectObject(GameObject player)
    {
        PlayerScript playerScript = player.GetComponentInChildren<PlayerScript>();
        InteractionSystem interactionSystem = player.GetComponentInChildren<InteractionSystem>();
        if (playerScript == null || interactionSystem == null) return;

        if (playerScript.connectableObjectOnHold == null)
        {
            if (playerScript.isHoldingFireHose && interactionSystem.TargetObject.GetComponent<PipeConnector>().isConnectToFireHoseOnly)
            {
                ConnectObject(playerScript.currentEquipment, interactionSystem.TargetObject);
            }
            else if (!interactionSystem.TargetObject.GetComponent<PipeConnector>().isConnectToFireHoseOnly)
            {
                playerScript.connectableObjectOnHold = this.gameObject;
            }
        }
        else
        {
            ConnectObject(playerScript.connectableObjectOnHold, interactionSystem.TargetObject);
            playerScript.connectableObjectOnHold = null;
        }
    }

    private void ConnectObject(GameObject obj1, GameObject obj2)
    {
        if (TryConnect(obj1, obj2))
        {
            Debug.Log("Objects connected successfully.");
        }
        else
        {
            Debug.Log("Failed to connect objects.");
        }
    }

    private bool TryConnect(GameObject obj1, GameObject obj2)
    {
        PipeConnector connector1 = obj1.GetComponent<PipeConnector>();
        PipeConnector connector2 = obj2.GetComponent<PipeConnector>();

        if (connector1 != null && connector2 != null)
        {
            if (connector1.objConnectedTo != null || connector2.objConnectedTo != null)
            {
                Debug.Log("One or both objects are already connected.");
                return false;
            }

            connector1.objConnectedTo = connector2.gameObject;
            connector2.objConnectedTo = connector1.gameObject;
            return true;
        }

        return false;
    }
}