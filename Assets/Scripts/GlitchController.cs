using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Player.V2
{
    public class GlitchController : MonoBehaviour
    {
        [SerializeField] private Material glitchMaterial;
        private float noiseAmount = 30f;
        private float glitchStrength = 1f;
        private float scanLineStrength = 0;
        private float flickerStrength = 0.05f;
        private float baseNoiseAmount = 30f;
        private float baseGlitchStrength = 1f;
        private float baseScanLineStrength = 0;
        private float baseFlickerStrength = 0.05f;
        private V2.Controller playerController;
        private PhaseController phaseController;
        // Start is called before the first frame update
        void OnEnable()
        {
            playerController = GetComponent<V2.Controller>();
            phaseController = FindObjectOfType<PhaseController>();
            phaseController.phaseChanged.AddListener(() => GlitchOnPhase());
            UpdateAllGlitchValues();
            Invoke(nameof(StartCutsceneValues), 1.5f);
        }

        void GlitchOnPhase()
        {
            noiseAmount += 10;
            glitchStrength += 1;
            flickerStrength += 0.05f;
            UpdateAllGlitchValues();
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
            Invoke(nameof(NormalValues), 2f);
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
