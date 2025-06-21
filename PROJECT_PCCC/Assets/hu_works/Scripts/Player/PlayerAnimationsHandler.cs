using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationsHandler : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    private InventorySystem inventorySystem;
    private PlayerState playerState;
    private DetectorSystem detectorSystem;
    private Animator animator;
    private bool isInAction = false;
    /////////////////////////////////////////////////////////////////
    [SerializeField] private GameObject rightHandIKTarget;
    [SerializeField] private GameObject leftHandIKTarget;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        playerState = GetComponent<PlayerState>();
        inventorySystem = GetComponent<InventorySystem>();
        detectorSystem = GetComponent<DetectorSystem>();
    }

    void Update()
    {
        PoseStateHandler();
        OnClimb();
        RHWeightForFireHose();
        animator.SetBool("isInPlacingMode", playerState.isInCarryState);
    }
    void RHWeightForFireHose()
    {
        if (playerState.isHoldingFireHose || playerState.isHoldingFireExtinguisher)
        {
            transform.Find("Rig2/RightHandAim").GetComponent<MultiAimConstraint>().weight = 1f;
            transform.Find("Rig1/RightHandIK").GetComponent<TwoBoneIKConstraint>().weight = 1f;
            rightHandIKTarget.transform.localPosition = new Vector3(0.226938576f,-0.207237259f,0.332701743f);
            rightHandIKTarget.transform.localRotation = new Quaternion(-0.835340321f,-0.030891275f,0.544437885f,0.0695679039f);
        }
        else if (!playerState.isHoldingFireHose)
        {
            transform.Find("Rig2/RightHandAim").GetComponent<MultiAimConstraint>().weight = 0f;
            transform.Find("Rig1/RightHandIK").GetComponent<TwoBoneIKConstraint>().weight = 0f;
        }
    }



    void PoseStateHandler()
    {
        animator.SetBool("isUsingAxe", playerState.isUsingFireAxe);
        animator.SetBool("isHoldingFireHose", playerState.isHoldingFireHose);
        animator.SetBool("isHoldingFireExtinguisher", playerState.isHoldingFireExtinguisher);
        animator.SetBool("isHoldingSledgeHammer", playerState.isHoldingSledgeHammer);
        animator.SetBool("isCarryingLadder", playerState.isCarryingLadder);
        animator.SetBool("isCarryingBucket", playerState.isCarryingBucket);
    }

    public void AxeBreaking()
    {
        isInAction = true;
        animator.SetTrigger("AxeBreak");
    }

    public void resetActionState()
    {
        isInAction = false;
        detectorSystem.Interact();
    }
    public void OnLand()
    {
    }

    public void OnClimb()
    {
        isInAction = playerState.isPlayerClimbing;
        animator.SetBool("isClimbing", playerState.isPlayerClimbing);
    }
}
