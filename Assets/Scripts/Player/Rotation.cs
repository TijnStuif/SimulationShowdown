using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.V1
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField] private Transform player;
        private Vector2 rotationInput;
        private float horizontalRotation;
        private readonly float rotationSpeed = 15;


        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                rotationInput = context.ReadValue<Vector2>();
                horizontalRotation = rotationInput.x * rotationSpeed;
            } 
            else if (context.canceled)
            {
                rotationInput = Vector2.zero;
                horizontalRotation = 0;
            }
        }

        void Update()
        {
            transform.Rotate(0, horizontalRotation * Time.deltaTime, 0);
        }
    }
}