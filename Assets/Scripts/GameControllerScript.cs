using System.Collections.Generic;
using System.Linq;
using Boss.Attack;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    private Boss.Controller bossController;
    private List<IAttack> attacks;
    private List<IAttack> allAttacks;
    [SerializeField] private PhaseController phaseController;
    private float timer;
    public float minAttackInterval = 6f; 
    public float maxAttackInterval = 8f;
    
    void Start()
    {
        bossController = FindObjectOfType<Boss.Controller>();
        bossController.ChangedPhase.AddListener(() => UpdateAttacks());
        allAttacks = FindObjectsOfType<MonoBehaviour>().OfType<IAttack>().ToList();
        attacks = phaseController.phases[phaseController.currentPhase];

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

        int randomIndex = Random.Range(0, attacks.Count);
        attacks[randomIndex].Execute();
    }

    private void SetRandomInterval()
    {
        timer = Mathf.Round(Random.Range(minAttackInterval, maxAttackInterval));
    }

    public void UpdateAttacks()
    {
        attacks = phaseController.phases[phaseController.currentPhase];
    }
}