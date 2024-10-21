using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace PauseMenu
{
    public class Controller : MonoBehaviour
    {
        private VisualElement m_root;
        private Button m_resumeButton;
        
        // get necessary ui components
        void Awake()
        {
            m_root = GetComponent<UIDocument>().rootVisualElement;
            m_resumeButton = m_root.Q<Button>("resume");
        }

        // register 
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
            Debug.Log("hey!\nlisten!");
            // disable pause menu
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            GameControllerScript.GamePaused = false;
            Time.timeScale = 1;
            m_root.AddToClassList("hidden");
        }
    }
}
