using System;
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
        public HealthBar healthBar;
        private int currentHealth;

        public event Action<State> StateChange;

        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int damage)
        {   
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                healthBar.SetHealth(0);
                StateChange?.Invoke(State.Loss);
                return;
            }
            healthBar.SetHealth(currentHealth);
        }

        public void OnPause(InputAction.CallbackContext c)
        {
            StateChange?.Invoke(State.Pause);
        }

        private void OnTriggerEnter(Collider other)
        {
            TakeDamage(10);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                TakeDamage(10);
            }
        }
    }
}
