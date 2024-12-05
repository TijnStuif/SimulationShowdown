using System.Collections.Generic;
using System.Linq;
using Boss;
using Boss.Attack;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    private List<IAttack> attacks;
    private List<IAttack> environmentAttacks;
    private List<IAttack> directAttacks;
    [SerializeField] private PhaseController phaseController;
    private float timer;
    public float minAttackInterval = 4f; 
    public float maxAttackInterval = 5f;
    
    void Start()
    {
        // Gets all the objects in the scene that have the IAttack interface
        attacks = phaseController.phases[phaseController.currentPhase];

        // list of attacks per type
        // environmentAttacks = attacks.Where(attack => attack.Type == Type.Environment).ToList();
        // directAttacks = attacks.Where(attack => attack.Type == Type.Direct).ToList();

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

        attacks = phaseController.phases[phaseController.currentPhase];
        int randomIndex = Random.Range(0, attacks.Count);
        attacks[randomIndex].Execute();
    }

    private void SetRandomInterval()
    {
        timer = Mathf.Round(Random.Range(minAttackInterval, maxAttackInterval));
    }
}