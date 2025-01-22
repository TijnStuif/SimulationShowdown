using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace StartMenu
{
    public enum State
    {
       Start,
       Settings
    }
    public class Controller : AbstractUiController
    {
        private VisualElement m_root;
        private Button m_startButton;
        private Button m_settingsButton;

        public event Action<State> StateChanged;
        // get necessary ui components
        void Awake()
        {
            m_root = GetComponent<UIDocument>().rootVisualElement;
            m_startButton = m_root.Q<Button>("start");
            m_settingsButton = m_root.Q<Button>("settings");
        }

        // register 
        private void OnEnable()
        {
            m_startButton.RegisterCallback<ClickEvent>(OnStartClicked);
            m_settingsButton.RegisterCallback<ClickEvent>(OnSettingsClicked);
        }

        private void OnDisable()
        {
            m_startButton.UnregisterCallback<ClickEvent>(OnStartClicked);
            m_settingsButton.UnregisterCallback<ClickEvent>(OnSettingsClicked);
        }

        private void OnStartClicked(ClickEvent evt)
        {
            PlayButtonSound();
            StateChanged?.Invoke(State.Start);
        }
        
        private void OnSettingsClicked(ClickEvent evt)
        {
            PlayButtonSound();
            StateChanged?.Invoke(State.Settings);
        }
    }
}