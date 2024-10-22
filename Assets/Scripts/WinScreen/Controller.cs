using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace WinScreen
{
    public class Controller : MonoBehaviour
    {
        private UIDocument m_uiDocument;
        private Button m_restartButton;
        private Button m_titleButton;
        
        void Awake()
        {
            m_uiDocument = GetComponent<UIDocument>();
            m_restartButton = m_uiDocument.rootVisualElement.Q<Button>("restart");
            m_titleButton = m_uiDocument.rootVisualElement.Q<Button>("title-screen");
        }

        void OnEnable()
        {
            m_restartButton.RegisterCallback<ClickEvent>(OnButtonClicked);
            m_titleButton.RegisterCallback<ClickEvent>(OnButtonClicked);
            m_restartButton.RegisterCallback<ClickEvent>(OnRestartClicked);
            m_titleButton.RegisterCallback<ClickEvent>(OnTitleClicked);
        }

        void OnDisable()
        {
            m_restartButton.UnregisterCallback<ClickEvent>(OnButtonClicked);
            m_titleButton.UnregisterCallback<ClickEvent>(OnButtonClicked);
            m_restartButton.UnregisterCallback<ClickEvent>(OnRestartClicked);
            m_titleButton.UnregisterCallback<ClickEvent>(OnTitleClicked);
        }

        // for when any button is clicked (it unpauses the game basically)
        private void OnButtonClicked(ClickEvent evt)
        {
            Time.timeScale = 1;
        }

        private void OnRestartClicked(ClickEvent evt)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnTitleClicked(ClickEvent evt)
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}
