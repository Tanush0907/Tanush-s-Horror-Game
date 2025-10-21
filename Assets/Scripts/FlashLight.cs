using UnityEngine;

public class FlashLight : MonoBehaviour, IInteractable
{
    [SerializeField] private Light flash;
    [SerializeField] private float flashIntensity;
    public void Interact()
    {
        flash.intensity = flash.intensity == 0f ? flashIntensity : 0f;
    }
}
