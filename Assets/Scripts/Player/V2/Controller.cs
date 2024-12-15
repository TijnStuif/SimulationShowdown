using System;
using Boss.Attack;
using UnityEngine;
using UnityEngine.InputSystem;

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
        }
        private void FixedUpdate()
        {
            //Check if the player is outside the map
            //If this is the case the player will die
            if (transform.position.x <= -15 || transform.position.z <= -15 || transform.position.x >= 15 || transform.position.z >= 15 || transform.position.y <= -3)
            {
                TakeDamage(maxHealth);
            }
        }
    }
}
