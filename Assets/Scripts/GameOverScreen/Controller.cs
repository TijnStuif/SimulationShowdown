using System;
using UnityEngine.UIElements;

namespace GameOverScreen
{
    public enum State
    {
       Restart,
       ReturnToTitleScreen
    }
    
    public class Controller : AbstractUiController
    {
        private Button m_restartButton;
        private Button m_titleScreenButton;

        public event Action<State> StateChange;

        private void Awake()
        {
            var uiDoc = GetComponent<UIDocument>();
            Root = GetComponent<UIDocument>().rootVisualElement;
            m_restartButton = Root.Q<Button>("restart");
            m_titleScreenButton = Root.Q<Button>("title-screen");
        }

        private void OnEnable()
        {
            m_restartButton.RegisterCallback<ClickEvent>(OnRestartClicked);
            m_titleScreenButton.RegisterCallback<ClickEvent>(OnTitleScreenClicked);
        }

        private void OnDisable()
        {
            m_restartButton.UnregisterCallback<ClickEvent>(OnRestartClicked);
            m_titleScreenButton.UnregisterCallback<ClickEvent>(OnTitleScreenClicked);
        }

        private void OnRestartClicked(ClickEvent evt)
        {
            PlayButtonSound();
            StateChange?.Invoke(State.Restart);
        }

        private void OnTitleScreenClicked(ClickEvent evt)
        {
            PlayButtonSound();
            StateChange?.Invoke(State.ReturnToTitleScreen);
        }
    }
}
