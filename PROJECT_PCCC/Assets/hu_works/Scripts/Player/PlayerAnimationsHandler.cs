using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationsHandler : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    private InventorySystem inventorySystem;
    private PlayerScript playerScript;
    private InteractionSystem interactionSystem;
    private Animator animator;
    private bool isInAction = false;
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
    }

    void PoseStateHandler()
    {
        animator.SetBool("isUsingAxe", GetComponent<PlayerScript>().isUsingFireAxe);
        animator.SetBool("isHoldingFireHose", GetComponent<PlayerScript>().isHoldingFireHose);
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
