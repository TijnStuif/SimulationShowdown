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
        private PickUp pickUp;
        private const float MASH_LENGTH = 3f;
        
        void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
            currentHealth = maxHealth;
            forceField.SetActive(false);

            pickUp = FindObjectOfType<PickUp>();
        }

        private void OnEnable()
        {
            Player.V2.Teleport.OnBossAttacked += OnTeleportOnBossAttacked;
            OnDamaged += TakeDamage;
        }

        private void OnDisable()
        {
            Player.V2.Teleport.OnBossAttacked -= OnTeleportOnBossAttacked;
            OnDamaged -= TakeDamage;
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
            if (Input.GetKeyDown(KeyCode.I))
            {
                forceField.SetActive(false);
                UnlockDamage();
                OnTeleportOnBossAttacked(20);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                forceField.SetActive(false);
                UnlockDamage();
            }
            if (pickUp.pickUpsCollected < 5)
            {
                LockDamage();
                forceField.SetActive(true);
            }
            else
            {
                UnlockDamage();
                forceField.SetActive(false);
            }
        }
    
        public void UnlockDamage() => damageLock = false;
        public void LockDamage() => damageLock = true;

        public void TakeDamage(float damage)
        {
            StartCoroutine(ResetPickUpAmount());
            ChangedPhase.Invoke();
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

        private IEnumerator ResetPickUpAmount()
        {
            yield return new WaitForSeconds(MASH_LENGTH);
            pickUp.pickUpsCollected = 0;
        }
    }
}