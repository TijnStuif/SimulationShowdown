// bad code:
using System;
using System.Collections.Generic;
using StartMenu;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;
public class StartMenuStateController : MonoBehaviour
{
    /// <summary>
    /// Exception for when a state event handler is not implemented yet
    /// (Message is overridden)
    /// </summary>
    private class StateHandlerNotImplementedException : NotImplementedException
    {
        public StateHandlerNotImplementedException() : base("ERROR: State handler hasn't been implemented in StateController") {}
    }
    
    /// <summary>
    /// Exception for if a script was not found
    /// Message should be based on script name
    /// This should be done using nameof(variableName)
    /// </summary>
    public class ScriptNotFoundException : NullReferenceException
    {
        public ScriptNotFoundException(string scriptName) : base($"ERROR: could not find script: {scriptName}") {}
    }
    
    // prefabs
    [SerializeField] private GameObject m_pauseMenuPrefab;


    [SerializeField] private GameObject m_settingsPrefab;
    
    // object instances
    private GameObject m_pauseMenu;
    private GameObject m_settings;
    
    private StartMenu.Controller m_startMenuController;
    
    // controller scripts
    private Settings.UiController m_settingsController;
    private AudioManager m_audioManager;
    
    
    // for nesting ui menus
    private AbstractUiController m_currentUiController;
    private AbstractUiController m_previousUiController; 

    private List<AbstractUiController> UiScripts { get; set; }

    private void Awake()
    {
        m_startMenuController = FindObjectOfType<StartMenu.Controller>();
        
        InitMenus();
    }

    private void InitMenus()
    {
        InitMenuObjects();
        InitControllerScripts();
        HideAllMenus();
    }

    private void InitMenuObjects()
    {
        m_settingsPrefab = Instantiate(m_settingsPrefab);
    }

    private void InitControllerScripts()
    {
        UiScripts = new List<AbstractUiController>(3);
        
        if ((m_settingsController = FindObjectOfType<Settings.UiController>()) == null)
            throw new ScriptNotFoundException(nameof(m_settingsController));
        else
            UiScripts.Add(m_settingsController);
    }

    private void HideAllMenus()
    {
        foreach (var script in UiScripts)
        {
            script.Hide();
        }
    }

    private void SubscribeToEvents()
    {
        m_startMenuController.StateChanged += StartMenuControllerOnStateChanged;
        m_settingsController.Exit += OnSettingsExit;
    }

    private void StartMenuControllerOnStateChanged(State state)
    {
        switch (state)
        {
           case State.Start:
               RestartGameplay();
               break;
           case State.Settings:
               m_settingsController.Show();
               break;
        }
    }

    private void UnsubscribeFromEvents()
    {
        m_settingsController.Exit -= OnSettingsExit;
    }
    
    private void OnSettingsExit()
    {
        m_settingsController.Hide();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void ReturnToTitle()
    {
        Debug.Log("Returning to title screen");
        SceneManager.LoadScene("StartMenu");
        m_audioManager.PlayMusic(m_audioManager.mainMenuMusic);
    }

    private void RestartGameplay()
    {
        Debug.Log("Restarting gameplay");
        SceneManager.LoadScene("GameScene");
        m_audioManager.PlayMusic(m_audioManager.bossFightMusic);
    }
}
