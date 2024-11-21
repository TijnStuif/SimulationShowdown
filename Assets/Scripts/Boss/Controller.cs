using System;
using UnityEngine;

namespace Boss
{
    public class Controller : MonoBehaviour
    {
        public int maxHealth = 100;
        public HealthBar healthBar;
        private int currentHealth;
        private bool m_damageLock;
        
        public event Action Death;
        
        void Awake()
        {
        }

        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                TakeDamage(10);
            }
        }
    
        private void UnlockDamage() => m_damageLock = false;
        private void LockDamage() => m_damageLock = true;

        public void TakeDamage(int damage)
        {
            if (m_damageLock) return;
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                Death?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // should use events ! ! !
            Debug.Log("Ow");
            if (other.CompareTag("PlayerTag"))
            { 
                TakeDamage(50); 
                // LockDamage();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                UnlockDamage();
            }
        }
    }
}