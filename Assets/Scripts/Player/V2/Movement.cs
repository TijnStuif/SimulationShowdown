using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.V2
{
    /// <summary>
    /// Handles player movement using CharacterController.
    /// Unity input events update certain Vector3 values.
    /// In FixedUpdate it calculates the movement Vector3 based on those values.
    /// (among other things like the camera angle)
    /// After every movement calculated, CharacterController.Move is called once.
    /// </summary>
    public class Movement : MonoBehaviour
    {
        public bool AreControlsInverted;

        private const float INITIAL_Y_GRAVITY = 9.81f;
        
        private const float MOVEMENT_THRESHOLD = 0.1f;
        
        private const float TURN_SMOOTH_TIME = 0.1f;
        
        /// <summary>
        /// When gravity is used and player is not accelerating, use this value:
        /// (0, 9.81f, 0)
        /// </summary>
        private Vector3 m_baseGravity3d = new(0, INITIAL_Y_GRAVITY, 0);

        /// <summary>
        /// actual gravity applied on player accounting for gravity acceleration
        /// </summary>
        private Vector3 m_gravity3d = new(0, INITIAL_Y_GRAVITY, 0);

        /// <summary>
        /// stores move direction using player input
        /// </summary>
        private Vector2 m_direction2d;
        
        /// <summary>
        /// stores move direction using player input
        /// </summary>
        private Vector3 m_direction3d;
        
        /// <summary>
        /// stores non-normalized movement vector used for moving the player on the ground
        /// </summary>
        private Vector3 m_movement3d;
        
        /// <summary>
        /// stores non-normalized vector used for jumping
        /// </summary>
        private Vector3 m_jump3d;
        
        // private LayerMask m_groundMask;
        
        private Transform m_mainCamera;
        
        private CharacterController m_characterController;
        
        /// <summary>
        /// direction the player should actually move to in degrees
        /// affected by camera angle and the SmoothDampAngle function
        /// </summary>
        private float m_targetAngle;

        /// <summary>
        /// field for storing the velocity of the Mathf.SmoothDampAngle function
        /// DO NOT MODIFY MANUALLY
        /// should only be used with the ref keyword in the SmoothDampAngle function
        /// </summary>
        private float m_turnVelocity;
        
        private float m_speed = 20f;
        private float m_drag = 0.2f;
        private float m_jump = 10f;

        // in V1 a ground mask was used
        public bool IsGrounded => m_characterController.isGrounded;
        
        /// <summary>
        /// returns smoothed out angle the player will be moving towards using
        /// Mathf.SmoothDampAngle
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
                m_mainCamera = cam.transform;
        }

        /// <summary>
        /// calculate movement and move player at fixed time steps.
        /// if you want to use this function for a different functionality, please
        /// move this block of code to a new function including this summary
        /// and call it from FixedUpdate
        /// </summary>
        private void FixedUpdate()
        {
            CalculateMovement();

            if (m_jump3d.magnitude > MOVEMENT_THRESHOLD)
                ApplyJump();
            // ApplyDrag(); 
            // ApplyGravity();
            // if (m_direction3d.magnitude > MOVEMENT_THRESHOLD)
            Move();
        }

        /// <summary>
        /// update jump vector based on input unless grounded using IsGrounded boolean
        /// </summary>
        /// <param name="c"></param>
        private void OnJump(InputAction.CallbackContext c)
        {
            if (!IsGrounded)
                return; 
            m_jump3d.Set(0, m_jump, 0);
        }

        /// <summary>
        /// update direction vectors based on input
        /// reverts controls automatically using AreControlsInverted boolean
        /// </summary>
        /// <param name="context"></param>
        private void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_direction2d = context.ReadValue<Vector2>();
                m_direction2d.Normalize();
                if (AreControlsInverted)
                    m_direction2d.y *= -1;
                m_direction3d.Set(m_direction2d.x, 0, m_direction2d.y);
                m_direction3d.Normalize();
                m_direction2d = Vector2.zero;
            }
            else if (context.canceled)
            {
                m_direction2d = Vector2.zero;
                m_direction3d = Vector3.zero;
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
        /// calculate movement based on input direction, camera angle, and speed
        /// set rotation based on camera angle
        /// </summary>
        private void CalculateMovement()
        {
            if (m_direction3d == Vector3.zero)
            {
                m_movement3d = Vector3.zero;
                return;
            }
            m_movement3d = m_direction3d * m_speed;
        }

        private void Move()
        {
            Debug.Log(m_movement3d);
            m_characterController.SimpleMove(m_movement3d);
        }

        private void ResetGravityAcceleration() => m_gravity3d = m_baseGravity3d;

    }
}
