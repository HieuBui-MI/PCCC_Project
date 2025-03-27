using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    public GameObject playerCameraRoot;
    private float reachDistance = 10f;
    private Outline previosOutline;
    private bool isTemporaryOutline;

    void Update()
    {
        SetTargetObject();
    }

    void SetTargetObject()
    {
        Vector3 origin = playerCameraRoot.transform.position;
        Vector3 direction = playerCameraRoot.transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, reachDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            Tag tagComponent = hitObject.GetComponent<Tag>();
            if (tagComponent == null || !tagComponent.HasTag("Interactable"))
            {
                return;
            }

            if (hitObject != targetObject)
            {
                RemoveHighlight();
                targetObject = hitObject;
                ApplyHighlight(targetObject);
            }
        }
        else
        {
            RemoveHighlight();
            targetObject = null;
        }
    }

    void ApplyHighlight(GameObject obj)
    {
        Outline outline = obj.GetComponent<Outline>();
        if (outline == null)
        {
            outline = obj.AddComponent<Outline>();
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
            isTemporaryOutline = true;
        }
        else
        {
            isTemporaryOutline = false;
        }

        previosOutline = outline;
        outline.enabled = true;
    }

    void RemoveHighlight()
    {
        if (previosOutline != null)
        {
            previosOutline.enabled = false;


            if (isTemporaryOutline)
            {
                Destroy(previosOutline);
            }

            previosOutline = null;
        }
    }

    public void Interact()
    {
        if (targetObject != null)
        {
            Interactable interactable = targetObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this.gameObject);
            }
        }
    }
}