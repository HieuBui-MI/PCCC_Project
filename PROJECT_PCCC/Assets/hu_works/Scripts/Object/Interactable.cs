using StarterAssets;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        None,
        Door,
        Drivable,
        Breakable,
        Climbable,
        Take,
        Connectable,
    }
    public InteractableType type = InteractableType.None;

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
        PlayerState playerScript = player.GetComponentInChildren<PlayerState>();
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

    private void HandleClimb(GameObject player)
    {
        PlayerState playerScript = player.GetComponentInChildren<PlayerState>();
        Animator animator = player.GetComponentInChildren<Animator>();
        if (playerScript == null) return;

        Vector3 playerCurrentPosition = player.transform.position;
        player.transform.position = new Vector3(playerCurrentPosition.x, playerCurrentPosition.y + 0.5f, playerCurrentPosition.z);

        playerScript.isPlayerClimbing = true;
        animator?.SetTrigger("Climb");
    }

    private void HandleConnectObject(GameObject player)
    {
        PlayerState playerScript = player.GetComponentInChildren<PlayerState>();
        DetectorSystem detectorSystem = player.GetComponentInChildren<DetectorSystem>();
        if (playerScript == null || detectorSystem == null) return;

        if (playerScript.connectableObjectOnHold == null)
        {
            if (playerScript.isHoldingFireHose && detectorSystem.TargetObject.GetComponent<PipeConnector>().isConnectToFireHoseOnly)
            {
                ConnectObject(playerScript.currentEquipment, detectorSystem.TargetObject);
            }
            else if (!detectorSystem.TargetObject.GetComponent<PipeConnector>().isConnectToFireHoseOnly)
            {
                playerScript.connectableObjectOnHold = this.gameObject;
            }
        }
        else
        {
            ConnectObject(playerScript.connectableObjectOnHold, detectorSystem.TargetObject);
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