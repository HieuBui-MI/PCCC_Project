using System;
using System.Collections;
using UnityEngine;

public class SetChildPosition : MonoBehaviour
{
    public GameObject parentObject; // Đối tượng cha
    public GameObject childObject;  // Đối tượng con

    void Start()
    {
        // Kiểm tra xem cha và con có tồn tại không
        if (parentObject != null && childObject != null)
        {
            childObject.transform.parent = parentObject.transform;
            StartCoroutine(WaitAndPrint());
        }
        else
        {
            Debug.LogError("Parent or Child GameObject is not assigned!");
        }
    }

    IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(2);
        childObject.transform.localPosition = new Vector3(0.000165084377f, -0.240364492f, -1.45882368e-05f);
    }
}
