using StarterAssets;
using UnityEngine;

public class ToolWheelController : MonoBehaviour
{
    private StarterAssetsInputs inputs;
    public GameObject toolWheel;

    private void Awake()
    {
        inputs = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        if (inputs.toolWheel)
        {
            GetComponentInChildren<PlayerScript>().isInWheelSelectionMode = true;
            toolWheel.SetActive(true);

            if (inputs != null)
            {
                inputs.cursorLocked = false;
                inputs.cursorInputForLook = false;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (!inputs.toolWheel)
        {
            GetComponentInChildren<PlayerScript>().isInWheelSelectionMode = false;
            toolWheel.SetActive(false);

            if (inputs != null)
            {
                inputs.cursorLocked = true;
                inputs.cursorInputForLook = true;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
