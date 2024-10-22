using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public HealthBar healthBar;
    private int currentHealth;
   
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
}
