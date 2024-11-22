using System;
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

        void Awake()
        {
            DamageAttack.PlayerDamaged += TakeDamage;
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                StateChange?.Invoke(State.Loss);
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

            //Check if the player is underneath the map
            //If this is the case the player will die
            if (transform.position.y <= -5)
            {
                TakeDamage(maxHealth);
            }
        }
    }
}
