using StarterAssets;
using UnityEngine;

public class ToolWheelController : MonoBehaviour
{
    public GameObject toolWheel;
    public GameObject player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            toolWheel.SetActive(true);

            StarterAssetsInputs inputs = player.GetComponent<StarterAssetsInputs>();
            if (inputs != null)
            {
                inputs.cursorLocked = false;
                inputs.cursorInputForLook = false;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            toolWheel.SetActive(false);

            StarterAssetsInputs inputs = player.GetComponent<StarterAssetsInputs>();
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
