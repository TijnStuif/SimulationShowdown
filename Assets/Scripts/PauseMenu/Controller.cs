using System;
using UnityEngine.UIElements;

namespace PauseMenu
{
    public enum State
    {
       ResumeGame,
       ReturnToTitleScreen,
       Exit
    }
    
    public class Controller : AbstractUiController
    {
        private Button m_resumeButton;
        private Button m_returnToTitleScreenButton;
        public event Action<State> StateChange;
        
        // get necessary UI components
        private void Awake()
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            m_resumeButton = Root.Q<Button>("resume");
            m_returnToTitleScreenButton = Root.Q<Button>("return");
        }
        
        // this can be refactored in the future in an AbstractUIController class or smth

        private void OnEnable()
        {
            m_resumeButton.RegisterCallback<ClickEvent>(OnResumeClicked);
            m_returnToTitleScreenButton.RegisterCallback<ClickEvent>(OnReturnToTitleClicked);
        }
        
        private void OnDisable()
        {
            m_resumeButton.UnregisterCallback<ClickEvent>(OnResumeClicked);
        }

        private void OnResumeClicked(ClickEvent evt)
        {
            PlayButtonSound();
            StateChange?.Invoke(State.ResumeGame);
        }

        private void OnReturnToTitleClicked(ClickEvent evt)
        {
            PlayButtonSound();
            StateChange?.Invoke(State.ReturnToTitleScreen);
        }
    }
}
