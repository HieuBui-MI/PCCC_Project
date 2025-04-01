using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlamePoint : MonoBehaviour
{
    public bool isBurning = false; // Trạng thái hiện tại của điểm
    public bool spreadable = false; // Có thể lan cháy hay không
    public float spreadRadius; // Bán kính ảnh hưởng để lan cháy
    public float spreadTime; // Thời gian để lan cháy sang các điểm khác
    private float spreadTimer = 0f; // Bộ đếm thời gian cho SpreadFire
    private float selfIncreaseTimer = 0f; // Bộ đếm thời gian cho việc hồi DOC
    public float spreadDamage; // Lực cân bằng lan cháy cho các điểm khác
    [SerializeField] private float degreeoOfCombustion;
    [SerializeField] private float maxdegreeoOfCombustion;

    private List<FlamePoint> flamePointColliders = new List<FlamePoint>();

    private void Start()
    {
        // Đặt giá trị ban đầu cho degreeoOfCombustion dựa trên trạng thái isBurning
        degreeoOfCombustion = isBurning ? maxdegreeoOfCombustion : 0f;
        DetectSurroundingFlamePoints();

    }

    private void Update()
    {
        SpreadFire();
        FireParticle();
        SetBurning();
        SelfIncreaseDOC();
    }

    /// Kiểm tra xem điểm có đang cháy hay không
    public void SetBurning()
    {
        isBurning = degreeoOfCombustion >= maxdegreeoOfCombustion / 2;
    }

    // Tìm tất cả các FlamePoint trong bán kính spreadRadius
    void DetectSurroundingFlamePoints()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, spreadRadius);
        foreach (Collider collider in colliders)
        {
            FlamePoint flamePoint = collider.GetComponent<FlamePoint>();
            if (flamePoint != null)
            {
                flamePointColliders.Add(flamePoint);
            }
        }
    }

    private void SpreadFire()
    {
        if (!spreadable || !isBurning) return;

        spreadTimer += Time.deltaTime;
        if (spreadTimer >= spreadTime)
        {
            foreach (FlamePoint flamePoint in flamePointColliders)
            {
                if (flamePoint != this && !flamePoint.isBurning)
                {
                    flamePoint.IncreaseDOC(spreadDamage);
                }
            }
            spreadTimer = 0f;
        }
    }

    public void SelfIncreaseDOC()
    {
        if (isBurning)
        {
            selfIncreaseTimer += Time.deltaTime;
            if (selfIncreaseTimer >= spreadTime)
            {
                degreeoOfCombustion += spreadDamage;
                degreeoOfCombustion = Mathf.Min(degreeoOfCombustion, maxdegreeoOfCombustion);
                selfIncreaseTimer = 0f;
            }
        }
        else
        {
            selfIncreaseTimer = 0f;
        }
    }

    void FireParticle()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "WildFire")
            {
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    // Tính toán giá trị normalized từ 0 đến 1 dựa trên DOC
                    float normalizedCombustion = (degreeoOfCombustion - (maxdegreeoOfCombustion / 2)) / (maxdegreeoOfCombustion / 2);
                    normalizedCombustion = Mathf.Clamp01(normalizedCombustion); // Đảm bảo giá trị nằm trong khoảng [0, 1]

                    // Tính toán radius từ 0.1 đến 1.0 dựa trên normalizedCombustion
                    float targetRadius = Mathf.Lerp(0.1f, 1.0f, normalizedCombustion);

                    // Điều chỉnh radius của Particle System
                    var shape = particleSystem.shape;
                    shape.radius = targetRadius;

                    // Bật hoặc tắt Particle System dựa trên trạng thái isBurning
                    child.gameObject.SetActive(isBurning);
                }
            }
        }
    }

    public void IncreaseDOC(float damage)
    {
        degreeoOfCombustion += damage;
        degreeoOfCombustion = degreeoOfCombustion >= maxdegreeoOfCombustion ? maxdegreeoOfCombustion : degreeoOfCombustion;
    }

    public void DescreaseDOC(float damage)
    {
        degreeoOfCombustion -= damage;
        degreeoOfCombustion = degreeoOfCombustion <= 0f ? 0f : degreeoOfCombustion;
    }
}