using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class StateController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPrefab;
    private UIDocument m_pauseMenuDocument;
    private bool m_gamePaused;

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
            throw new InvalidOperationException("Can't call awake.\nStateController should only have one instance!");
        }
        else
            Instance = this;
        
        // instantiate pause menu
        GameObject pauseObj = Instantiate(pauseMenuPrefab);
        m_pauseMenuDocument = pauseObj.GetComponent<UIDocument>();
        // hide pause menu
        m_pauseMenuDocument.rootVisualElement.AddToClassList("hidden");
        AssignReferences();
    }

    private void AssignReferences()
    {
        // give pause menu controller a reference to self
        m_pauseMenuDocument.gameObject.GetComponent<PauseMenu.Controller>().StateController = this;
        // give player controller a reference to self
        FindObjectOfType<Player.Controller>().StateController = this;
    }

    /// <summary>
    /// freeze/pause any operations using delta time
    /// </summary>
    public void FreezeState()
    {
        Time.timeScale = 0f;
        StatePaused = true;
    }

    /// <summary>
    /// thaw/resume any operations using delta time
    /// </summary>
    public void ThawState()
    {
        Time.timeScale = 1f;
        StatePaused = false;
    }

    /// <summary>
    /// toggle any operations using delta time
    /// </summary>
    public void ToggleState()
    {
        if (StatePaused)
            ThawState();
        else
           FreezeState();
    }

    /// <summary>
    /// pauses game by freezing state and showing pause menu
    /// </summary>
    public void Pause()
    {
        Debug.Log("pausing game!");
        m_pauseMenuDocument.rootVisualElement.RemoveFromClassList("hidden");
        FreezeState();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GamePaused = true;
    }

    /// <summary>
    /// unpauses game by thawing/resuming state and hiding pause menu
    /// </summary>
    public void Unpause()
    {
        Debug.Log("unpausing game!");
        m_pauseMenuDocument.rootVisualElement.AddToClassList("hidden");
        ThawState();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GamePaused = false;
    }
    
    /// <summary>
    /// toggle game pausing by toggling state and pause menu
    /// </summary>
    public void TogglePause()
    {
        if (GamePaused)
            Unpause();
        else
            Pause();
    }
}
