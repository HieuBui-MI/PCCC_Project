using Unity.VisualScripting;
using UnityEngine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

public class PlayerScript : MonoBehaviour
{
    [Header("Player State")]
    [SerializeField] private bool previousGroundedState = false;
    public bool isPlayerDriving = false;
    public bool isPlayerHoldingTool = false;
    public bool isPlayerCarryingAVictim = false;
    public bool isPlayerCarryingObject = false;
    public bool isPlayerClimbing = false;

    [Header("Equipment State")]
    public bool isUsingFireAxe = false;
    public bool isCarryingLadder = false;
    public bool isCarryingBucket = false;
    public bool isHoldingFireHose = false;
    public bool isHoldingFireExtinguisher = false;
    public bool isHoldingSledgeHammer = false;
    public GameObject currentEquipment = null;

    [Header("Carried Objects")]
    public GameObject carriedVictim = null;
    public GameObject carriedObject = null;
    public GameObject connectableObjectOnHold = null;

    [Header("Interaction Systems")]
    private InteractionSystem interactionSystem;
    private StarterAssetsInputs starterAssetsInputs;
    private PlayerAnimationsHandler playerAnimationsHandler;

    [Header("Placement and Selection Modes")]
    public bool isInPlaceingMode = false;
    public bool isInWheelSelectionMode = false;

    [Header("Vehicle State")]
    public GameObject vehicle = null;

    [Header("Miscellaneous")]
    private float leftClickTimeOutDelta = 0.0f;
    [SerializeField] private float climbSpeed = 5f;
    private void Awake()
    {
        interactionSystem = GetComponent<InteractionSystem>();
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        playerAnimationsHandler = GetComponent<PlayerAnimationsHandler>();
    }

    private void Update()
    {
        Interact();
        PlaceModeState();
        HandleLeftClick();
        ClimbLadder();
        ClimbState();
    }

    void PlaceModeState()
    {
        isInPlaceingMode = carriedObject != null;
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F) && interactionSystem != null)
        {
            interactionSystem.Interact();
        }
    }

    public void ResetToolStates()
    {
        isUsingFireAxe = false;
        isCarryingLadder = false;
        isCarryingBucket = false;
        isHoldingFireHose = false;
        isHoldingFireExtinguisher = false;
        isHoldingSledgeHammer = false;
        currentEquipment = null;
    }

    void HandleLeftClick()
    {
        if (starterAssetsInputs.leftClick && starterAssetsInputs != null)
        {
            if (Time.time >= leftClickTimeOutDelta + 0.1f)
            {
                Debug.Log("1");

                SetIsUsingFireAxe();
                GetComponent<PlacementSystem>().PlaceDownObj();
                leftClickTimeOutDelta = Time.time;
            }
            else
            {
                starterAssetsInputs.leftClick = false;
            }
        }
    }

    public void SetIsUsingFireAxe()
    {
        if (isUsingFireAxe && playerAnimationsHandler != null || isHoldingSledgeHammer && playerAnimationsHandler != null)
        {
            playerAnimationsHandler.AxeBreaking();
        }
    }

    public void AfterActionInteract()
    {
        if (!isUsingFireAxe) return;
        if (interactionSystem.TargetObject == null) return;
        if (interactionSystem.TargetObject.GetComponent<Interactable>() == null) return;
        if (interactionSystem.TargetObject.GetComponent<Interactable>().type != Interactable.InteractableType.Breakable) return;
        interactionSystem.TargetObject.GetComponent<Interactable>().HandleBroken();
    }

    void ClimbLadder()
    {
        if (!isPlayerClimbing) return;
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 climbMovement = new Vector3(0, verticalInput * climbSpeed * Time.deltaTime, 0);

        transform.parent.position += climbMovement;
    }

    void ClimbState()
    {
        FirstPersonController firstPersonController = GetComponentInParent<FirstPersonController>();
        if (firstPersonController == null) return;
        bool currentGroundedState = firstPersonController.Grounded;
        if (!previousGroundedState && currentGroundedState)
        {
            isPlayerClimbing = false;
        }
        previousGroundedState = GetComponentInParent<FirstPersonController>().Grounded;
    }
}
