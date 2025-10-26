using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public Transform cameraTransform;
    public float mouseSensitivity = 300f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    // Stored input from events
    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        InputHandler.OnMove += v => moveInput = v;
        InputHandler.OnLook += v => lookInput = v;
    }

    private void OnDisable()
    {
        InputHandler.OnMove -= v => moveInput = v;
        InputHandler.OnLook -= v => lookInput = v;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    // ----------------------------
    // Movement
    // ----------------------------
    private void HandleMovement()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0f)
            velocity.y = -0.5f;

        // Convert input to world movement
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ----------------------------
    // Camera / Look
    // ----------------------------
    private void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (camera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (player body)
        transform.Rotate(Vector3.up * mouseX);
    }
}
