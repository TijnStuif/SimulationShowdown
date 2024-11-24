using System;
using UnityEngine;
using System.Collections;

namespace Boss.Attack
{
    public class SceneFlip : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

        private Transform playerCamera;
        private ParticleSystem indicatorParticle;
        private bool isFlipped = false;

        private void Awake()
        {
            // find PlayerFollower (this object is under Player)
            playerCamera = FindObjectOfType<Player.Rotation>().gameObject.transform;

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

        public void Execute()
        {
            if (indicatorParticle != null)
            {
                StartCoroutine(ActivateSceneFlip());
            }
            else
            {
                Debug.LogError("Cannot execute, particle system not found!");
            }
        }

        private IEnumerator ActivateSceneFlip()
        {
            //change the color of the particle to yellow
            var main = indicatorParticle.main;
            main.startColor = new Color(1, 1, 0, 1);
            // Play the particle system
            indicatorParticle.Play();

            // Wait for 2 seconds
            yield return new WaitForSeconds(2f);

            // Change the gravity
            if (isFlipped)
            {
                playerCamera.localRotation = Quaternion.Euler(0, 0, 0);
                playerCamera.position = new Vector3(playerCamera.position.x, playerCamera.position.y - 2, playerCamera.position.z);
            }
            else
            {
                 playerCamera.localRotation =  Quaternion.Euler(0, 0, 180);
                    playerCamera.position = new Vector3(playerCamera.position.x, playerCamera.position.y + 2, playerCamera.position.z);
            }

                isFlipped = !isFlipped;
        }
    }
}