using System.Collections;
using UnityEngine;

public class BurningPoint : MonoBehaviour
{
    public bool isBurning = false; // Trạng thái hiện tại của điểm
    public float burningRadius; // Bán kính ảnh hưởng để lan cháy
    public float burningSpreadTime; // Thời gian để lan cháy sang các điểm khác

    private void Update()
    {
        SpreadFire();
        fireParticle();
    }

    private void SpreadFire()
    {
        if (!isBurning) return;

        // Tìm tất cả các đối tượng có component BurningPoint trong phạm vi
        Collider[] colliders = Physics.OverlapSphere(transform.position, burningRadius);

        foreach (Collider collider in colliders)
        {
            BurningPoint burningPoint = collider.GetComponent<BurningPoint>();
            if (burningPoint != null && !burningPoint.isBurning)
            {
                // Bắt đầu lan cháy với thời gian trễ
                StartCoroutine(SpreadFireWithDelay(burningPoint));
            }
        }
    }

    private IEnumerator SpreadFireWithDelay(BurningPoint targetPoint)
    {
        yield return new WaitForSeconds(burningSpreadTime); // Chờ thời gian lan cháy
        targetPoint.isBurning = true; // Kích hoạt trạng thái cháy cho điểm mục tiêu
        // Debug.Log("Fire spread to " + targetPoint.name);
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ bán kính ảnh hưởng trong Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, burningRadius);
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