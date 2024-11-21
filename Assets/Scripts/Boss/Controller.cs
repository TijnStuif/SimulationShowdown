using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Boss
{
    public class Controller : MonoBehaviour
    {
        public int maxHealth = 100;
        [HideInInspector] public int currentHealth;
        private bool damageLock;
        private bool playerWon;
        private UIDocument winDocument;
        [SerializeField] private GameObject winDocumentPrefab;
        
        void Awake()
        {
            var winDocumentObj = Instantiate(winDocumentPrefab);
            winDocument = winDocumentObj.GetComponent<UIDocument>();
            winDocument.rootVisualElement.AddToClassList("hidden");
        }

        void Start()
        {
            currentHealth = maxHealth;
        }

        void Update()
        {
            if (currentHealth <= 0 && !playerWon)
            {
                playerWon = true;
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                winDocument.rootVisualElement.RemoveFromClassList("hidden");
            }
        }
    
        public void UnlockDamage() => damageLock = false;
        public void LockDamage() => damageLock = true;

        public void TakeDamage(int damage)
        {
            if (damageLock) return;
            // LockDamage();
            // Invoke(nameof(UnlockDamage), damageCooldownSeconds);
            currentHealth -= damage;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                TakeDamage(50);
            }
        }
    }
}