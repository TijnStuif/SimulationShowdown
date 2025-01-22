using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

/// <summary>
/// V1 script
/// This class handles state change events and is able to change state such as
/// game-overing, pausing, freezing state, etc.
/// It's also a singleton although the Instance has a private access level
/// so it's not globally accessible but there's only one instance
/// </summary>
public class StateController : MonoBehaviour
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
    [SerializeField] private GameObject m_gameOverScreenPrefab;
    [SerializeField] private GameObject m_winScreenPrefab;
    [SerializeField] private GameObject m_settingsPrefab;
    
    // object instances
    private GameObject m_pauseMenu;
    private GameObject m_gameOverScreen;
    private GameObject m_winScreen;
    private GameObject m_settings;
    
    // controller scripts
    private Player.V2.Controller m_playerController;
    private Boss.Controller m_bossController;
    private Settings.UiController m_settingsController;
    private AudioManager m_audioManager;
    private CutsceneManager m_cutsceneManager;
    
    // for nesting ui menus
    private AbstractUiController m_currentUiController;
    private AbstractUiController m_previousUiController; 
    
    // UI controller scripts
    private PauseMenu.Controller m_pauseMenuController;
    private GameOverScreen.Controller m_gameOverScreenController;
    private WinScreen.Controller m_winScreenController;


    private List<AbstractUiController> UiScripts { get; set; }
    
    private bool m_gamePaused;
    
    /// <summary>
    /// prevents the game to be paused
    /// </summary>
    private bool PauseLock { get; set; }

    // pseudo singleton which ensures there is no other instance of this class,
    // while not giving any other class access to the instance
    // so no encapsulation is broken while impossible state is avoided
    //     (like there being two state managers one of which think the state is different from what it actually is)
    private static StateController Instance { get; set; }

    // when game is paused or unpaused the state must also be paused
    // realistically if this code remains unchanged, this isn't necessary
    // but just in case
    private bool GamePaused
    {
        get => m_gamePaused; 
        set => m_gamePaused = StatePaused = value;
    }
    private bool StatePaused { get; set; }

    private void Awake()
    {
        // avoid multiple instances of self
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            throw new InvalidOperationException("ERROR: Can't call Awake.\nStateController should only have one instance!");
        }
        else
            Instance = this;
        
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
        m_pauseMenu = Instantiate(m_pauseMenuPrefab);
        m_gameOverScreen = Instantiate(m_gameOverScreenPrefab);
        m_winScreen = Instantiate(m_winScreenPrefab);
        m_settingsPrefab = Instantiate(m_settingsPrefab);
    }

    private void InitControllerScripts()
    {
        UiScripts = new List<AbstractUiController>(3);

        // assign script, if it's null, throw exception
        if ((m_playerController = FindObjectOfType<Player.V2.Controller>()) == null)
            throw new ScriptNotFoundException(nameof(m_playerController));

        if ((m_bossController = FindObjectOfType<Boss.Controller>()) == null)
            throw new ScriptNotFoundException(nameof(m_bossController));

        if ((m_audioManager = FindObjectOfType<AudioManager>()) == null)
            throw new ScriptNotFoundException(nameof(m_audioManager));

        if ((m_cutsceneManager = FindObjectOfType<CutsceneManager>()) == null)
            throw new ScriptNotFoundException(nameof(m_cutsceneManager));

        if ((m_pauseMenuController = m_pauseMenu.GetComponent<PauseMenu.Controller>()) == null)
            throw new ScriptNotFoundException(nameof(m_pauseMenuController));
        else
            UiScripts.Add(m_pauseMenuController);
        
        if ((m_settingsController = m_settings.GetComponent<Settings.UiController>()) == null)
            throw new ScriptNotFoundException(nameof(m_settingsController));
        else
            UiScripts.Add(m_settingsController);

        if ((m_gameOverScreenController = m_gameOverScreen.GetComponent<GameOverScreen.Controller>()) == null)
            throw new ScriptNotFoundException(nameof(m_gameOverScreenController));
        else
            UiScripts.Add(m_gameOverScreenController);

        if ((m_winScreenController = m_winScreen.GetComponent<WinScreen.Controller>()) == null)
            throw new ScriptNotFoundException(nameof(m_winScreenController));
        else
            UiScripts.Add(m_winScreenController);
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
        m_cutsceneManager.winCutscene.stopped += OnBossDeath;
        m_cutsceneManager.loseCutscene.stopped += OnPlayerDeath;
        m_playerController.StateChange += OnPlayerStateChange;
        m_pauseMenuController.StateChange += OnPauseMenuStateChange;
        m_gameOverScreenController.StateChange += OnGameOverScreenStateChange;
        m_winScreenController.StateChange += OnWinScreenStateChange;
        m_settingsController.Exit += OnSettingsExit;
    }

    private void OnSettingsExit()
    {
        if (m_currentUiController == m_settingsController)
            m_previousUiController.Show();
        else
            Debug.LogError("what?");
    }

    private void UnsubscribeFromEvents()
    {
        m_cutsceneManager.winCutscene.stopped -= OnBossDeath;
        m_cutsceneManager.loseCutscene.stopped -= OnPlayerDeath;
        m_playerController.StateChange -= OnPlayerStateChange;
        m_pauseMenuController.StateChange -= OnPauseMenuStateChange;
        m_gameOverScreenController.StateChange -= OnGameOverScreenStateChange;
        m_winScreenController.StateChange -= OnWinScreenStateChange;
        m_settingsController.Exit -= OnSettingsExit;
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
        ThawState();
    }

    private void OnBossDeath(PlayableDirector director)
    {
        Win();
    }

    //this is bad code, but since I couldn't get my cutscene to work with the "OnPlayerStateChange" method, I had to make a separate method for the player death cutscene - Tijn Stuifbergen
    private void OnPlayerDeath(PlayableDirector director)
    {
        Lose();
    }

    private void OnPlayerStateChange(Player.V2.State state)
    {
        switch (state)
        {
            case Player.V2.State.Loss:
                // Lose();
                break; 
            case Player.V2.State.Pause:
                #if DEBUG
                Debug.Log("Paused!");
                #endif
                TogglePause();
                break;     
            default:
                throw new StateHandlerNotImplementedException();
        }
    }

    private void OnPauseMenuStateChange(PauseMenu.State state)
    {
        switch (state)
        {
            case PauseMenu.State.ResumeGame:
                Unpause();
                break;
            case PauseMenu.State.ReturnToTitleScreen:
                ReturnToTitle();
                break;
            case PauseMenu.State.Exit:
                throw new NotImplementedException("ERROR: Still have to implement quitting the game");
            case PauseMenu.State.Settings:
                m_previousUiController = m_pauseMenuController;
                m_previousUiController.Hide();
                m_currentUiController = m_settingsController;
                m_currentUiController.Show();
                break;
            default:
                throw new StateHandlerNotImplementedException();
        }
    }

    private void OnGameOverScreenStateChange(GameOverScreen.State state)
    {
        switch (state)
        {
            case GameOverScreen.State.ReturnToTitleScreen:
                ReturnToTitle();
                break;
            case GameOverScreen.State.Restart:
                RestartGameplay();
                break;
            default:
                throw new StateHandlerNotImplementedException();
        }
    }

    private void OnWinScreenStateChange(WinScreen.State state)
    {
        switch (state)
        {
            case WinScreen.State.ReturnToTitleScreen:
                ReturnToTitle();
                break;
            case WinScreen.State.Restart:
                RestartGameplay();
                break;
            default:
                throw new StateHandlerNotImplementedException();
        }
    }

    private void Win()
    {
        Debug.Log("You win!");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_audioManager.PlayMusic(m_audioManager.winScreenMusic);
        PauseLock = true;
        FreezeState();
        m_winScreenController.Show();
        
    }

    private void Lose()
    {
        Debug.Log("You lose!");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_audioManager.PlayMusic(m_audioManager.gameOverScreenMusic);
        PauseLock = true;
        FreezeState();
        m_gameOverScreenController.Show();
    }

    private void ReturnToTitle()
    {
        Debug.Log("Returning to title screen");
        ThawState();
        SceneManager.LoadScene("StartMenu");
        m_audioManager.PlayMusic(m_audioManager.mainMenuMusic);
    }

    private void RestartGameplay()
    {
        Debug.Log("Restarting gameplay");
        SceneManager.LoadScene("GameScene");
        m_audioManager.PlayMusic(m_audioManager.bossFightMusic);
    }

    /// <summary>
    /// freeze/pause any operations using delta time
    /// </summary>
    private void FreezeState()
    {
        Time.timeScale = 0f;
        StatePaused = true;
    }

    /// <summary>
    /// thaw/resume any operations using delta time
    /// </summary>
    private void ThawState()
    {
        Time.timeScale = 1f;
        StatePaused = false;
    }

    /// <summary>
    /// toggle any operations using delta time
    /// </summary>
    private void ToggleState()
    {
        if (StatePaused)
            ThawState();
        else
           FreezeState();
    }

    /// <summary>
    /// pauses game by freezing state and showing pause menu
    /// </summary>
    private void Pause()
    {
        if (PauseLock) return;
        m_audioManager.DampenMusic();
        FreezeState();
        m_pauseMenuController.Show();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GamePaused = true;
        
    }

    /// <summary>
    /// unpauses game by thawing/resuming state and hiding pause menu
    /// </summary>
    private void Unpause()
    {
        if (PauseLock) return;
        m_audioManager.UndampenMusic();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_pauseMenuController.Hide();
        ThawState();
        GamePaused = false;
    }
    
    /// <summary>
    /// toggle game pausing by toggling state and pause menu
    /// </summary>
    private void TogglePause()
    {
        if (PauseLock) return;
        
        if (GamePaused != StatePaused) return;
        if (GamePaused)
            Unpause();
        else
            Pause();
    }
}