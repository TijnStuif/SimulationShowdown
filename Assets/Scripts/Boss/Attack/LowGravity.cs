using System;
using System.Collections;
using UnityEngine;

namespace Boss.Attack
{
    public class LowGravity : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

        [SerializeField] private float gravityAmount = -4f;
        private AudioManager audioManager;
        private ParticleSystem indicatorParticle;
        
        public static event Action GravityChanged;

        public void Awake()
        {
            // Find the player GameObject
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                // Find the ParticleSystem component on the player GameObject
                indicatorParticle = player.GetComponentInChildren<ParticleSystem>();

                // Disable Play On Awake
                var main = indicatorParticle.main;
                main.playOnAwake = false;

                // Stop the particle system
                indicatorParticle.Stop();
            }
            audioManager = FindObjectOfType<AudioManager>();
        }

        private void OnDisable()
        {
            Physics.gravity = new Vector3(0,-9.81f, 0);
            GravityChanged?.Invoke();
        }

        public void Execute()
        {
            if (indicatorParticle != null)
            {
                audioManager.PlaySFX(audioManager.bossLowGravitySFX);
                StartCoroutine(ActivateGravityChange());
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
            GravityChanged?.Invoke();
        }
    }
}