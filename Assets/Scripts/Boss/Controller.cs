using System;
using System.Collections;
using Player.V2;
using UnityEngine;
using UnityEngine.Events;

namespace Boss
{
    public class Controller : MonoBehaviour
    {
        public int maxHealth = 100;
        [HideInInspector] public float currentHealth;
        private AudioManager audioManager;
        private bool damageLock;
        private bool playerWon;
        public event Action Death;
        public event Action<float> OnDamaged;
        public UnityEvent ChangedPhase;
        private int invincibilityFrames = 0;
        private int invincibilityFramesMax = 300;
        [SerializeField] GameObject forceField;
        
        void Start()
        {
            Player.V2.Teleport.OnBossAttacked += OnTeleportOnBossAttacked;
            audioManager = FindObjectOfType<AudioManager>();
            currentHealth = maxHealth;
            OnDamaged += TakeDamage;
            forceField.SetActive(false);
            OnDamaged += (float i) => ChangedPhase.Invoke();
        }

        private void OnTeleportOnBossAttacked(float damage)
        {
            OnDamaged?.Invoke(damage);
        }

        void Update()
        {
            if (invincibilityFrames < invincibilityFramesMax)
            {
                invincibilityFrames += 1;
            }
        }
    
        public void UnlockDamage() => damageLock = false;
        public void LockDamage() => damageLock = true;

        public void TakeDamage(float damage)
        {
            if (damageLock) return;
            currentHealth -= damage;
            
            StartCoroutine(InvincibilityFrames());
            if (currentHealth <= 0)
            {
                Death?.Invoke();
            }
        }

        private IEnumerator InvincibilityFrames()
        {
            invincibilityFrames = 0;
            while (invincibilityFrames < invincibilityFramesMax)
            {
                forceField.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                forceField.SetActive(false);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}