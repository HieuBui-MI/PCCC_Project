using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject Equipments = null;
    public bool isPlayerHoldingTool = false;
    public bool isUsingAxe = false;
    private void Awake()
    {
        Equipments = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Equipments").gameObject;
    }
    void Start()
    {
        // foreach (Transform child in Equipments.transform)
        // {
        //     if (child.GetComponent<Tool>().toolName == "Axe" && child.gameObject.activeSelf == true)
        //     {
        //         isPlayerHoldingTool = true;
        //         isUsingAxe = true;
        //     }
        // }
    }

    void Update()
    {
        SwitchTools();
    }

    void SwitchTools()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isPlayerHoldingTool = !isPlayerHoldingTool;
            isUsingAxe = !isUsingAxe;
        }
    }
}
