using System.Collections;
using UnityEngine;

namespace Boss.Attack
{
    public class LowGravity : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

        [SerializeField] private float gravityAmount = -1.6f;
        [SerializeField] private ParticleSystem indicatorParticle;

        public void Awake()
        {
            indicatorParticle.Stop();
        }

        public void Execute()
        {
            StartCoroutine(ActivateGravityChange());
        }

        private IEnumerator ActivateGravityChange()
        {
            indicatorParticle.Play();

            yield return new WaitForSeconds(1f);

            // Change the gravity
            if (Physics.gravity.y == gravityAmount)
            {
                Physics.gravity = new Vector3(0, -7f, 0);
            }
            else
            {
                Physics.gravity = new Vector3(0, gravityAmount, 0);
            }
        }
    }
}