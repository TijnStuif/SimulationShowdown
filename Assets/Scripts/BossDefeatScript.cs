using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeatScript : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Transform boss; // Reference to the boss's transform
    public float behindThreshold = 0.5f; // Threshold to determine if the player is behind the boss
    public KeyCode defeatKey = KeyCode.E; // Key to press to defeat the boss

    private BossFocusScript bossFocusScript; // Reference to the BossFocusScript

    void Start()
    {
        // Get the BossFocusScript component from the boss GameObject
        bossFocusScript = boss.GetComponent<BossFocusScript>();
    }

    void Update()
    {
        if (bossFocusScript.FocusBar < 50 && IsPlayerBehindBoss() && Input.GetKeyDown(defeatKey))
        {
            DefeatBoss();
        }
    }

    bool IsPlayerBehindBoss()
    {
        // Calculate the direction from the boss to the player
        Vector3 directionToPlayer = (player.position - boss.position).normalized;

        // Calculate the forward direction of the boss
        Vector3 bossForward = boss.forward;

        // Calculate the dot product to determine if the player is behind the boss
        float dotProduct = Vector3.Dot(bossForward, directionToPlayer);

        // If the dot product is less than the threshold, the player is behind the boss
        return dotProduct < -behindThreshold;
    }

    void DefeatBoss()
    {
        // Logic to defeat the boss
        Debug.Log("Boss defeated!");
        // You can add more logic here, such as playing an animation, destroying the boss, etc.
    }
}