using UnityEngine;

public class CloneObject : MonoBehaviour
{
    public Material red; // Màu đỏ
    public Material green; // Màu xanh lá cây
    private Renderer[] objectRenderers; // Mảng Renderer của các phần tử con

    void Start()
    {
        Transform model = transform.Find("Model");
        if (model != null)
        {
            objectRenderers = model.GetComponentsInChildren<Renderer>();
        }
        else
        {
            Debug.LogError("No 'Model' child found in this object!");
        }

        if (objectRenderers == null || objectRenderers.Length == 0)
        {
            Debug.LogError("No Renderers found in 'Model'!");
        }

        RemoveObjectRendererMeshCollider();
    }

    void RemoveObjectRendererMeshCollider()
    {
        if (objectRenderers != null)
        {
            foreach (Renderer renderer in objectRenderers)
            {
                MeshCollider meshCollider = renderer.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    Destroy(meshCollider);
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (objectRenderers != null)
        {
            foreach (Renderer renderer in objectRenderers)
            {
                renderer.material = red;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (objectRenderers != null)
        {
            foreach (Renderer renderer in objectRenderers)
            {
                renderer.material = green;
            }
        }
    }
}