using Unity.VisualScripting;
using UnityEngine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

public class PlayerScript : MonoBehaviour
{
    public bool isDriving = false;
    public GameObject vehicle;
    public bool isPlayerHoldingTool = false;
    public bool isPlayerCarryingAVictim = false;

    ////////////////////////////////////////////////
    public bool isUsingFireAxe = false;
    public bool isCarryingLadder = false;
    public bool isCarryingBucket = false;
    public bool isHoldingFireHose = false;

    private float leftClickTimeOutDelta = 0.0f;
    public InteractionSystem interactionSystem;
    private StarterAssetsInputs starterAssetsInputs;
    private PlayerAnimationsHandler playerAnimationsHandler;
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
    }
    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Interact key pressed");
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
        if (starterAssetsInputs.leftClick)
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
        if (isUsingFireAxe)
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
}
