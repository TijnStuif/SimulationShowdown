using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    private List<IAttack> attacks;
    private float timer;
    public float minAttackInterval = 5f; 
    public float maxAttackInterval = 15f; 

    void Start()
    {
        // Gets all the objects in the scene that have the IAttack interface
        attacks = FindObjectsOfType<MonoBehaviour>().OfType<IAttack>().ToList();
        
        // Debug log all attacks
        foreach (var attack in attacks)
        {
            Debug.Log($"{attack.GetType().Name} - Type: {attack.Type}");
            Debug.Log(attack.GetType().Name);
        }

        SetRandomInterval(); 
    }

    void Update()
    {
        timer -= Time.deltaTime; 

        if (timer <= 0f)
        {
            ExecuteRandomAttack();
            SetRandomInterval(); 
        }
    }

    private void ExecuteRandomAttack()
    {
        if (attacks.Count == 0)
        {
            Debug.LogWarning("No attacks available to execute.");
            return;
        }

        int randomIndex = Random.Range(0, attacks.Count);
        attacks[randomIndex].Execute();
        Debug.Log($"Executed attack: {attacks[randomIndex].GetType().Name}");
    }

    private void SetRandomInterval()
    {
        timer = Mathf.Round(Random.Range(minAttackInterval, maxAttackInterval));
        Debug.Log($"Next attack in {timer} seconds");
    }
}