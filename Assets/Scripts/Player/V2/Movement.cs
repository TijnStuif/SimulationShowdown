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
        
        private const float GRAVITATIONAL_ACCELERATION_CONSTANT = 9.81f;
        private const float TURN_SMOOTH_TIME = 0.05f;
        
        /// <summary>
        /// Jump height if gravity is (0, -9.81f, 0)
        /// </summary>
        private const float m_baseJumpHeight = 1.5f;
        
        [SerializeField] private float m_speed = 10f;
        
        private CharacterController m_characterController;

        /// <summary>
        /// Stores move direction using player input.
        /// </summary>
        private Vector2 m_direction2d;
        
        /// <summary>
        /// Jump height scaled with gravity
        /// (automatically updated in OnGravityChanged)
        /// </summary>
        private float m_jumpHeight;

        private bool m_frozen;
        
        /// <summary>
        /// Stores non-normalized movement vector used for moving the player on the ground.
        /// </summary>
        private Vector3 m_groundMovement3d;
        
        private float m_gravitationalAccelerationVariable;
        
        private Vector3 m_gravitationalDirection;
        
        /// <summary>
        /// Stores non-normalized vector used for jumping
        /// </summary>
        private Vector3 m_jump3d;
        
        private Transform m_mainCameraTarget;

        private Quaternion m_movementRotation;
        
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
        
        /// <summary>
        /// Stores non-normalized vector used for vertical movement
        /// </summary>
        private Vector3 m_verticalMovement3d;

        /// <summary>
        /// Stores vector for actual move direction
        /// not only accounting for input, but also the camera angle
        /// </summary>
        public Vector3 FullMoveDirection3d;
        
        public CharacterController CharacterController => m_characterController;

        /// <summary>
        /// Returns CharacterController.IsGrounded.
        /// In V1 a ground mask was used
        /// </summary>
        public bool IsGrounded => m_characterController.isGrounded;
        
        /// <summary>
        /// Property for movement vector (using both ground and vertical movement)
        /// </summary>
        private Vector3 Movement3d => m_groundMovement3d + m_verticalMovement3d;

        /// <summary>
        /// Returns smoothed out angle the player will be moving towards using
        /// Mathf.SmoothDampAngle.
        /// </summary>
        private float TurnAngle(float deltaTime) => Mathf.SmoothDampAngle(
            current: transform.eulerAngles.y,
            target: m_targetAngle,
            currentVelocity: ref m_turnVelocity,
            smoothTime: TURN_SMOOTH_TIME,
            maxSpeed: Mathf.Infinity, // for some reason setting this as default renders the entire function useless
            deltaTime: deltaTime);    // so I'm manually setting the default value


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
            
            Teleport.MashSequenceStateChange += OnMashSequenceStateChange;
        }
        
        private void OnEnable()
        {
            OnGravityChanged();
            Boss.Attack.LowGravity.GravityChanged+= OnGravityChanged;
        }

        private void OnDisable()
        {
            OnGravityChanged();
            Boss.Attack.LowGravity.GravityChanged += OnGravityChanged;
        }

        /// <summary>
        /// Calculate movement and move player at fixed time steps.
        /// If you want to use this function for a different functionality, please
        /// move this block of code to a new function including this summary,
        /// and call it from FixedUpdate.
        /// </summary>
        private void FixedUpdate()
        {
            CalculateMovement(Time.fixedDeltaTime);
            RotateUsingMovedirection();
            ApplyGravity(Time.fixedDeltaTime);
            // ApplyDrag(); 
            // if (m_direction3d.magnitude > MOVEMENT_THRESHOLD)
            Move(Time.fixedDeltaTime);
        }

        /// <summary>
        /// Update jump vector based on input unless grounded using IsGrounded boolean.
        /// </summary>
        /// <param name="context"></param>
        private void OnJump(InputAction.CallbackContext context)
        {
            if (!IsGrounded) return;
            if (!context.performed) return;
            // torricelli's law: https://en.wikipedia.org/wiki/Torricelli%27s_law
            // v = sqrt(2gh)
            // g = gravitational acceleration constant
            //     (which is variable in our game, so Physics.gravity.magnitude is used)
            // h = jump height
            //     (this gets scaled with the gravitational acceleration in OnGravityChanged)
            m_verticalMovement3d -= m_gravitationalDirection * Mathf.Sqrt(2.0f * m_gravitationalAccelerationVariable * m_jumpHeight);
            // since it's a Vector and gravity is variable I multiplied the speed scalar with
            // the direction of the gravity (which you can get by normalizing the gravity vector)
        }

        private void OnMashSequenceStateChange(Teleport.MashState state)
        {
            switch (state)
            {
                case Teleport.MashState.Start:
                    FreezeController();
                    break;
                case Teleport.MashState.End:
                    ThawController();
                    break;
            }
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

        /// <summary>
        /// Update gravitational values when gravity is changed
        /// </summary>
        private void OnGravityChanged() => UpdateGravity();

        
        // might be for a later sprint
        private void ApplyDrag() => throw new NotImplementedException();

        public void FreezeController()
        {
            m_frozen = true;
            m_characterController.enabled = false;
        }

        public void ThawController()
        {
            m_frozen = false;
            m_characterController.enabled = true;
        }

        private void ApplyGravity(float deltaTime)
        {
            if (IsGrounded && m_verticalMovement3d.y < 0)
                m_verticalMovement3d.y = 0f;
            m_verticalMovement3d += Physics.gravity * deltaTime;
        }

        /// <summary>
        /// Calculate movement based on input direction, camera angle, and speed.
        /// </summary>
        private void CalculateMovement(float deltaTime)
        {
            if (m_characterController.isGrounded && m_verticalMovement3d.y < 0)
                m_verticalMovement3d.y = 0;
                
            if (m_direction2d == Vector2.zero)
            {
                m_groundMovement3d = FullMoveDirection3d = Vector3.zero;
                return;
            }
            // get angle based on input direction
            m_targetAngle = Mathf.Atan2(m_direction2d.x, m_direction2d.y) * Mathf.Rad2Deg
                            + m_mainCameraTarget.eulerAngles.y;
            m_movementRotation = Quaternion.Euler(0, TurnAngle(deltaTime), 0);
            m_groundMovement3d = m_movementRotation * Vector3.forward;
            m_groundMovement3d.Normalize();
            FullMoveDirection3d = m_groundMovement3d;
            m_groundMovement3d *= m_speed;
        }


        private void Move(float deltaTime)
        {
            m_characterController.Move(Movement3d * deltaTime);
        }
        
        /// <summary>
        /// Sets rotation based on move direction
        /// </summary>
        private void RotateUsingMovedirection()
        {
            if (m_frozen == true)
                return;
            // goal:
            // use direction to get angle in degrees
            // create euler quaternion to set y rotation 
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x, 
                m_movementRotation.eulerAngles.y, 
                transform.rotation.eulerAngles.z);
        }
        
        private void UpdateGravity()
        {
            m_gravitationalAccelerationVariable = Physics.gravity.magnitude;
            m_gravitationalDirection = Physics.gravity.normalized;
            
            // scale jump height with gravity changes
            // if this isn't done then every jump is the same height, no matter how high or low the gravity is
            // which sorta makes sense, but not really what we want
            m_jumpHeight = Mathf.Approximately(m_gravitationalAccelerationVariable, GRAVITATIONAL_ACCELERATION_CONSTANT)
            ? m_baseJumpHeight
            : GRAVITATIONAL_ACCELERATION_CONSTANT / m_gravitationalAccelerationVariable * m_baseJumpHeight;
        }

        // private void ResetGravityAcceleration() => m_gravity3d = m_baseGravity3d;

    }
}
