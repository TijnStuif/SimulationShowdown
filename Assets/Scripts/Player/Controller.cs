using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        public int maxHealth = 100;
        public HealthBar healthBar;
        private int currentHealth;
        [SerializeField] private GameObject gameOverPrefab;
        private UIDocument gameOverDocument;
        
        public StateController StateController { private get; set; }
        private bool Lost { get; set; }

        private void Awake()
        {
            gameOverDocument = Instantiate(gameOverPrefab).GetComponent<UIDocument>();
            gameOverDocument.rootVisualElement.AddToClassList("hidden");
        }

        void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int damage)
        {   
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
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

        public void OnPause(InputAction.CallbackContext c)
        {
            StateController.TogglePause();
        }

        private void OnTriggerEnter(Collider other)
        {
            TakeDamage(10);
        }

        private void Update()
        {
            if (currentHealth <= 0 && Lost == false)
            {
                Lost = true;
                StateController.FreezeState();
                gameOverDocument.rootVisualElement.RemoveFromClassList("hidden");
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                TakeDamage(10);
            }
        }
    }
}
