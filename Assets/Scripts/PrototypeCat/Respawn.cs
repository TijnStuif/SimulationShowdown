using UnityEngine;

namespace PrototypeCat
{
    public class Respawn : MonoBehaviour
    {
        private const float HEIGHT = -100f;
        
        private Vector3 m_spawnPosition;

        private ThirdPersonMovement m_tpm;
        // Start is called before the first frame update
        private void Start()
        {
            m_tpm = GetComponent<ThirdPersonMovement>();
            m_spawnPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if (transform.localPosition.y > HEIGHT)
            {
                return;
            }
            // else
            Debug.Log("mama mia");
            transform.position = m_spawnPosition;
            m_tpm.ResetGravitationalForce();
        }
    }
}
