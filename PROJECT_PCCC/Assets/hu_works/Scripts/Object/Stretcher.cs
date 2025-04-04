using TreeEditor;
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
        victim.transform.SetParent(transform); // Đặt cha cho victim

        // // Đặt vị trí và rotation cho victim
        // victim.transform.localPosition = Vector3.zero; // Đặt vị trí về (0, 0, 0)
        victim.transform.localPosition = new Vector3(0f,-0.025f,0.0313612074f); // Đặt vị trí lên trên một chút
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