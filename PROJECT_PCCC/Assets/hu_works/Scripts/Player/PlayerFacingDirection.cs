using UnityEngine;

public class PLa : MonoBehaviour
{
    [SerializeField] private GameObject playerCameraRoot;
    [SerializeField] private bool isAiming;
    private void Awake()
    {
        playerCameraRoot = transform.Find("PlayerCameraRoot").gameObject;
    }

    void Update()
    {
        if (isAiming)
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            Vector3 targetRotation = playerCameraRoot.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotation.x, targetRotation.y, currentRotation.z);
        }
    }
}
