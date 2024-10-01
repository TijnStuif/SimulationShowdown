using UnityEngine;

namespace PrototypeCat.Projectile
{
    public class Controller : MonoBehaviour
    {
        // SET THESE BEFORE ENABLING
        public int damage;
        private const float SPEED = 25f;
        public Quaternion rotation;
        public Rigidbody rb;
        private float m_timeStepsActive;

        private void OnDestroy()
        {
           Debug.Log("Mama mia"); 
        }

        // call when enabled
        private void Start()
        {
           Move(); 
        }
        // Update is called once per frame
        // private void FixedUpdate()
        // {
        //         transform.position += Vector3.forward * (Time.fixedDeltaTime * speed);
        // }

        private void Move()
        {
           rb.AddForce(transform.rotation * Vector3.forward * SPEED, ForceMode.Impulse); 
        }

        private void FixedUpdate()
        {
            if (m_timeStepsActive >= 5f)
            {
                Destroy(gameObject);
            }

            m_timeStepsActive+= Time.fixedDeltaTime;
        }
    }
}