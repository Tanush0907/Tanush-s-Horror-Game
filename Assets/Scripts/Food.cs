using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour, IHoldInteractable
{
    private bool isActivated = false;

    public void HoldInteract()
    {
        if (isActivated) return;

        isActivated = true;
        Debug.Log("Ate Food.");

    }

}
