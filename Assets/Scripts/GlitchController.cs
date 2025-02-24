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
        private float baseGlitchStrength = 0;
        private float baseScanLineStrength = 0;
        private float baseFlickerStrength = 0.05f;
        private float noiseAmountIncrement = 20f;
        private float glitchStrengthIncrement = 0.3f;
        private float flickerStrengthIncrement = 0.02f;
        private float noiseAmountMax = 100f;
        private float glitchStrengthMax = 10f;
        private float flickerStrengthMax = 1f;
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
            noiseAmount += noiseAmountIncrement;
            glitchStrength += glitchStrengthIncrement;
            flickerStrength += flickerStrengthIncrement;
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
            noiseAmount = noiseAmountMax;
            glitchStrength = glitchStrengthMax;
            flickerStrength = flickerStrengthMax;
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
