using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject lightSet;
    [SerializeField] private AudioSource lightSwitchFlick;
    [SerializeField] private GameObject switchOn;
    [SerializeField] private GameObject switchOff;

    private Light[] lightComponents;
    private bool isLightOn;

    private void Start()
    {
        lightComponents = lightSet.GetComponentsInChildren<Light>();

        isLightOn = lightComponents.Length > 0 && lightComponents[0].intensity > 0f;

        SetSwitchVisuals(isLightOn);
    }

    public void Interact()
    {
        lightSwitchFlick.Play();

        isLightOn = !isLightOn;

        SetLightState(isLightOn);
        SetSwitchVisuals(isLightOn);
    }

    private void SetLightState(bool isOn)
    {
        foreach (Light lightComponent in lightComponents)
        {
            lightComponent.intensity = isOn
                ? lightComponent.GetComponent<LightIntensity>().lightIntensity
                : 0f;
        }
    }

    private void SetSwitchVisuals(bool isOn)
    {
        switchOn.SetActive(isOn);
        switchOff.SetActive(!isOn);
    }
}
