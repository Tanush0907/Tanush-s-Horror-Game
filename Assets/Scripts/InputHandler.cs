using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private GameInput controls;

    // Movement & Look
    public static event Action<Vector2> OnMove;
    public static event Action<Vector2> OnLook;

    // Interaction events
    public static event Action OnInteractPressed;
    public static event Action OnPickUpPressed;
    public static event Action OnThrowPressed;
    public static event Action OnHoldStarted;
    public static event Action OnHoldCanceled;

    private void Awake()
    {
        controls = new GameInput();
    }

    private void OnEnable()
    {
        controls.Enable();

        // Continuous input
        controls.Player.Movement.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        controls.Player.Movement.canceled += ctx => OnMove?.Invoke(Vector2.zero);

        controls.Player.Look.performed += ctx => OnLook?.Invoke(ctx.ReadValue<Vector2>());
        controls.Player.Look.canceled += ctx => OnLook?.Invoke(Vector2.zero);

        // Button presses
        controls.Player.Interact.performed += ctx => OnInteractPressed?.Invoke();
        controls.Player.PickUp.performed += ctx => OnPickUpPressed?.Invoke();
        controls.Player.Throw.performed += ctx => OnThrowPressed?.Invoke();

        // Hold Interaction
        controls.Player.HoldInteract.started += ctx => OnHoldStarted?.Invoke();
        controls.Player.HoldInteract.canceled += ctx => OnHoldCanceled?.Invoke();


    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
