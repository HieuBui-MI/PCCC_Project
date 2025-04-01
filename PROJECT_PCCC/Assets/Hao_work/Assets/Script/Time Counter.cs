using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float ElapsedTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ElapsedTime += Time.deltaTime;
        timerText.text = ElapsedTime.ToString();
    }
}
