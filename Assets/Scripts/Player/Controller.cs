using System;
using Boss.Attack;
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
        // hotfix
        // I think aside from how the boss is identified, this isn't a bad solution
        // I just want to avoid modifying the scene so I can't add tags to the boss
        private void OnCollisionEnter(Collision other)
        {
            // scuffed hotfix, inefficient, should be refactored ! ! !
            // when collision happens 
            // try to get the boss script as component (very inefficient)
            var bossController = other.gameObject.GetComponent<Boss.Controller>();
            // if it actually worked, then the collider is a boss
            // damage it
            if (bossController != null)
            {
                bossController.TakeDamage(50);
                // prevent boss to be damaged anymore
                bossController.LockDamage();
            }
        }
        private void OnCollisionExit(Collision other)
        {
            // same thing but when a collision ends
            var bossController = other.gameObject.GetComponent<Boss.Controller>();
            // if bossController exists, it can now be damaged again
            if (bossController != null) bossController.UnlockDamage(); 
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
    }
}
