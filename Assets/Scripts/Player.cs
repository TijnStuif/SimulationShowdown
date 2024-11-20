using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
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
    }

    public void TakeDamage(int damage)
    {   
        currentHealth -= damage;
    }
    // hotfix
    // I think aside from how the boss is identified, this isn't a bad solution
    // I just want to avoid modifying the scene so I can't add tags to the boss
    private void OnCollisionEnter(Collision other)
    {
        // when collision happens 
        // try to get the boss script as component (very inefficient)
        var possibleBoss = other.gameObject.GetComponent<Boss>();
        // if it actually worked, then the collider is a boss
        if (possibleBoss != null)
        {
            // damage it
           possibleBoss.TakeDamage(50);
           // boss can't be damaged
           possibleBoss.LockDamage();
        }
    }
    private void OnCollisionExit(Collision other)
    {
        // same thing but when a collision ends
        var possibleBoss = other.gameObject.GetComponent<Boss>();
        if (possibleBoss != null)
        {
            // boss can now be damaged again
           possibleBoss.UnlockDamage(); 
        }
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
