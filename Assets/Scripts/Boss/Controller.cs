using System;
using UnityEngine;

namespace Boss
{
    public class Controller : MonoBehaviour
    {
        public int maxHealth = 100;
        [HideInInspector] public int currentHealth;
        private bool damageLock;
        private bool playerWon;
        
        public event Action Death;
        
        void Start()
        {
            currentHealth = maxHealth;
        }
    
        public void UnlockDamage() => damageLock = false;
        public void LockDamage() => damageLock = true;

        public void TakeDamage(int damage)
        {
            if (damageLock) return;
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Debug.Log("What?");
                Death?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // should use events ! ! !
            if (other.CompareTag("Player"))
            { 
                TakeDamage(50); 
                // LockDamage();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // UnlockDamage();
            }
        }
    }
}