using UnityEngine;
using UnityEngine.InputSystem;

public class Food : MonoBehaviour, IHoldInteractable
{
    public void HoldInteract()
    {
        Debug.Log("Eating Food");
    }
}
