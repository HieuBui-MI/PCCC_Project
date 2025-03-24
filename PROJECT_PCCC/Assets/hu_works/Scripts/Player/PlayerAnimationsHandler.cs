using UnityEngine;

public class PlayerAnimationsHandler : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {

    }

    void Update()
    {
        AxePose();
    }

    void AxePose()
    {
        animator.SetBool("isUsingAxe", GetComponent<InventorySystem>().isUsingAxe);
    }

    public void AxeBreaking(){
        animator.SetTrigger("AxeBreak");
    }
}
