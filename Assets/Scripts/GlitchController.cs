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
        [SerializeField] private CutsceneActivator cutsceneActivator;

        void Start()
        {
            playerController = GetComponent<V2.Controller>();
            phaseController = FindObjectOfType<PhaseController>();
            audioManager = FindObjectOfType<AudioManager>();
            cutsceneActivator.glitchOnCutscene.AddListener(() => CutsceneValues());
            phaseController.phaseChanged.AddListener(() => GlitchOnPhase());
            UpdateAllGlitchValues();
        }

        void GlitchOnPhase()
        {
            noiseAmount += 5;
            glitchStrength += 0.5f;
            flickerStrength += 0.02f;
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

        void CutsceneValues()
        {
            noiseAmount = 100;
            glitchStrength = 10;
            flickerStrength = 1;
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
