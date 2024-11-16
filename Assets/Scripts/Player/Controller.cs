using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        public int maxHealth = 100;
        public HealthBar healthBar;
        private int currentHealth;
        private GameObject gameOverFab;
        private UIDocument gameOverDocument;
        private bool lost;

        private void Awake()
        {
            // loading resource like this so I don't need to modify the scene
            // resources can be loaded like this when your resources is in the Assets/Resources folder
            try
            {
                gameOverFab = Resources.Load<GameObject>("Prefabs/Game Over Screen");
                gameOverDocument = Instantiate(gameOverFab).GetComponent<UIDocument>();
                gameOverDocument.rootVisualElement.AddToClassList("hidden");
            }
            catch (ArgumentException)
            {
               Debug.LogError("ERROR: prefab path is invalid, have you considered creating a resource manager?"); 
            }
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

        private void OnTriggerEnter(Collider other)
        {
            TakeDamage(10);
        }

        private void Update()
        {
            if (currentHealth <= 0 && lost == false)
            {
                lost = true;
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                gameOverDocument.rootVisualElement.RemoveFromClassList("hidden");
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                TakeDamage(10);
            }
        }
    }
}
