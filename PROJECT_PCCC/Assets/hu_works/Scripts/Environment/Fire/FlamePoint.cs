using System.Collections;
using UnityEngine;

public class FlamePoint : MonoBehaviour
{
    public bool isBurning = false; // Trạng thái hiện tại của điểm
    public bool spreadable = false;
    public float spreadRadius; // Bán kính ảnh hưởng để lan cháy
    public float SpreadTime; // Thời gian để lan cháy sang các điểm khác

    private void Update()
    {
        SpreadFire();
        fireParticle();
    }

    private void SpreadFire()
    {
        if (!spreadable) return; 
        if (!isBurning) return;

        // Tìm tất cả các đối tượng có component FlamePoint trong phạm vi
        Collider[] colliders = Physics.OverlapSphere(transform.position, spreadRadius);

        foreach (Collider collider in colliders)
        {
            FlamePoint flamePoint = collider.GetComponent<FlamePoint>();
            if (flamePoint != null && !flamePoint.isBurning)
            {
                // Bắt đầu lan cháy với thời gian trễ
                StartCoroutine(SpreadFireWithDelay(flamePoint));
            }
        }
    }

    private IEnumerator SpreadFireWithDelay(FlamePoint targetPoint)
    {
        yield return new WaitForSeconds(SpreadTime); // Chờ thời gian lan cháy
        targetPoint.isBurning = true; 
    }

    void fireParticle()
    {
        if (isBurning == true)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "WildFire")
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                if (child.name == "WildFire")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}