using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        None,
        Pickup,
        Door,
        NPC,
        Button,
        Breakable
    }

    [SerializeField] private InteractableType type = InteractableType.None; // Loại của Interactable

    public void Interact(GameObject player)
    {
        if (type == InteractableType.Breakable)
        {
            Broken();
        }
    }

    void Broken()
    {
        transform.Find("Broken").gameObject.SetActive(true);
        transform.Find("Normal").gameObject.SetActive(false);
    }
}