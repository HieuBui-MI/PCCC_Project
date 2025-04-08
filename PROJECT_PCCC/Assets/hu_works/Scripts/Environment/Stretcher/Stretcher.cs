using TreeEditor;
using UnityEngine;

public class Stretcher : MonoBehaviour
{
    [SerializeField] private GameObject victim;
    public bool isOcupied = false;

    private void Update()
    {
        SetVictim();
        isOcupied = victim != null;
    }

    public void PutVictimInStretcher(GameObject victim)
    {
        victim.transform.SetParent(transform);

        victim.transform.localPosition = new Vector3(0f, -0.0275f, 0.0313612074f); // Đặt vị trí lên trên một chút
        victim.transform.localRotation = Quaternion.identity; // Đặt rotation về mặc định (không xoay)
    }

    private void SetVictim()
    {
        foreach (Transform item in transform)
        {
            if (item.GetComponent<Victim>() != null)
            {
                victim = item.gameObject;
                break;
            }
        }
    }
}