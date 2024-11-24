using System;
using System.Collections;
using UnityEngine;

namespace Boss.Attack
{
    public class LowGravity : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

        [SerializeField] private float gravityAmount = -1.6f;
        private ParticleSystem indicatorParticle;

        public void Awake()
        {
            // Find the player GameObject
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                // Find the ParticleSystem component on the player GameObject
                indicatorParticle = player.GetComponentInChildren<ParticleSystem>();
                Debug.Assert(indicatorParticle, "Particle system component not found!");

                // Disable Play On Awake
                var main = indicatorParticle.main;
                main.playOnAwake = false;

                // Stop the particle system
                indicatorParticle.Stop();
            }
            else
            {
                Debug.LogError("Player GameObject not found!");
            }
        }

        private void OnDisable()
        {
            Physics.gravity = new Vector3(0,-9.81f, 0);
        }

        public void Execute()
        {
            if (indicatorParticle != null)
            {
                StartCoroutine(ActivateGravityChange());
            }
            else
            {
                Debug.LogError("Cannot execute, particle system not found!");
            }
        }

        private IEnumerator ActivateGravityChange()
        {
            //set particle to purple
            var main = indicatorParticle.main;
            main.startColor = new Color(1, 0, 1, 1);

            // Play the particle system
            indicatorParticle.Play();

            // Wait for 2 seconds
            yield return new WaitForSeconds(2f);

            // Change the gravity
            if (Physics.gravity.y == gravityAmount)
            {
                Physics.gravity = new Vector3(0, -9.81f, 0);
            }
            else
            {
                Physics.gravity = new Vector3(0, gravityAmount, 0);
            }
        }
    }
}