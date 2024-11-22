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
        public event Action<State> StateChange;
        
        // get necessary UI components
        private void Awake()
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            m_resumeButton = Root.Q<Button>("resume");
        }
        
        // this can be refactored in the future in an AbstractUIController class or smth

        private void OnEnable()
        {
            m_resumeButton.RegisterCallback<ClickEvent>(OnResumeClicked);
        }
        
        private void OnDisable()
        {
            m_resumeButton.UnregisterCallback<ClickEvent>(OnResumeClicked);
        }

        private void OnResumeClicked(ClickEvent evt)
        { 
            StateChange?.Invoke(State.ResumeGame);
        }

        private void OnReturnToTitleClicked()
        { 
            StateChange?.Invoke(State.ReturnToTitleScreen); 
        }
    }
}
