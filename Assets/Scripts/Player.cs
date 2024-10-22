using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public HealthBar healthBar;
    private int currentHealth;
    private GameObject gameOverFab;
    private UIDocument gameOverDocument;
    private bool lost = false;

    private void Awake()
    {
        // loading resource like this so I don't need to modify the scene
        // resources can be loaded like this when your resources is in the Assets/Resources folder
        gameOverFab = Resources.Load<GameObject>("Game Over Screen");
        gameOverDocument = Instantiate(gameOverFab).GetComponent<UIDocument>();
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
    }
}
