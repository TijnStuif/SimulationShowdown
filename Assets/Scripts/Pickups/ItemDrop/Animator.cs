using UnityEngine;

namespace Pickups.ItemDrop
{
    // using roll a ball here
    public class Animator : MonoBehaviour
    {
        // would be neat if you could have readonly editor assigned fields as compile time constants
        [SerializeField] private float rotationSpeed = 1f;
        // would be cool if Unity Vector3 could be a compile time constant
        private static readonly Vector3 EulerRotation = new Vector3(15, 30, 45);

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.Rotate(EulerRotation * (Time.fixedDeltaTime * rotationSpeed));
        }
    }
}
