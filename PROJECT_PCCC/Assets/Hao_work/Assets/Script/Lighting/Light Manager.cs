using UnityEngine;

[ExecuteAlways]
public class LightManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;

    [SerializeField] private Lightingpreset Preset;

    [SerializeField, Range(0, 24)] private float TimeOfDay;

    [SerializeField] private float TimeSpeed = 1f; // Speed multiplier for time progression

    void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            // Increment TimeOfDay based on Time.deltaTime and TimeSpeed
            TimeOfDay += Time.deltaTime * TimeSpeed;
            TimeOfDay %= 24; // Ensure TimeOfDay loops between 0 and 24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }

    void Start()
    {
        // Optional: Initialize TimeOfDay or other settings if needed
    }
}