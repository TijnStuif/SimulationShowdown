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
    /// After the value is used, it immediately gets reset.
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
            var cam = Camera.main;
            m_characterController = GetComponentInChildren<CharacterController>();
            if (cam != null)
                m_mainCamera = cam.transform;
            else
                throw new InvalidOperationException("ERROR: couldn't find main camera");
        }

        /// <summary>
        /// calculate movement and move player at fixed time steps.
        /// if you want to use this function for a different functionality, please
        /// move this block of code to a new function including this summary
        /// and call it from FixedUpdate
        /// </summary>
        private void FixedUpdate()
        {
            if (m_direction3d.magnitude > MOVEMENT_THRESHOLD)
                CalculateMovement();
            if (m_jump3d.magnitude > MOVEMENT_THRESHOLD)
                ApplyJump();
            // ApplyDrag(); 
            // ApplyGravity();
            Move(Time.fixedDeltaTime);
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
        /// <param name="value"></param>
        private void OnMove(InputValue value)
        {
            m_direction2d = value.Get<Vector2>();
            if (AreControlsInverted)
                m_direction2d.y *= -1;
            m_direction3d.Set(m_direction2d.x, 0, m_direction2d.y);
            m_direction2d = Vector2.zero;
        }

        private void OnGravityChanged(Vector3 newGravity)
        {
            m_baseGravity3d = newGravity;
        }
        
        // might be for the next sprint
        private void ApplyDrag() => throw new NotImplementedException();
        
        private void ApplyGravity() => throw new NotImplementedException();

        private void ApplyJump()
        {
            m_jump3d = Vector3.zero;
            throw new NotImplementedException();
        }

        /// <summary>
        /// calculate movement based on input direction, camera angle, and speed
        /// set rotation based on camera angle
        /// </summary>
        private void CalculateMovement()
        {
            m_movement3d = m_direction3d.normalized;
            m_direction3d = Vector2.zero;
        }

        private void Move(float deltaTime)
        {
            m_characterController.Move(m_movement3d * deltaTime);
            m_movement3d = Vector3.zero;
        }

        private void ResetGravityAcceleration() => m_gravity3d = m_baseGravity3d;

    }
}
