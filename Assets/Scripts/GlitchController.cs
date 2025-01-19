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
        private float baseNoiseAmount = 0f;
        private float baseGlitchStrength = 0.25f;
        private float baseScanLineStrength = 0;
        private float baseFlickerStrength = 0.05f;
        private V2.Controller playerController;
        private PhaseController phaseController;
        private AudioManager audioManager;
        [SerializeField] private CutsceneActivator cutsceneActivator;
        [SerializeField] private CutsceneDeactivator cutsceneDeactivator;

        void Start()
        {
            playerController = GetComponent<V2.Controller>();
            phaseController = FindObjectOfType<PhaseController>();
            audioManager = FindObjectOfType<AudioManager>();
            cutsceneActivator.glitchOnCutscene.AddListener(() => CutsceneValues());
            cutsceneDeactivator.glitchEnd.AddListener(() => NormalValues());
            phaseController.phaseChanged.AddListener(() => GlitchOnPhase());
            UpdateAllGlitchValues();
        }

        //every phase change increments the glitch shader values by these specific amounts
        void GlitchOnPhase()
        {
            noiseAmount += 20f;
            glitchStrength += 0.25f;
            flickerStrength += 0.02f;
            UpdateAllGlitchValues();
            audioManager.PlaySFX(audioManager.bossGlitchSFX);
        }

        //updates values in the glitch shader graph
        void UpdateAllGlitchValues()
        {
            glitchMaterial.SetFloat("_NoiseAmount", noiseAmount);
            glitchMaterial.SetFloat("_GlitchStrength", glitchStrength);
            glitchMaterial.SetFloat("_ScanLineStrength", scanLineStrength);
            glitchMaterial.SetFloat("_FlickerStrength", flickerStrength);
        }

        //these are the glitch shader values used by most cutscenes that transition between gameplay and cutscenes
        void CutsceneValues()
        {
            noiseAmount = 100;
            glitchStrength = 10;
            flickerStrength = 1;
            UpdateAllGlitchValues();
            audioManager.PlaySFX(audioManager.bossGlitchSFX);
            Invoke(nameof(NormalValues), 2.5f);
        }

        //these are the glitch shader values used in phase 1
        void NormalValues()
        {
            noiseAmount = baseNoiseAmount;
            glitchStrength = baseGlitchStrength;
            flickerStrength = baseFlickerStrength;
            UpdateAllGlitchValues();
        }
    }
}
