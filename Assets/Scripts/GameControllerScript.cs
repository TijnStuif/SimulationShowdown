using System.Collections.Generic;
using System.Linq;
using Boss.Attack;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    private List<IAttack> attacks;
    private List<IAttack> environmentAttacks;
    private List<IAttack> directAttacks;
    private float timer;
    public float minAttackInterval = 5f; 
    public float maxAttackInterval = 15f;
    
    void Start()
    {
        // Gets all the objects in the scene that have the IAttack interface
        attacks = FindObjectsOfType<MonoBehaviour>().OfType<IAttack>().ToList();

        // list of attacks per type
        environmentAttacks = attacks.Where(attack => attack.Type == Type.Environment).ToList();
        directAttacks = attacks.Where(attack => attack.Type == Type.Direct).ToList();

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
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, attacks.Count);
        attacks[randomIndex].Execute();
    }

    private void SetRandomInterval()
    {
        timer = Mathf.Round(UnityEngine.Random.Range(minAttackInterval, maxAttackInterval));
    }
}