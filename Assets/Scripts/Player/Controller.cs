using System;
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
        [HideInInspector] public int currentHealth;

        public event Action<State> StateChange;

        void Awake()
        {
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
