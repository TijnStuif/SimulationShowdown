using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform playerRotator;
    [SerializeField] private PlayerBehaviour player;
    private Vector2 rotationInput;
    private float horizontalRotation;
    private float rotationSpeed = 15;

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
        rotationInput = context.ReadValue<Vector2>();
        horizontalRotation = rotationInput.x * rotationSpeed;
        } else if (context.canceled)
        {
            rotationInput = Vector2.zero;
            horizontalRotation = 0;
        }
    }

    void Update()
    {
        playerRotator.Rotate(0, horizontalRotation * Time.deltaTime, 0);
    }
}