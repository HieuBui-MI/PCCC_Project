using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool isDriving = false;
    public GameObject vehicle;
    public bool isPlayerHoldingTool = false;
    public bool isUsingAxe = false;
    public bool isCarryingLadder = false;
    public bool isCarryingBucket = false;
    public bool isCarryingRope = false;
    public InteractionSystem interactionSystem;
    private void Awake()
    {
        interactionSystem = GetComponent<InteractionSystem>();
    }
    private void Update()
    {
        Interact();
    }
    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            interactionSystem.Interact();
        }
    }
}
