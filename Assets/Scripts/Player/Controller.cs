using System;
using System.Collections;
using Boss.Attack;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public enum State
    {
        Loss,
        Pause,
    }
        
    public class Controller : MonoBehaviour
    {
        public int maxHealth = 100;
        [HideInInspector] public int currentHealth;

        public event Action<State> StateChange;

        [SerializeField] private GameObject vfxElectricityPrefab;
        private ParticleSystem vfxElectricity;

        void Awake()
        {
            DamageAttack.PlayerDamaged += TakeDamage;
            currentHealth = maxHealth;            

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
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                StateChange?.Invoke(State.Loss);
            }
            else
            {
                StartCoroutine(PlayVFX());
            }
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Attack"))
            {
                switch (other.gameObject.name)
                {
                    case "CloseRangeAttack":
                        TakeDamage(50);
                        break;
                    case "LaserAttack":
                        TakeDamage(40);
                        break;
                }
            }
        }

        private void FixedUpdate()
        {
            // Check if the player is outside the map
            // If this is the case the player will die
            if (transform.position.x <= -15 || transform.position.z <= -15 || transform.position.x >= 15 || transform.position.z >= 15 || transform.position.y <= -3)
            {
                TakeDamage(maxHealth);
            }
        }
    }
}