using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    private SphereCollider detector;
    [SerializeField] private GameObject detectedObject;
    public string tagToDetect = "BreakableObject";

    private void Awake()
    {
        if (detector == null) detector = GetComponent<SphereCollider>();

        if (detector != null)
        {
            detector.isTrigger = true;
        }
    }

    private void Update()
    {
        InteractBreak();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToDetect))
        {
            detectedObject = other.gameObject;
            Debug.Log("Đã phát hiện vật thể có thể tương tác: " + detectedObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagToDetect))
        {
            if (detectedObject == other.gameObject)
            {
                detectedObject = null;
            }
        }
    }

    public GameObject GetDetectedObject()
    {
        return detectedObject;
    }

    void InteractBreak()
    {
        if (detectedObject != null && Input.GetKeyDown(KeyCode.Mouse0) && GetComponent<InventorySystem>().isUsingAxe)
        {
            GetComponent<PlayerAnimationsHandler>().AxeBreaking();
        }
    }

    public void ObjectBreak()
    {
        if (detectedObject != null)
        {
            detectedObject.GetComponent<BreakableObject>().Beinteracted();
        }
    }
}
