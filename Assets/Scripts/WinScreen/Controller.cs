using System;
using UnityEngine.UIElements;

namespace WinScreen
{
    public enum State
    {
       Restart,
       ReturnToTitleScreen
    }
    public class Controller : AbstractUiController
    {
        private Button m_restartButton;
        private Button m_titleButton;

        public event Action<State> StateChange;
        
        void Awake()
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            m_restartButton = Root.Q<Button>("restart");
            m_titleButton = Root.Q<Button>("title-screen");
        }

        void OnEnable()
        {
            m_restartButton.RegisterCallback<ClickEvent>(OnRestartClicked);
            m_titleButton.RegisterCallback<ClickEvent>(OnTitleClicked);
        }

        void OnDisable()
        {
            m_restartButton.UnregisterCallback<ClickEvent>(OnRestartClicked);
            m_titleButton.UnregisterCallback<ClickEvent>(OnTitleClicked);
        }

        private void OnRestartClicked(ClickEvent evt)
        {
            PlayButtonSound();
            StateChange?.Invoke(State.Restart);
        }

        private void OnTitleClicked(ClickEvent evt)
        {
            PlayButtonSound();
            StateChange?.Invoke(State.ReturnToTitleScreen);
        }
    }
}
