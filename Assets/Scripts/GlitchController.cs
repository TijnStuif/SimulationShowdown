using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.V2
{
    public class GlitchController : MonoBehaviour
    {
        [SerializeField] private Material glitchMaterial;
        private float noiseAmount = 0;
        private float glitchStrength = 0;
        private float scanLineStrength = 0;
        private float flickerStrength = 0;
        private float baseNoiseAmount = 30f;
        private float baseGlitchStrength = 1f;
        private float baseScanLineStrength = 0;
        private float baseFlickerStrength = 0.05f;
        private V2.Controller playerController;
        private PhaseController phaseController;
        private AudioManager audioManager;

        void OnEnable()
        {
            playerController = GetComponent<V2.Controller>();
            phaseController = FindObjectOfType<PhaseController>();
            audioManager = FindObjectOfType<AudioManager>();
            phaseController.phaseChanged.AddListener(() => GlitchOnPhase());
            UpdateAllGlitchValues();
            Invoke(nameof(StartCutsceneValues), 3f);
        }

        void GlitchOnPhase()
        {
            noiseAmount += 10;
            glitchStrength += 1;
            flickerStrength += 0.05f;
            UpdateAllGlitchValues();
            audioManager.PlaySFX(audioManager.bossGlitchSFX);
        }

        void UpdateAllGlitchValues()
        {
            glitchMaterial.SetFloat("_NoiseAmount", noiseAmount);
            glitchMaterial.SetFloat("_GlitchStrength", glitchStrength);
            glitchMaterial.SetFloat("_ScanLineStrength", scanLineStrength);
            glitchMaterial.SetFloat("_FlickerStrength", flickerStrength);
        }

        void StartCutsceneValues()
        {
            noiseAmount = 100;
            glitchStrength = 10;
            flickerStrength = 0.5f;
            UpdateAllGlitchValues();
            audioManager.PlaySFX(audioManager.bossGlitchSFX);
            Invoke(nameof(NormalValues), 2.5f);
        }

        void NormalValues()
        {
            noiseAmount = baseNoiseAmount;
            glitchStrength = baseGlitchStrength;
            flickerStrength = baseFlickerStrength;
            UpdateAllGlitchValues();
        }
    }
}
