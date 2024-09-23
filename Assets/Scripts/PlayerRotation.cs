using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform playerRotator;
    private PlayerBehaviour player;
    private Vector2 rotationInput;
    private float horizontalRotation;
    //private float verticalRotation;
    private float rotationSpeed = 5;

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
        rotationInput = context.ReadValue<Vector2>();
        horizontalRotation = (rotationInput.x) * rotationSpeed;
        //verticalRotation = (rotationInput.y) * -rotationSpeed;
        //player.RotationBasedInputs();
        }
        
    }

    void Update()
    {
        playerRotator.Rotate(0, horizontalRotation * Time.deltaTime, 0);
    }
}