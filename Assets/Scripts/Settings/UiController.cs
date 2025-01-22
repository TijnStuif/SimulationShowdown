using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Settings
{
    public class UiController : AbstractUiController
    {
        [SerializeField] private UIDocument m_uiDocument;
        private Button m_graphicsOption;
        private Button m_controlsOption;
        private Button m_exitOption;

        private VisualElement m_controlsSection;
        private VisualElement m_graphicsSection;

        public event Action Exit;
        // Start is called before the first frame update
        void Awake()
        {
            Root = m_uiDocument.rootVisualElement;
            m_graphicsOption = m_uiDocument.rootVisualElement.Q<Button>("GraphOpt");
            m_controlsOption = m_uiDocument.rootVisualElement.Q<Button>("ContrOpt");
            m_exitOption = m_uiDocument.rootVisualElement.Q<Button>("ExitOpt");
            m_graphicsSection = m_uiDocument.rootVisualElement.Q<VisualElement>("grahpics");
            m_controlsSection = m_uiDocument.rootVisualElement.Q<VisualElement>("controls");
        }

        private void OnEnable()
        {
            m_graphicsOption.RegisterCallback<ClickEvent>(OnGraphicsOption);
            m_controlsOption.RegisterCallback<ClickEvent>(OnControlsOption);
            m_exitOption.RegisterCallback<ClickEvent>(OnExitOption);
        }
        private void OnDisable()
        {
            m_graphicsOption.UnregisterCallback<ClickEvent>(OnGraphicsOption);
            m_controlsOption.UnregisterCallback<ClickEvent>(OnControlsOption);
            m_exitOption.UnregisterCallback<ClickEvent>(OnExitOption);
        }

        private void OnGraphicsOption(ClickEvent evt)
        {
            m_graphicsOption.ToggleInClassList("hidden");
        }
        
        private void OnControlsOption(ClickEvent evt)
        {
            m_controlsOption.ToggleInClassList("hidden");
        }
        
        private void OnExitOption(ClickEvent evt)
        {
            Exit?.Invoke();
        }

    }
}
