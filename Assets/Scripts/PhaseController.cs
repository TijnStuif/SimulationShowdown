using System;
using System.Collections.Generic;
using Boss.Attack;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PhaseController : MonoBehaviour
{
    private ColorAdjustments colorAdjustments;
    private ChromaticAberration chromaticAberration;
    private DepthOfField depthOfField;
    private Volume volume;
    private Boss.Controller bossController;
    public Dictionary<int, List<IAttack>> phases;
    public int currentPhase;
    private GameControllerScript gameControllerScript;
    private MovingWalls movingWalls;
    private MovingGround movingGround;
    private int phaseHealthThreshold;
    private void Awake()
    {
        volume = FindObjectOfType<Volume>();
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out depthOfField);

        bossController = FindObjectOfType<Boss.Controller>();
        gameControllerScript = FindObjectOfType<GameControllerScript>();
        phases = new Dictionary<int, List<IAttack>>{
            {1, new List<IAttack>{gameObject.AddComponent<Laser>(), gameObject.AddComponent<CloseRange>(), gameObject.AddComponent<FallingPlatforms>()}},
            {2, new List<IAttack>{gameObject.AddComponent<LowGravity>(), gameObject.AddComponent<InvertedControls>(), gameObject.AddComponent<Laser>(), gameObject.AddComponent<CloseRange>(), gameObject.AddComponent<FallingPlatforms>()}},
            {3, new List<IAttack>{gameObject.AddComponent<SceneFlip>(), gameObject.AddComponent<LowGravity>(), gameObject.AddComponent<InvertedControls>(), gameObject.AddComponent<Laser>(), gameObject.AddComponent<CloseRange>(), gameObject.AddComponent<FallingPlatforms>()}},
            {4, new List<IAttack>{gameObject.AddComponent<SceneFlip>(), gameObject.AddComponent<LowGravity>(), gameObject.AddComponent<InvertedControls>(), gameObject.AddComponent<Laser>(), gameObject.AddComponent<CloseRange>(), gameObject.AddComponent<FallingPlatforms>()}},
        };
        currentPhase = 1;
    }
    private void Start()
    {
        phaseHealthThreshold = bossController.maxHealth / 4;
    }

    private void OnEnable()
    {
        bossController.HealthUpdated.AddListener(UpdatePhase);
    }

    private void OnDisable()
    {
        bossController.HealthUpdated.RemoveListener(UpdatePhase);
    }

    private void UpdatePhase()
    {
        
        if (currentPhase >= 4) return;
        if (bossController.currentHealth <= phaseHealthThreshold * 3 && bossController.currentHealth > phaseHealthThreshold * 2)
        {
            currentPhase = 2;
            chromaticAberration.intensity.value = 0.33f;
            depthOfField.focusDistance.value = 7f;
            
        }
        else if (bossController.currentHealth <= phaseHealthThreshold * 2 && bossController.currentHealth > phaseHealthThreshold)
        {
            currentPhase = 3;
            chromaticAberration.intensity.value = 0.66f;
            depthOfField.focusDistance.value = 6f;
            colorAdjustments.active = true;
        }
        else if (bossController.currentHealth <= phaseHealthThreshold)
        {
            currentPhase = 4;
            chromaticAberration.intensity.value = 1f;
            depthOfField.focusDistance.value = 5f;
        }
        switch (currentPhase)
        {
            case 2:
                gameControllerScript.minAttackInterval = 4f;
                gameControllerScript.maxAttackInterval = 5f;
                break;
            case 3:
                gameControllerScript.minAttackInterval = 3.5f;
                gameControllerScript.maxAttackInterval = 4.5f;
                break;
            case 4:
                gameControllerScript.minAttackInterval = 3f;
                gameControllerScript.maxAttackInterval = 4f;
                break;
        }
        #if DEBUG
        Debug.Log($"Current phase {currentPhase}");
        #endif
    }
}
