using UnityEngine;

public class virtualSc : MonoBehaviour
{
    public FlamePoint scrt;
    public void DealDamage(float damage)
    {
        scrt.DescreaseDOC(damage);
        Debug.Log("Hit vir!");
    }
}
