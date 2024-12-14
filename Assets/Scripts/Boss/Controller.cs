using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Boss
{
    public class Controller : MonoBehaviour
    {
        public int maxHealth = 100;
        [HideInInspector] public int currentHealth;
        private bool damageLock;
        private bool playerWon;
        private int damageToTake = 25;
        public event Action Death;
        public event Action<int> OnDamaged;
        public UnityEvent ChangedPhase;
        private int invincibilityFrames = 0;
        private int invincibilityFramesMax = 60;
        private Material bossMaterial;
        
        void Start()
        {
            bossMaterial = GetComponent<MeshRenderer>().material;
            currentHealth = maxHealth;
            OnDamaged += TakeDamage;
        }

        void Update()
        {
            if (invincibilityFrames < invincibilityFramesMax)
            {
                invincibilityFrames++;
            }
        }
    
        public void UnlockDamage() => damageLock = false;
        public void LockDamage() => damageLock = true;

        public void TakeDamage(int damage)
        {
            if (damageLock) return;
            currentHealth -= damage;
            StartCoroutine(InvincibilityFrames());
            if (currentHealth <= 0)
            {
                Death?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && invincibilityFrames >= 60)
            {
                OnDamaged?.Invoke(damageToTake);
                ChangedPhase?.Invoke();
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

        private IEnumerator InvincibilityFrames()
        {
            invincibilityFrames = 0;
            Color baseBossColor = bossMaterial.color;
            while (true)
            {
                Color color = bossMaterial.color;
                bossMaterial.color = Color.black;
                yield return new WaitForSeconds(0.2f);
                bossMaterial.color = baseBossColor;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}