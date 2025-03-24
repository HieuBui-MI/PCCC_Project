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
    }

    void Update()
    {
        SwitchTools();
        ActiveEquipTool();
    }

    void SwitchTools()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isPlayerHoldingTool = !isPlayerHoldingTool;
            isUsingAxe = !isUsingAxe;
        }
    }

    void ActiveEquipTool()
    {
        foreach (Transform child in Equipments.transform)
        {
            child.gameObject.SetActive(isUsingAxe);
        }
    }
}
