using UnityEngine;

namespace PrototypeCat.Projectile
{
    public class Controller : MonoBehaviour
    {
        // SET THESE BEFORE ENABLING
        public int damage;
        [SerializeField] private float speed;
        private Vector3 m_direction;

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (isActiveAndEnabled)
            {
                transform.position += m_direction * Time.fixedDeltaTime;
            }
        }
    }
}