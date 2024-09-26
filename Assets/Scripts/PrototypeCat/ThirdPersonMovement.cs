using UnityEngine;

namespace PrototypeCat
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        // 1f = max; 0f = min
        private const float MovementThreshold = 0.1f;
        private const float TurnSmoothTime = 0.1f;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private CharacterController controller;
        [SerializeField] private float speed = 6f;
        private Vector3 m_direction3D;
        private float m_targetAngle;
        private float m_turnVelocity;

        // not linear, but "sigmoid-like" interpolation
        // see: https://docs.unity3d.com/ScriptReference/Mathf.SmoothDampAngle.html
        //      https://gamedev.stackexchange.com/questions/116272/whats-the-difference-between-mathf-lerp-and-mathf-smoothdamp
        private float TurnAngle => 
            Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetAngle, ref m_turnVelocity, TurnSmoothTime);

        private void HandleInput()
        {
            m_direction3D.x = Input.GetAxisRaw("Horizontal");
            m_direction3D.y = 0f;
            m_direction3D.z = Input.GetAxisRaw("Vertical");
            m_direction3D.Normalize();
        }

        private void Move()
        {
            m_targetAngle =
                // Mathf.Atan2(m_direction3D.x, m_direction3D.z) * Mathf.Rad2Deg * cameraTarget.eulerAngles.y;
                Mathf.Atan2(m_direction3D.x, m_direction3D.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, TurnAngle, 0f);
            // Not going to pretend to know how this works
            Vector3 moveDirection = Quaternion.Euler(0f, m_targetAngle, 0f) * Vector3.forward;
            controller.Move(m_direction3D * (speed * Time.deltaTime));
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
            if (!(m_direction3D.magnitude >= MovementThreshold)) return;
            Move();
        }
    }
}
