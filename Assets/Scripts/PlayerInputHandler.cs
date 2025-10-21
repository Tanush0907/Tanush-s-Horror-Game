using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
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
    public Vector2 GetMouseVectorNormalized()
    {
        return playerInput.Player.LookAround.ReadValue<Vector2>().normalized;
    }
}
