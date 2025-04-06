using UnityEngine;
using System.Collections;

public class GenRope : MonoBehaviour
{
    private FireHydrant fireHydrant;
    private bool hasLoggedPositions = false; // Biến cờ để kiểm tra trạng thái
    public GameObject parentRope;
    public GameObject ropePrefab;
    private void Start()
    {
        fireHydrant = GetComponent<FireHydrant>();
    }

    void Update()
    {
        if (GetComponent<FireHydrant>().isConnectedToFireTruck && !hasLoggedPositions)
        {
            CreateRope();
            hasLoggedPositions = true;
        }
    }

    public void CreateRope()
    {
        Vector3 fireHydrantPosition = transform.Find("ConnectPoint").position;
        Vector3 connectedObjectPosition = fireHydrant.objConnectedTo.transform.Find("ConnectPoint").position;

        Vector3 middlePosition = (fireHydrantPosition + connectedObjectPosition) / 2;
        GameObject rope = Instantiate(ropePrefab, middlePosition, Quaternion.identity);

        if (parentRope != null)
        {
            rope.transform.SetParent(parentRope.transform);
        }

        // Bắt đầu Coroutine để delay 3 giây trước khi đặt vị trí cho S1 và S2
        StartCoroutine(SetRopePositionsWithDelay(rope, GetComponent<FireHydrant>().objConnectedTo));
    }

    private IEnumerator SetRopePositionsWithDelay(GameObject rope, GameObject connectedObject)
    {
        WaterSourceConnector waterSourceConnector = connectedObject.GetComponent<WaterSourceConnector>();

        // Chờ 3 giây
        yield return new WaitForSeconds(0.5f);

        // Tìm các GameObject con S1 và S2 trong ropePrefab
        Transform s1 = rope.transform.Find("S1");
        Transform s2 = rope.transform.Find("S2");

        if (s1 != null)
        {
            s1.position = transform.Find("ConnectPoint").position; // Đặt vị trí của S1
        }
        else
        {
            Debug.LogWarning("S1 not found in ropePrefab.");
        }

        if (s2 != null)
        {
            s2.position = waterSourceConnector.transform.Find("ConnectPoint").position; // Đặt vị trí của S2
        }
        else
        {
            Debug.LogWarning("S2 not found in ropePrefab.");
        }

        Debug.Log("Positions of S1 and S2 set after delay.");
    }
}
