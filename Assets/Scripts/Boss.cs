using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHealth = 100;
    public HealthBar healthBar;
    private int currentHealth;
    private bool damageLock;
    // private float damageCooldownSeconds = 2f;
    
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
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
