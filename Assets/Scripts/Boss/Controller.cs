using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Boss
{
    public class Controller : MonoBehaviour
    {
        public int maxHealth = 100;
        public HealthBar healthBar;
        private int currentHealth;
        private bool damageLock;
        private bool playerWon;
        private UIDocument winDocument;
        // private float damageCooldownSeconds = 2f;
        void Awake()
        {
            winDocument = Instantiate(Resources.Load<GameObject>("Win Screen")).GetComponent<UIDocument>();
            winDocument.rootVisualElement.AddToClassList("hidden");
        }

        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
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
            if (Input.GetKeyDown(KeyCode.K))
            {
                TakeDamage(10);
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
            healthBar.SetHealth(currentHealth);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "PlayerTag")
            {
                TakeDamage(50);
            }
        }
    }
}
