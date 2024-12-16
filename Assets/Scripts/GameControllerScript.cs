using System.Collections.Generic;
using System.Linq;
using Boss.Attack;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public static bool Frozen;
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

        if (timer <= 0f && Frozen == false)
        {
            ExecuteRandomAttack();
            SetRandomInterval(); 
        }
    }

    private void ExecuteRandomAttack()
    {
        if (attacks.Count == 0)
        {
            #if DEBUG
            Debug.Log("No attacks left to execute");
            #endif
            return;
        }

        int randomIndex = Random.Range(0, attacks.Count);
        attacks[randomIndex].Execute();
        #if DEBUG
        Debug.Log($"executing {attacks[randomIndex].GetType()}");
        #endif
    }

    private void SetRandomInterval()
    {
        timer = Mathf.Round(Random.Range(minAttackInterval, maxAttackInterval));
    }

    public void UpdateAttacks()
    {
        attacks = phaseController.phases[phaseController.currentPhase];
        #if DEBUG
        Debug.Log($"Updating attacks (phase {phaseController.currentPhase})");
        foreach (var attack in attacks)
        {
            Debug.Log($"{attack.GetType().Name}"); 
        }
        #endif
    }
}