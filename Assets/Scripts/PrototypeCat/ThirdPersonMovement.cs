using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PrototypeCat
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        // 1f = max; 0f = min
        private const float INITIAL_GRAVITY = 9.81f;
        private const float MOVEMENT_THRESHOLD = 0.1f;
        private const float TURN_SMOOTH_TIME = 0.1f;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private CharacterController controller;
        [SerializeField] private float speed = 20f;
        [SerializeField] private float jumpMotionScalar = 10f;
        private Vector2 m_direction2d = Vector2.zero;
        private Vector3 m_jump3d;
        private float m_gravity = INITIAL_GRAVITY;
        private Vector3 m_moveDirection3d;
        private float m_targetAngle;
        private float m_turnVelocity;

        // the turn angle is calculated with sigmoid-like interpolation
        // see: https://docs.unity3d.com/ScriptReference/Mathf.SmoothDampAngle.html
        //      https://gamedev.stackexchange.com/questions/116272/whats-the-difference-between-mathf-lerp-and-mathf-smoothdamp
        private float TurnAngle => 
            Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetAngle, ref m_turnVelocity, TURN_SMOOTH_TIME);

        private void Awake()
        {
            m_jump3d = new(0f, jumpMotionScalar, 0f);
        }

        private void ApplyGravity(float deltaTime)
        {
            if (controller.isGrounded)
                m_gravity = INITIAL_GRAVITY;
            else
                // action occurs every 0.02/fixed time step seconds if the player is grounded
                // gravity is 9.81m/s/s
                // so we should add 9.81 every second by adding the delta time
                m_gravity += INITIAL_GRAVITY * deltaTime;
            controller.Move(Vector3.down * (m_gravity * deltaTime));
        }

        private void Move(float deltaTime)
        {
            // if you're not actually moving / if there's no movement input, don't do anything
            if (m_direction2d.magnitude < MOVEMENT_THRESHOLD) return;
            // get angle using 2 argument arctangent function
            //    you input y and z, and it outputs an angle in radians
            //    y and x are swapped since Unity's "unit circle" degrees turn clockwise instead of counterclockwise
            // angle is converted from radians to degrees
            // camera y (euler) rotation value is added to the angle so the target angle is based on the camera view
            m_targetAngle =
                Mathf.Atan2(m_direction2d.x, m_direction2d.y) * Mathf.Rad2Deg + cameraTarget.eulerAngles.y;
            // set rotation to a quaternion consisting of only euler rotations
            transform.rotation = Quaternion.Euler(0f, TurnAngle, 0f);
            // move direction is a forward 3-dimensional vector, rotated using a quaternion consisting of only euler rotations
            m_moveDirection3d = Quaternion.Euler(0f, m_targetAngle, 0f) * Vector3.forward;
            m_moveDirection3d.y = 0f;
            // move player using the move direction, speed, and between frame interval
            controller.Move(m_moveDirection3d.normalized * (speed * deltaTime));
            
        }

        private void OnJump()
        {
            if (!controller.isGrounded)
            {
                return;
            }
            controller.Move(m_jump3d);
        }

        private void OnMove(InputValue inputValue)
        {
            m_direction2d = inputValue.Get<Vector2>();
        }
        
        // physics, like movement or gravity are done in FixedUpdate
        private void FixedUpdate()
        { 
            Move(Time.fixedDeltaTime);
            ApplyGravity(Time.fixedDeltaTime);
        }

        private void Update()
        {
            // check for gravity again
            // to account for between frame intervals outside FixedUpdate
            // this ensures gravity to not accelerate when grounded at all times
            if (controller.isGrounded)
                m_gravity = INITIAL_GRAVITY;
        }
    }
}
