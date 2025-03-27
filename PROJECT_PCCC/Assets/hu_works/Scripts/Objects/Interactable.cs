using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        None,
        Pickup,
        Door,
        Burnable,
        Button,
        Breakable
    }

    [SerializeField] private InteractableType type = InteractableType.None;

    [System.Serializable]
    public class BurnableOptions
    {
        [Range(0f, 100f)] public float flammability = 0.5f;
        public float burnDuration = 5f;
    }

    [SerializeField] private BurnableOptions burnableOptions; 

    public void Interact(GameObject player)
    {
        switch (type)
        {
            case InteractableType.Breakable:
                Broken();
                break;
            case InteractableType.Burnable:
                Burn();
                break;
            default:
                Debug.Log($"Interacted with {type}");
                break;
        }
    }

    void Broken()
    {
        Transform brokenPart = transform.Find("Broken");
        Transform normalPart = transform.Find("Normal");

        if (brokenPart != null) brokenPart.gameObject.SetActive(true);
        if (normalPart != null) normalPart.gameObject.SetActive(false);
    }

    void Burn()
    {
        if (burnableOptions != null)
        {
            Debug.Log($"Burning {gameObject.name} with flammability: {burnableOptions.flammability} and burn duration: {burnableOptions.burnDuration}s");
        }
    }
}
