using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationsHandler : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    private InventorySystem inventorySystem;
    private PlayerScript playerScript;
    private InteractionSystem interactionSystem;
    private Animator animator;
    private bool isInAction = false;
    /////////////////////////////////////////////////////////////////
    [SerializeField] private GameObject rightHandIKTarget;
    [SerializeField] private GameObject leftHandIKTarget;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        playerScript = GetComponent<PlayerScript>();
        inventorySystem = GetComponent<InventorySystem>();
        interactionSystem = GetComponent<InteractionSystem>();
    }

    void Update()
    {
        PoseStateHandler();
        OnClimb();
        RHWeightForFireHose();
        animator.SetBool("isInPlacingMode", GetComponent<PlayerScript>().isInPlacingMode);
    }
    void RHWeightForFireHose()
    {
        if (GetComponent<PlayerScript>().isHoldingFireHose || GetComponent<PlayerScript>().isHoldingFireExtinguisher)
        {
            transform.Find("Rig2/RightHandAim").GetComponent<MultiAimConstraint>().weight = 1f;
            transform.Find("Rig1/RightHandIK").GetComponent<TwoBoneIKConstraint>().weight = 1f;
            rightHandIKTarget.transform.localPosition = new Vector3(0.226938576f,-0.207237259f,0.332701743f);
            rightHandIKTarget.transform.localRotation = new Quaternion(-0.835340321f,-0.030891275f,0.544437885f,0.0695679039f);
        }
        else if (!GetComponent<PlayerScript>().isHoldingFireHose)
        {
            transform.Find("Rig2/RightHandAim").GetComponent<MultiAimConstraint>().weight = 0f;
            transform.Find("Rig1/RightHandIK").GetComponent<TwoBoneIKConstraint>().weight = 0f;
        }
    }



    void PoseStateHandler()
    {
        animator.SetBool("isUsingAxe", GetComponent<PlayerScript>().isUsingFireAxe);
        animator.SetBool("isHoldingFireHose", GetComponent<PlayerScript>().isHoldingFireHose);
        animator.SetBool("isHoldingFireExtinguisher", GetComponent<PlayerScript>().isHoldingFireExtinguisher);
        animator.SetBool("isHoldingSledgeHammer", GetComponent<PlayerScript>().isHoldingSledgeHammer);
        animator.SetBool("isCarryingLadder", GetComponent<PlayerScript>().isCarryingLadder);
        animator.SetBool("isCarryingBucket", GetComponent<PlayerScript>().isCarryingBucket);
    }

    public void AxeBreaking()
    {
        isInAction = true;
        animator.SetTrigger("AxeBreak");
    }

    public void resetActionState()
    {
        isInAction = false;
        interactionSystem.Interact();
    }
    public void OnLand()
    {
    }

    public void OnClimb()
    {
        isInAction = GetComponent<PlayerScript>().isPlayerClimbing;
        animator.SetBool("isClimbing", GetComponent<PlayerScript>().isPlayerClimbing);
    }
}
