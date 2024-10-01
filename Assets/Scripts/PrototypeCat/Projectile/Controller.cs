using UnityEngine;

namespace PrototypeCat.Projectile
{
    public class Controller : MonoBehaviour
    {
        // SET THESE BEFORE ENABLING
        public int damage;
        private const float SPEED = 100f;
        public Quaternion rotation;
        public Rigidbody rb;

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
            Debug.Log("moving!");
           rb.AddForce(transform.rotation * Vector3.forward * SPEED, ForceMode.Impulse); 
        }
    }
}