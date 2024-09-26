using UnityEngine;

namespace PrototypeCat
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;

        [SerializeField] private float speed = 6f;
        private Vector3 _direction3D;
        // 1f = max; 0f = min
        private const float MovementThreshold = 0.1f;

        // Update is called once per frame
        void Update()
        {
            _direction3D.x = Input.GetAxisRaw("Horizontal");
            _direction3D.y = 0f;
            _direction3D.z = Input.GetAxisRaw("Vertical");
            _direction3D.Normalize();
            if (_direction3D.magnitude >= MovementThreshold)
                controller.Move(_direction3D * (speed * Time.deltaTime));
        }
    }
}
