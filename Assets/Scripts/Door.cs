using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Animator Settings")]
    public Animator doorAnimator;
    public string boolName = "isDoorOpen";

    private bool isOpen = false;

    public void Interact()
    {
        if (doorAnimator == null) return;

        isOpen = !isOpen; // toggle state
        doorAnimator.SetBool(boolName, isOpen);
    }
}
