using System;
using System.Collections;
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
        public UnityEvent HealthUpdated;
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

            
            OnPickUpCollected(0);
            pickUp = FindObjectOfType<PickUp>(true);
        }

        private void OnEnable()
        {
            Player.V2.Teleport.OnBossAttacked += OnTeleportOnBossAttacked;
            PickUp.PickUpCollected += OnPickUpCollected;
            OnDamaged += TakeDamage;
        }

        private void OnDisable()
        {
            Player.V2.Teleport.OnBossAttacked -= OnTeleportOnBossAttacked;
            PickUp.PickUpCollected -= OnPickUpCollected;
            OnDamaged -= TakeDamage;
        }

        private void OnPickUpCollected(float amountOfPickUpsCollected)
        {
            if (amountOfPickUpsCollected < 5)
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
            if (Input.GetKeyDown(KeyCode.P))
            {
                forceField.SetActive(false);
                UnlockDamage();
                OnTeleportOnBossAttacked(20);
            }
        }
    
        public void UnlockDamage() => damageLock = false;
        public void LockDamage() => damageLock = true;

        public void TakeDamage(float damage)
        {
            
            if (damageLock) return;
            currentHealth -= damage;
            StartCoroutine(ResetPickUpAmount());
            HealthUpdated.Invoke();
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
                OnPickUpCollected(pickUp.AmountOfPickupsCollected);
                yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator ResetPickUpAmount()
        {
            yield return new WaitForSeconds(MASH_LENGTH);
            pickUp.AmountOfPickupsCollected = 0;
        }
    }
}