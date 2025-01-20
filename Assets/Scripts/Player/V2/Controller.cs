using System;
using System.Collections;
using Boss.Attack;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

// copypasted from V1
namespace Player.V2
{
    public enum State
    {
        Loss,
        Pause,
    }
        
    public class Controller : MonoBehaviour
    {
        private Vignette vignette;
        private Volume volume;
        
        public int MaxHealth = 100;
        [HideInInspector] public int CurrentHealth;
        private AudioManager m_audioManager;

        public event Action<State> StateChange;

        [SerializeField] private GameObject vfxElectricityPrefab;
        private ParticleSystem vfxElectricity;
        private Vector3 startPosition;

        void Awake()
        {
            m_audioManager = FindObjectOfType<AudioManager>();
            DamageAttack.PlayerDamaged += TakeDamage;
            CurrentHealth = MaxHealth;            

            var vfxInstance = transform.Find("vfx_Electricity_01");
            if (vfxInstance != null)
            {
                vfxElectricity = vfxInstance.GetComponent<ParticleSystem>();
                vfxElectricity.Stop();
            }
            else
            {
                Debug.LogWarning("vfx_Electricity_01 not found under the player.");
            }

            volume = FindObjectOfType<Volume>();
            volume.profile.TryGet(out vignette);

            startPosition = transform.position;

        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            // I unfortunately could not get anything to work aside from this
            // Maybe AudioManager should be a singleton
            if (m_audioManager == null)
                m_audioManager = FindObjectOfType<AudioManager>();
            m_audioManager.PlaySFX(m_audioManager.playerDamagedSFX);
            if (CurrentHealth <= 0)
            {
                StateChange?.Invoke(State.Loss);
            }
            else
            {
                StartCoroutine(PlayVFX());
            }

            vignette.intensity.value += damage * 0.005f;
        }

        private IEnumerator PlayVFX()
        {
            if (vfxElectricity != null)
            {
                vfxElectricity.Play();
                yield return new WaitForSeconds(0.1f);
                vfxElectricity.Stop();
            }
        }

        public void OnPause(InputAction.CallbackContext c)
        {
            StateChange?.Invoke(State.Pause);
        }

        private void FixedUpdate()
        {
            // Check if the player is outside the map
            // If this is the case the player will die, and the player will start glitching in the ground for 1-2 seconds before ending the game
            if (transform.position.y <= -3)
            {
                TakeDamage(MaxHealth);
                transform.position += Vector3.up;
            }
        }
    }
}