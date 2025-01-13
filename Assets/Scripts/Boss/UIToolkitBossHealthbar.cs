using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Boss
{
    public class UIToolkitBossHealthbar : AbstractUiController
    {
        [SerializeField] private Boss.Controller m_bossController;
        private const float PROGRESSBAR_OUTLINE_WIDTH = 5;
        private VisualElement m_healthbar;
        private PhaseController m_phaseController;
        private ProgressBar m_progressBar;
   
        private void Awake()
        {
            //access the UI document
            Root = GetComponent<UIDocument>().rootVisualElement;
            m_phaseController = FindObjectOfType<PhaseController>();

            // a little confusing but this is the container for the health bar
            m_healthbar = Root.Q<VisualElement>("hp-container");
            // and this is the progress bar named healthbar
            m_progressBar = m_healthbar.Q<ProgressBar>("healthbar");

            // progressbar.style.top = -450;
        }

        private void Start()
        {
            StartCoroutine(SetThresholdBars());
            // set the width between each threshold bar based on the number of phases
        }

        private void OnEnable()
        {
            m_bossController.HealthUpdated.AddListener(OnBossHealthUpdated);
        }

        private void OnDisable()
        {
            m_bossController.HealthUpdated.RemoveListener(OnBossHealthUpdated);
        }

        private IEnumerator SetThresholdBars()
        {
            int phaseCount = m_phaseController.phases.Count;
            float[] thresholds = new float[phaseCount];
            // Wait until width is initialized
            yield return new WaitUntil(() => m_healthbar.resolvedStyle.width > 0);
        
            var progressbarWidth = m_healthbar.resolvedStyle.width;
            for (int i = 0; i < phaseCount; i++)
            {
                thresholds[i] = (i + 1) * (progressbarWidth / phaseCount) - (PROGRESSBAR_OUTLINE_WIDTH * 2);
            }

            // stylize the threshold bars
            for (int i = 0; i < phaseCount-1; i++)
            {
                VisualElement thresholdBar = new VisualElement();
                thresholdBar.name = "thresholdBar";
                thresholdBar.style.backgroundColor = new StyleColor(Color.black);
                thresholdBar.style.position = Position.Absolute;
                thresholdBar.style.width = 20f;
                thresholdBar.style.height = 20f;
                thresholdBar.style.left = thresholds[i];
                m_progressBar.Add(thresholdBar);
            }
        }

        private void OnBossHealthUpdated()
        {
            m_progressBar.value = m_bossController.currentHealth;
        }
    }
}
