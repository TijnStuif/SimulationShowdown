using System;
using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.Serialization;

namespace Boss.Attack
{
    public class SceneFlip : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;
        /// <summary>
        /// Amount of time before flipping (since indicator)
        /// </summary>
        [SerializeField] private float m_indicatorTime = 2f;
        /// <summary>
        /// Amount of time the screen flip lasts
        /// </summary>
        [SerializeField] private float m_flippedTime = 3f;

        private static AudioManager AudioManager => AudioManager.Instance;
        private CinemachineFreeLook m_cinemachineFreeLook;
        private ParticleSystem m_playerParticleSystem;

        private void Awake()
        {
            m_playerParticleSystem = FindObjectOfType<Player.V2.Controller>()
                .gameObject
                .GetComponentInChildren<ParticleSystem>();
            if (m_playerParticleSystem == null)
                throw new NullReferenceException("ERROR: indicator particle system not found");
            
            m_cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
            if (m_cinemachineFreeLook == null)
                throw new NullReferenceException("ERROR: CM FreeLook component not found");

            if (AudioManager == null)
                throw new NullReferenceException("ERROR: AudioManager not found");
        }

        // For some reason you can only set scripts as active/inactive
        // if this method is declared
        // even if it has an empty function body
        private void Start()
        {
        }

        public void Execute()
        {
            AudioManager.PlaySFX(AudioManager.bossSceneFlipSFX);
            StartCoroutine(ActivateSceneFlip());
        }

        private IEnumerator ActivateSceneFlip()
        {
            // Change the color of the particle to yellow
            var main = m_playerParticleSystem.main;
            main.startColor = new Color(1, 1, 0, 1);
            // Play the particle system
            m_playerParticleSystem.Play();
            // Wait
            yield return new WaitForSeconds(m_indicatorTime);
            
            // ggs
            // Flip camera 180 degrees
            m_cinemachineFreeLook.m_Lens.Dutch = 180f;
            // Wait
            yield return new WaitForSeconds(m_flippedTime);
            
            // Play particles again
            main.startColor = new Color(1, 1, 0, 1);
            m_playerParticleSystem.Play();
            // Wait
            yield return new WaitForSeconds(m_indicatorTime);
            
            // Return camera to regular rotation
            m_cinemachineFreeLook.m_Lens.Dutch = 0f;
        }
    }
}