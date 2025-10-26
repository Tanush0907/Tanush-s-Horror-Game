using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [Header("Light Settings")]
    [SerializeField] private GameObject lightSet;
    [SerializeField] private GameObject switchOn;
    [SerializeField] private GameObject switchOff;

    [Header("Audio")]
    [SerializeField] private AudioSource lightSwitchFlick;

    private Light[] lights;
    private bool isLightOn;

    private void Start()
    {
        // Cache all child lights
        lights = lightSet.GetComponentsInChildren<Light>();

        // Initialize state based on first light's intensity
        isLightOn = lights.Length > 0 && lights[0].intensity > 0f;

        UpdateSwitchVisuals();
    }

    // ----------------------------
    // IInteractable implementation
    // ----------------------------
    public void Interact()
    {
        // Play sound if assigned
        if (lightSwitchFlick != null) lightSwitchFlick.Play();

        // Toggle state
        isLightOn = !isLightOn;

        UpdateLightState();
        UpdateSwitchVisuals();
    }

    // ----------------------------
    // Private helpers
    // ----------------------------
    private void UpdateLightState()
    {
        foreach (Light light in lights)
        {
            float targetIntensity = isLightOn ? GetLightIntensity(light) : 0f;
            light.intensity = targetIntensity;
        }
    }

    private float GetLightIntensity(Light light)
    {
        LightIntensity li = light.GetComponent<LightIntensity>();
        return li != null ? li.lightIntensity : 1f; // default to 1 if missing
    }

    private void UpdateSwitchVisuals()
    {
        if (switchOn != null) switchOn.SetActive(isLightOn);
        if (switchOff != null) switchOff.SetActive(!isLightOn);
    }
}
