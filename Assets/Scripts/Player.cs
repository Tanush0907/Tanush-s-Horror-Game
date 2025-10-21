using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("Player Mechanics")]
    [SerializeField] private CharacterController playerCharacterController;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float rayDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector3 moveDir;
    private float xRotation = 0;
    public Transform playerPickedUpObjectSlot;


    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        PlayerMove();
        PlayerLookAround();
    }

    private void PlayerMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f;
        }

        moveDir = transform.forward * inputHandler.GetMovementVectorNormalized().y +
                  transform.right * inputHandler.GetMovementVectorNormalized().x;

        playerCharacterController.Move(moveDir * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        playerCharacterController.Move(velocity * Time.deltaTime);
    }

    private void PlayerLookAround()
    {
        float mouseX = inputHandler.GetMouseVectorNormalized().x * mouseSensitivity * Time.deltaTime;
        float mouseY = inputHandler.GetMouseVectorNormalized().y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void TriggerInteract(InputAction.CallbackContext context)
    {
        int interactableMask = LayerMask.GetMask("Interactable");

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, rayDistance, interactableMask))
        {
            if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }

    public void TriggerPickUp(InputAction.CallbackContext context)
    {
        int pickableMask = LayerMask.GetMask("Pickable");
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, pickableMask))
        {
            if (hit.collider.gameObject.TryGetComponent<Pickable>(out Pickable pickable))
            {
                pickable.HandlePickUp();
            }
        }
    }

    public void TriggerThrow(InputAction.CallbackContext context)
    {
        if (playerPickedUpObjectSlot.GetChild(0).TryGetComponent<Pickable>(out Pickable pickable))
        {
            pickable.ExecuteThrow(pickable.transform);
        }
    }
    public void TriggerInteractionWithPickedUpObject(InputAction.CallbackContext context)
    {
        if (playerPickedUpObjectSlot.GetChild(0).TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactable.Interact();
        }
    }

}
