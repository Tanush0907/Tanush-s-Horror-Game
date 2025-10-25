using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Transform playerPickedUpObjectSlot;
    private float holdTimer;
    private bool isHolding = false;

    private Input playerInput;
    [SerializeField] private Player player;
    private void Awake()
    {
        playerInput = new Input();
    }
    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Interaction.performed += player.TriggerInteract;
        playerInput.Player.PickUp.performed += player.TriggerPickUp;
        playerInput.Player.Throw.performed += player.TriggerThrow;
        playerInput.Player.InteractWithPickedUpObject.performed += player.TriggerInteractionWithPickedUpObject;
        playerInput.Player.HoldInteract.started += OnStarted;
        playerInput.Player.HoldInteract.canceled += OnCanceled;




    }
    private void OnStarted(InputAction.CallbackContext context)
    {
        isHolding = true;
        holdTimer = 0f;
    }
    private void OnCanceled(InputAction.CallbackContext context)
    {
        isHolding = false;
        holdTimer = 0f;
    }
    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.Interaction.performed -= player.TriggerInteract;
        playerInput.Player.PickUp.performed -= player.TriggerPickUp;
        playerInput.Player.Throw.performed -= player.TriggerThrow;
        playerInput.Player.InteractWithPickedUpObject.performed -= player.TriggerInteractionWithPickedUpObject;

    }
    public Vector2 GetMovementVectorNormalized()
    {
        return playerInput.Player.Movement.ReadValue<Vector2>().normalized;
    }
    public Vector2 GetMouseVector()
    {
        return playerInput.Player.LookAround.ReadValue<Vector2>();
    }
    private void Update()
    {
        if (!isHolding) return;

        if (playerPickedUpObjectSlot.childCount == 0)
        {
            isHolding = false;
            return;
            Debug.Log("hi");
        }

        Transform heldObject = playerPickedUpObjectSlot.GetChild(0);

        if (heldObject.TryGetComponent<HoldInteractable>(out HoldInteractable holdInteractable))
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdInteractable.holdTimeRequired)
            {
                if (heldObject.TryGetComponent<IHoldInteractable>(out IHoldInteractable interactable))
                {
                    interactable.HoldInteract();
                }

                isHolding = false;
                holdTimer = 0f;
            }
        }
    }

}
