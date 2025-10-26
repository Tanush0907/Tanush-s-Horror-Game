using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [Header("General Settings")]
    public Transform playerCamera;
    public float interactDistance = 3f;
    public Transform pickUpSlot;

    // ----------------------------
    // Held Object (PickUp / Throw)
    // ----------------------------
    private GameObject heldObject;

    // ----------------------------
    // Hold Interact
    // ----------------------------
    private IHoldInteractable currentHoldTarget;
    private float holdTimer = 0f;
    private bool isHolding = false;

    private void OnEnable()
    {
        // Standard interactions
        InputHandler.OnInteractPressed += HandleInteract;
        InputHandler.OnPickUpPressed += HandlePickUp;
        InputHandler.OnThrowPressed += HandleThrow;

        // Hold interactions
        InputHandler.OnHoldStarted += StartHold;
        InputHandler.OnHoldCanceled += CancelHold;
    }

    private void OnDisable()
    {
        // Standard interactions
        InputHandler.OnInteractPressed -= HandleInteract;
        InputHandler.OnPickUpPressed -= HandlePickUp;
        InputHandler.OnThrowPressed -= HandleThrow;

        // Hold interactions
        InputHandler.OnHoldStarted -= StartHold;
        InputHandler.OnHoldCanceled -= CancelHold;
    }

    private void Update()
    {
        HandleHold();
    }

    // ----------------------------
    // Standard Interact
    // ----------------------------
    private void HandleInteract()
    {
        if (heldObject != null) return; // optionally restrict while holding

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }

    // ----------------------------
    // Pick Up / Drop
    // ----------------------------
    private void HandlePickUp()
    {
        if (heldObject != null) return; // already holding

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.TryGetComponent<IPickable>(out IPickable pickable))
            {
                pickable.PickUp(pickUpSlot);
                heldObject = hit.collider.gameObject;
            }
        }
        else if (heldObject != null) // drop if nothing hit
        {
            heldObject.transform.parent = null;
            heldObject = null;
        }
    }

    // ----------------------------
    // Throw
    // ----------------------------
    private void HandleThrow()
    {
        if (heldObject == null) return;

        if (heldObject.TryGetComponent<IPickable>(out IPickable pickable))
        {
            Vector3 throwDir = playerCamera.forward;
            pickable.Throw(throwDir, 10f);
        }

        heldObject = null;
    }

    // ----------------------------
    // Hold Interact
    // ----------------------------
    private void StartHold()
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.TryGetComponent<IHoldInteractable>(out IHoldInteractable holdObj))
            {
                currentHoldTarget = holdObj;
                holdTimer = 0f;
                isHolding = true;
            }
        }
    }

    private void CancelHold()
    {
        isHolding = false;
        holdTimer = 0f;
        currentHoldTarget = null;
    }

    private void HandleHold()
    {
        if (!isHolding || currentHoldTarget == null) return;

        holdTimer += Time.deltaTime;

        // Get hold time from the HoldInteractable component
        float requiredHoldTime = 2f; // default
        if (currentHoldTarget is HoldInteractable holdComponent)
        {
            requiredHoldTime = holdComponent.HoldTime;
        }

        if (holdTimer >= requiredHoldTime)
        {
            currentHoldTarget.HoldInteract();
            CancelHold();
        }
    }
}
