using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlamePoint : MonoBehaviour
{
    public bool isBurning = false; // Trạng thái hiện tại của điểm
    public bool spreadable = false;
    public float spreadRadius; // Bán kính ảnh hưởng để lan cháy
    public float spreadTime; // Thời gian để lan cháy sang các điểm khác
    private float spreadTimer = 0f; // Bộ đếm thời gian cho SpreadFire
    public float spreadDamage;
    [SerializeField] private float degreeoOfCombustion;
    [SerializeField] private float maxdegreeoOfCombustion;

    private List<FlamePoint> flamePointColliders = new List<FlamePoint>();

    private void Start()
    {
        // Đặt giá trị ban đầu cho degreeoOfCombustion dựa trên trạng thái isBurning
        degreeoOfCombustion = isBurning ? maxdegreeoOfCombustion : 0f;

        // Tìm tất cả các FlamePoint trong bán kính spreadRadius
        Collider[] colliders = Physics.OverlapSphere(transform.position, spreadRadius);
        foreach (Collider collider in colliders)
        {
            FlamePoint flamePoint = collider.GetComponent<FlamePoint>();
            if (flamePoint != null)
            {
                // Thêm FlamePoint vào danh sách
                flamePointColliders.Add(flamePoint);
            }
        }
    }

    private void Update()
    {
        SpreadFire();
        fireParticle();
        SetBurning();
    }

    private void SpreadFire()
    {
        if (!spreadable || !isBurning) return;

        spreadTimer += Time.deltaTime;

        if (spreadTimer >= spreadTime)
        {
            foreach (FlamePoint flamePoint in flamePointColliders)
            {
                if (flamePoint.degreeoOfCombustion < flamePoint.maxdegreeoOfCombustion && flamePoint != this)
                {
                    flamePoint.TakeDamage(spreadDamage);
                }
            }
            spreadTimer = 0f;
        }
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

    public void TakeDamage(float damage)
    {
        degreeoOfCombustion += damage;
        if (degreeoOfCombustion >= maxdegreeoOfCombustion)
        {
            degreeoOfCombustion = maxdegreeoOfCombustion;
        }
    }

    public void SetBurning()
    {
        if (degreeoOfCombustion >= maxdegreeoOfCombustion / 2)
        {
            isBurning = true;
        }
        else
        {
            isBurning = false;
        }
    }

}