using UnityEngine;

public class Stretcher : MonoBehaviour
{
    [SerializeField] private GameObject victim;

    private void Update()
    {
        SetVictim();
    }

    public void PutVictimInStretcher(GameObject victim)
    {
        victim.transform.parent = transform;
    }

    private void SetVictim()
    {
        victim = transform.childCount > 0? transform.GetChild(0).gameObject : null;
    }
}