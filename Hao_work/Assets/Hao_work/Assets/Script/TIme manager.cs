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
        // Initialize the day variable to start at 14:00
        day = 14f / 24f;
    }

    // Update is called once per frame
    void Update()
    {
        float hoursPerDay = 24f;

        day += Time.deltaTime / REAL_SECONDS_PER_INGAME_DAY;

        float dayNormalized = day % 1f;
        string hoursString = Mathf.Floor(dayNormalized * hoursPerDay).ToString("00");

        float minutesPerHour = 60f;
        string minutesString = Mathf.Floor(((dayNormalized * hoursPerDay) % 1f) * minutesPerHour).ToString("00");

        timeText.text = hoursString + ":" + minutesString;
    }
}