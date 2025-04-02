using UnityEngine;

public class Victim : MonoBehaviour
{
    [SerializeField] private bool isOnStretcher = false;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        OnStretcherCheck();
        if (isOnStretcher)
        {
            animator.SetBool("isOnStretcher", true);
        }
        else
        {
            animator.SetBool("isOnStretcher", false);
        }
    }

    private void OnStretcherCheck()
    {
        isOnStretcher = transform.parent.GetComponent<Stretcher>() != null;
    }
}
