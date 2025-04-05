using Unity.VisualScripting;
using UnityEngine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

public class PlayerScript : MonoBehaviour
{
    private bool previousGroundedState = false;
    //////////////////////////////////////////////////////
    public bool isDriving = false;
    public GameObject vehicle;
    public bool isUsingFireAxe = false;
    public bool isCarryingLadder = false;
    public bool isCarryingBucket = false;
    public bool isHoldingFireHose = false;
    public bool isPlayerHoldingTool = false;
    public bool isPlayerCarryingAVictim = false;
    public GameObject carriedVictim = null;
    public bool isPlayerCarryingObject = false;
    public GameObject carriedObject = null;
    private float leftClickTimeOutDelta = 0.0f;
    public bool isPlayerClimbing = false;
    public GameObject connectableObjectOnHold = null;
    ///////////////////////////////////////////////////////
    private InteractionSystem interactionSystem;
    private StarterAssetsInputs starterAssetsInputs;
    private PlayerAnimationsHandler playerAnimationsHandler;

    ///////////////////////////////////////////////////////
    [SerializeField] float climbSpeed = 5f;
    private void Awake()
    {
        interactionSystem = GetComponent<InteractionSystem>();
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        playerAnimationsHandler = GetComponent<PlayerAnimationsHandler>();
    }

    private void Update()
    {
        Interact();
        HandleLeftClick();
        RHWeightForFireHose();
        ClimbLadder();
        ClimbState();
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
    }

    void HandleLeftClick()
    {
        if (starterAssetsInputs.leftClick && starterAssetsInputs != null)
        {
            if (Time.time >= leftClickTimeOutDelta + 0.1f)
            {
                SetIsUsingFireAxe();
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
        if (isUsingFireAxe && playerAnimationsHandler != null)
        {
            playerAnimationsHandler.AxeBreaking();
        }
    }

    void RHWeightForFireHose()
    {
        if (isHoldingFireHose)
        {
            transform.Find("Rig2/RightHandAim").GetComponent<MultiAimConstraint>().weight = 1f;
        }
        else
        {
            transform.Find("Rig2/RightHandAim").GetComponent<MultiAimConstraint>().weight = 0f;
        }
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
