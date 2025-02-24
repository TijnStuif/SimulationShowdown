using System.Collections.Generic;
using System.Linq;
using Boss.Attack;
using Player.V2;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    private bool m_attacksAreFrozen;
    private Boss.Controller bossController;
    private List<IAttack> attacks;
    private List<IAttack> allAttacks;
    [SerializeField] private PhaseController phaseController;
    private float timer;
    public float minAttackInterval = 5f; 
    public float maxAttackInterval = 6f;
    
    void Start()
    {
        allAttacks = FindObjectsOfType<MonoBehaviour>().OfType<IAttack>().ToList();
        attacks = phaseController.phases[phaseController.currentPhase];

        SetRandomInterval(); 
    }

    private void OnEnable()
    {
        bossController = FindObjectOfType<Boss.Controller>();
        Teleport.MashSequenceStateChange += OnMashSequenceStateChange;
        bossController.HealthUpdated.AddListener(UpdateAttacks);
    }

    private void OnDisable()
    {
        Teleport.MashSequenceStateChange -= OnMashSequenceStateChange;
        bossController.HealthUpdated.RemoveListener(UpdateAttacks);
    }

    void Update()
    {
        timer -= Time.deltaTime; 

        if (timer <= 0f && m_attacksAreFrozen == false)
        {
            ExecuteRandomAttack();
            SetRandomInterval(); 
        }
    }

    private void ExecuteRandomAttack()
    {
        if (attacks.Count == 0)
            return;

        int randomIndex = Random.Range(0, attacks.Count);
        attacks[randomIndex].Execute();
        #if DEBUG
        Debug.Log($"executing {attacks[randomIndex].GetType()}");
        #endif
    }

    private void OnMashSequenceStateChange(Teleport.MashState state)
    {
        switch (state)
        {
            case Teleport.MashState.Start:
                m_attacksAreFrozen = true;
                break;
            case Teleport.MashState.End:
                m_attacksAreFrozen = false;
                break;
        }
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