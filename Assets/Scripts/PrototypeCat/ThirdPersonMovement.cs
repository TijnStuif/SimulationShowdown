using UnityEngine;

namespace PrototypeCat
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        // 1f = max; 0f = min
        private const float MOVEMENT_THRESHOLD = 0.1f;
        private const float TURN_SMOOTH_TIME = 0.1f;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private CharacterController controller;
        [SerializeField] private float speed = 6f;
        private Vector3 m_direction3D;
        private float m_targetAngle;
        private float m_turnVelocity;

        // the turn angle is calculated with sigmoid-like interpolation
        // see: https://docs.unity3d.com/ScriptReference/Mathf.SmoothDampAngle.html
        //      https://gamedev.stackexchange.com/questions/116272/whats-the-difference-between-mathf-lerp-and-mathf-smoothdamp
        private float TurnAngle => 
            Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetAngle, ref m_turnVelocity, TURN_SMOOTH_TIME);

        private void HandleInput()
        {
            // use Input values to get initial move direction
            m_direction3D.x = Input.GetAxisRaw("Horizontal");
            m_direction3D.y = 0f;
            m_direction3D.z = Input.GetAxisRaw("Vertical");
            m_direction3D.Normalize();
        }

        private void Move(float deltaTime)
        {
            // get angle using 2 argument arctangent function
            //    you input y and z, and it outputs an angle in radians
            // angle is converted from radians to degrees
            // camera y (euler) rotation value is added to the angle so the target angle is based on the camera view
            m_targetAngle =
                Mathf.Atan2(m_direction3D.x, m_direction3D.z) * Mathf.Rad2Deg + cameraTarget.eulerAngles.y;
            // move direction is a forward 3-dimensional vector, rotated using a quaternion consisting of only euler rotations
            Vector3 moveDirection = Quaternion.Euler(0f, m_targetAngle, 0f) * Vector3.forward;
            // set rotation to a quaternion consisting of only euler rotations
            transform.rotation = Quaternion.Euler(0f, TurnAngle, 0f);
            // move player using the move direction, speed, and between frame interval
            controller.Move(moveDirection.normalized * (speed * deltaTime));
        }
        
        // physics like movement are done in FixedUpdate
        private void FixedUpdate()
        {
            if (m_direction3D.magnitude >= MOVEMENT_THRESHOLD) 
                Move(Time.fixedDeltaTime);
        }

        // input handling is done every frame in Update
        void Update()
        {
            HandleInput();
        }
    }
}
