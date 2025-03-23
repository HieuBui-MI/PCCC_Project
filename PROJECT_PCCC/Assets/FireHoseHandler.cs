using UnityEngine;

public class FireHoseHandler : MonoBehaviour
{
    public Transform handPosition; // Vị trí tay nhân vật
    public Transform fireHosePoint; // Điểm trên xe cứu hỏa
    public GameObject hosePrefab; // Prefab hình trụ tượng trưng cho vòi nước

    private GameObject currentHose;
    private bool isNearHose = false;
    private bool isHoldingHose = false;

    void Update()
    {
        if (isNearHose && Input.GetKeyDown(KeyCode.F))
        {
            ToggleHose();
        }

        if (isHoldingHose)
        {
            UpdateHoseTransform();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FireHose"))
        {
            Debug.Log("Near fire hose");
            isNearHose = true;
            fireHosePoint = fireHosePoint.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FireHose"))
        {
            Debug.Log("Not near fire hose");
            isNearHose = false;
        }
    }

    void ToggleHose()
    {
        if (!isHoldingHose)
        {
            currentHose = Instantiate(hosePrefab);
            currentHose.transform.parent = null; // Đảm bảo không bị khóa theo nhân vật
            isHoldingHose = true;
        }
        else
        {
            Destroy(currentHose);
            isHoldingHose = false;
        }
    }

    void UpdateHoseTransform()
    {
        if (currentHose == null) return;

        Vector3 startPos = fireHosePoint.position;
        Vector3 endPos = handPosition.position;
        Vector3 midPoint = (startPos + endPos) / 2;

        float distance = Vector3.Distance(startPos, endPos);

        currentHose.transform.position = midPoint;
        currentHose.transform.up = (endPos - startPos).normalized; // Xoay theo hướng giữa hai điểm
        currentHose.transform.localScale = new Vector3(0.1f, distance / 2, 0.1f); // Điều chỉnh chiều dài
    }
}
