using System;
using System.Collections.Generic;
using Boss.Attack;
using UnityEngine;
using UnityEngine.Events;
public class PhaseController : MonoBehaviour
{
    private Boss.Controller bossController;
    public Dictionary<int, List<IAttack>> phases;
    public int currentPhase;
    private GameControllerScript gameControllerScript;
    private int phaseHealthThreshold;
    public UnityEvent phaseChanged;
    private void Awake()
    {
        bossController = FindObjectOfType<Boss.Controller>();
        gameControllerScript = FindObjectOfType<GameControllerScript>();
        phases = new Dictionary<int, List<IAttack>>{
            {1, new List<IAttack>{gameObject.AddComponent<Laser>(), gameObject.AddComponent<CloseRange>()}},
            {2, new List<IAttack>{gameObject.AddComponent<LowGravity>(), gameObject.AddComponent<InvertedControls>(), gameObject.AddComponent<Laser>(), gameObject.AddComponent<CloseRange>()}},
            {3, new List<IAttack>{gameObject.AddComponent<SceneFlip>(), gameObject.AddComponent<LowGravity>(), gameObject.AddComponent<InvertedControls>(), gameObject.AddComponent<Laser>(), gameObject.AddComponent<CloseRange>(), gameObject.AddComponent<FallingPlatforms>()}},
            {4, new List<IAttack>{gameObject.AddComponent<SceneFlip>(), gameObject.AddComponent<LowGravity>(), gameObject.AddComponent<InvertedControls>(), gameObject.AddComponent<Laser>(), gameObject.AddComponent<CloseRange>(), gameObject.AddComponent<FallingPlatforms>()}},
        };
        bossController.CheckForPhaseChanged.AddListener(() => UpdatePhase());
        phaseChanged.AddListener(() => gameControllerScript.UpdateAttacks());
        currentPhase = 1;
    }
    private void Start()
    {
        phaseHealthThreshold = bossController.maxHealth / 4;
    }
    private void UpdatePhase()
    {
        if (currentPhase >= 4) return;
        float checkCurrentPhase = currentPhase;
        if (bossController.currentHealth <= phaseHealthThreshold * 3 && bossController.currentHealth > phaseHealthThreshold * 2)
        {
            currentPhase = 2;
        }
        else if (bossController.currentHealth <= phaseHealthThreshold * 2 && bossController.currentHealth > phaseHealthThreshold)
        {
            currentPhase = 3;
        }
        else if (bossController.currentHealth <= phaseHealthThreshold)
        {
            currentPhase = 4;
        }
        switch (currentPhase)
        {
            case 2:
                gameControllerScript.minAttackInterval = 5f;
                gameControllerScript.maxAttackInterval = 7f;
                break;
            case 3:
                gameControllerScript.minAttackInterval = 4f;
                gameControllerScript.maxAttackInterval = 6f;
                break;
            case 4:
                gameControllerScript.minAttackInterval = 3f;
                gameControllerScript.maxAttackInterval = 5f;
                break;
        }
        if (checkCurrentPhase != currentPhase)
        {
            PhaseChange();
        }
    }

    void PhaseChange()
    {
        phaseChanged.Invoke();
        Debug.Log($"Phase changed to {currentPhase}");
    }
}
