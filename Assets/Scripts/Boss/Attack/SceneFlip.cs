using System;
using UnityEngine;
using System.Collections;
using Player.V1;

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
            var rotationScript = FindObjectOfType<Rotation>();
            if (rotationScript == null)
                throw new StateController.ScriptNotFoundException(nameof(rotationScript));
            playerCamera = rotationScript.gameObject.transform;

            var playerScript = FindObjectOfType<Player.V1.Controller>();
            if (playerScript == null)
                throw new StateController.ScriptNotFoundException(nameof(playerScript));
            var player = playerScript.gameObject;
            // Find the ParticleSystem component on the player GameObject
            indicatorParticle = player.GetComponentInChildren<ParticleSystem>();

            // Disable Play On Awake
            var main = indicatorParticle.main;
            main.playOnAwake = false;

            // Stop the particle system
            indicatorParticle.Stop();
        }

        public void Execute()
        {
            if (indicatorParticle != null)
            {
                StartCoroutine(ActivateSceneFlip());
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