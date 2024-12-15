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
        private AudioManager audioManager;
        private bool damageLock;
        private bool playerWon;
        private int damageToTake = 25;
        public event Action Death;
        public event Action<int> OnDamaged;
        public UnityEvent ChangedPhase;
        private int invincibilityFrames = 0;
        private int invincibilityFramesMax = 300;
        [SerializeField] GameObject forceField;
        
        void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
            currentHealth = maxHealth;
            OnDamaged += TakeDamage;
            forceField.SetActive(false);
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

        public void TakeDamage(int damage)
        {
            if (damageLock) return;
            currentHealth -= damage;
            audioManager.PlaySFX(audioManager.bossDamagedSFX[UnityEngine.Random.Range(0, audioManager.bossDamagedSFX.Length)]);
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