using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private bool isUsingAxe = false;
    private bool isUsingHammer = false;
    [SerializeField] private GameObject Equipments = null;
    private void Awake()
    {
        Equipments = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == "Equipments").gameObject;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
