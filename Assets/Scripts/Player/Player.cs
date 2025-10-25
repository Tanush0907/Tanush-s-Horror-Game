using TMPro;
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
    public Transform playerPickedUpObjectSlot;
    public TextMeshProUGUI dialogueBox;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector3 moveDir;
    private float xRotation = 0f;
    private Transform currentPerson;
    [Header("Talk Mechanics")]
    private bool isTalking = false;
    private float rotationSpeed;
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

        if (isTalking && currentPerson != null)
        {
            Vector3 direction = transform.position - currentPerson.position;
            direction.y = 0;
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                currentPerson.rotation = Quaternion.Slerp(
                    currentPerson.rotation,
                    targetRot,
                    Time.deltaTime * rotationSpeed
                );
            }
        }
    }

    private void LateUpdate()
    {
        PlayerLookAround();
    }

    private void PlayerMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -0.5f;

        Vector2 moveInput = inputHandler.GetMovementVectorNormalized();
        moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        playerCharacterController.Move(moveDir * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        playerCharacterController.Move(velocity * Time.deltaTime);
    }

    private void PlayerLookAround()
    {
        float mouseX = inputHandler.GetMouseVector().x * mouseSensitivity * Time.deltaTime;
        float mouseY = inputHandler.GetMouseVector().y * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void TriggerInteract(InputAction.CallbackContext context)
    {
        // 🔹 Interactables
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();
            }

            else if (hit.collider.gameObject.TryGetComponent<ITalkable>(out ITalkable talkable))
            {
                talkable.Talk();
                rotationSpeed = hit.transform.GetComponent<Dialogue>().rotationSpeed;
                currentPerson = hit.transform;
                isTalking = true;
            }
            else
            {
                rotationSpeed = 0;
                isTalking = false;
                currentPerson = null;
            }
        }

    }

    public void TriggerPickUp(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.gameObject.TryGetComponent<Pickable>(out Pickable pickable))
                pickable.HandlePickUp();
        }
    }

    public void TriggerThrow(InputAction.CallbackContext context)
    {
        if (playerPickedUpObjectSlot.childCount > 0 &&
            playerPickedUpObjectSlot.GetChild(0).TryGetComponent<Pickable>(out Pickable pickable))
        {
            pickable.ExecuteThrow(pickable.transform);
        }
    }

    public void TriggerInteractionWithPickedUpObject(InputAction.CallbackContext context)
    {
        if (playerPickedUpObjectSlot.childCount > 0 &&
            playerPickedUpObjectSlot.GetChild(0).TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactable.Interact();
        }
    }

}
