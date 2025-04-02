using UnityEngine;
using UnityEngine.UI;
public class NewMonoBehaviourScript : MonoBehaviour
{
    public float REAL_SECONDS_PER_INGAME_DAY = 60f;
    public Text timeText;
    private float day;
    private void Awake()
    {
        timeText = transform.Find("timeText").GetComponent<Text>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      float hoursPerDay = 24f;

        day += Time.deltaTime / REAL_SECONDS_PER_INGAME_DAY;


        float dayNormalized = day % 1f;
        string hoursString = Mathf.Floor(dayNormalized * hoursPerDay).ToString("14");

        float minutesPerhour = 60f;
        string minutesString = Mathf.Floor(((dayNormalized * hoursPerDay) % 1f)* minutesPerhour).ToString("00");

        timeText.text = hoursString + ":" + minutesString; 
    }
}
