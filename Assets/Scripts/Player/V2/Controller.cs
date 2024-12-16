using System;
using Boss.Attack;
using UnityEngine;
using UnityEngine.InputSystem;
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
        
        public int MaxHealth = 100;
        [HideInInspector] public int CurrentHealth;
        private static AudioManager AudioManager => AudioManager.Instance;

        public event Action<State> StateChange;

        void Awake()
        {
            DamageAttack.PlayerDamaged += TakeDamage;
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            AudioManager.PlaySFX(AudioManager.playerDamagedSFX);
            if (CurrentHealth <= 0)
            {
                StateChange?.Invoke(State.Loss);
            }
        }

        public void OnPause(InputAction.CallbackContext c)
        {
            StateChange?.Invoke(State.Pause);
        }

        private void FixedUpdate()
        {
            //Check if the player is outside the map
            //If this is the case the player will die
            if (transform.position.x <= -15 || transform.position.z <= -15 || transform.position.x >= 15 || transform.position.z >= 15 || transform.position.y <= -3)
            {
                TakeDamage(MaxHealth);
            }
        }
    }
}
