using UnityEngine;
using UnityEngine.Events;

public class HoldInteractable : MonoBehaviour, IHoldInteractable
{
    [Header("Hold Settings")]
    [Tooltip("Time in seconds the player must hold to trigger interaction")]
    [SerializeField] private float holdTimeRequired = 2f;

    [Header("Optional Feedback")]
    public GameObject visualFeedback;

    [Header("Events")]
    public UnityEvent onHoldComplete;

    private bool hasInteracted = false;

    public float HoldTime => holdTimeRequired;

    public void HoldInteract()
    {
        if (hasInteracted) return;

        hasInteracted = true;
        Debug.Log($"{gameObject.name} hold completed after {holdTimeRequired} seconds!");

        // Trigger all events assigned in the inspector
        onHoldComplete?.Invoke();

        // Optional: disable feedback
        if (visualFeedback != null)
            visualFeedback.SetActive(false);
    }

    public void ResetHold()
    {
        hasInteracted = false;
        if (visualFeedback != null)
            visualFeedback.SetActive(true);
    }
}
