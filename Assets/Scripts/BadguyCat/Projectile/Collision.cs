using UnityEngine;

namespace BadguyCat.Projectile
{
    public class Collision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }
    }
}
