using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.V2
{
    /// <summary>
    /// (WIP)
    /// Handles player movement using CharacterController.
    /// Unity input events update certain Vector3 values.
    /// In FixedUpdate it calculates the movement Vector3 based on those values
    /// (among other things like the camera angle).
    /// Player also gets rotated based on the movement direction.
    /// After the movement vector calculated, CharacterController.Move is called once using it.
    /// </summary>
    public class Movement : MonoBehaviour
    {
        public bool AreControlsInverted;

        private const float INITIAL_Y_GRAVITY = 9.81f;
        
        private const float MOVEMENT_THRESHOLD = 0.1f;
        
        private const float TURN_SMOOTH_TIME = 0.1f;
        
        /// <summary>
        /// When gravity is used and player is not accelerating, use this value:
        /// (0, 9.81f, 0).
        /// </summary>
        private Vector3 m_baseGravity3d = new(0, INITIAL_Y_GRAVITY, 0);

        /// <summary>
        /// Actual gravity applied on player accounting for gravity acceleration.
        /// </summary>
        private Vector3 m_gravity3d = new(0, INITIAL_Y_GRAVITY, 0);

        /// <summary>
        /// Stores move direction using player input.
        /// </summary>
        private Vector2 m_direction2d;
        
        /// <summary>
        /// Stores non-normalized movement vector used for moving the player on the ground.
        /// </summary>
        private Vector3 m_movement3d;

        private Quaternion m_movementRotation;
        
        /// <summary>
        /// Stores non-normalized vector used for jumping
        /// </summary>
        private Vector3 m_jump3d;
        
        // private LayerMask m_groundMask;
        
        private Transform m_mainCameraTarget;
        
        private CharacterController m_characterController;
        
        /// <summary>
        /// Direction the player should actually move to in degrees.
        /// Affected by camera angle and the SmoothDampAngle function
        /// </summary>
        private float m_targetAngle;

        /// <summary>
        /// Field for storing the velocity of the Mathf.SmoothDampAngle function.
        /// DO NOT MODIFY MANUALLY.
        /// Should only be used with the ref keyword in the SmoothDampAngle function.
        /// </summary>
        private float m_turnVelocity;
        
        [SerializeField] private float m_speed = 10f;
        // private float m_drag = 0.2f;
        private float m_jump = 10f;

        // in V1 a ground mask was used
        public bool IsGrounded => m_characterController.isGrounded;
        
        /// <summary>
        /// Returns smoothed out angle the player will be moving towards using
        /// Mathf.SmoothDampAngle.
        /// </summary>
        private float TurnAngle => Mathf.SmoothDampAngle(
            current: transform.eulerAngles.y, 
            target: m_targetAngle, 
            currentVelocity: ref m_turnVelocity, 
            smoothTime: TURN_SMOOTH_TIME);

        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            var cam = Camera.main;
            m_characterController = GetComponentInChildren<CharacterController>();
            if (cam == null)
                throw new InvalidOperationException("ERROR: couldn't find main camera");
            else
                m_mainCameraTarget = cam.transform;
        }

        /// <summary>
        /// Calculate movement and move player at fixed time steps.
        /// If you want to use this function for a different functionality, please
        /// move this block of code to a new function including this summary,
        /// and call it from FixedUpdate.
        /// </summary>
        private void FixedUpdate()
        {
            CalculateMovement();
            RotateUsingMovedirection();
            if (m_jump3d.magnitude > MOVEMENT_THRESHOLD)
                ApplyJump();
            // ApplyDrag(); 
            // ApplyGravity();
            // if (m_direction3d.magnitude > MOVEMENT_THRESHOLD)
            Move(Time.fixedDeltaTime);
        }

        /// <summary>
        /// Update jump vector based on input unless grounded using IsGrounded boolean.
        /// </summary>
        /// <param name="c">unused</param>
        private void OnJump(InputAction.CallbackContext c)
        {
            if (!IsGrounded)
                return; 
            m_jump3d.Set(0, m_jump, 0);
        }

        /// <summary>
        /// Update direction vectors based on input.
        /// Reverts controls automatically using AreControlsInverted boolean.
        /// </summary>
        /// <param name="context">Vector2 value is read</param>
        private void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_direction2d = context.ReadValue<Vector2>();
                m_direction2d.Normalize();
                if (AreControlsInverted)
                    m_direction2d *= -1;
            }
            else if (context.canceled)
            {
                m_direction2d = Vector2.zero;
            }
        }

        public void OnGravityChanged(Vector3 newGravity)
        {
            m_baseGravity3d = newGravity;
        }
        
        // might be for the next sprint
        private void ApplyDrag() => throw new NotImplementedException();
        
        private void ApplyGravity() => throw new NotImplementedException();

        private void ApplyJump()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculate movement based on input direction, camera angle, and speed.
        /// </summary>
        private void CalculateMovement()
        {
            if (m_direction2d == Vector2.zero)
            {
                m_movement3d = Vector3.zero;
                return;
            }
            // get angle based on input direction
            m_targetAngle = Mathf.Atan2(m_direction2d.x, m_direction2d.y) * Mathf.Rad2Deg
                            + m_mainCameraTarget.eulerAngles.y;
            m_movementRotation = Quaternion.Euler(0, TurnAngle, 0);
            m_movement3d = m_movementRotation * Vector3.forward;
            m_movement3d.Normalize();
            m_movement3d *= m_speed;
        }

        /// <summary>
        /// Sets rotation based on move direction
        /// </summary>
        private void RotateUsingMovedirection()
        {
            // goal:
            // use direction to get angle in degrees
            // create euler quaternion to set y rotation 
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x, 
                m_movementRotation.eulerAngles.y, 
                transform.rotation.eulerAngles.z);
        }

        private void Move(float deltaTime)
        {
            #if DEBUG
            Debug.Log(m_movement3d);
            #endif
            m_characterController.SimpleMove(m_movement3d);
        }

        private void ResetGravityAcceleration() => m_gravity3d = m_baseGravity3d;

    }
}
