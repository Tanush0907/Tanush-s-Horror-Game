using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator doorAnimator;
    public void Interact()
    {
        doorAnimator.SetBool("IsDoorOpen", !doorAnimator.GetBool("IsDoorOpen"));
    }
}
