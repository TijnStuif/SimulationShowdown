using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GameOverScreen
{
    public class Controller : MonoBehaviour
    {
        private UIDocument m_uiDocument;
        private Button m_restartButton;
        private Button m_titleScreenButton;

        private void Awake()
        {
            m_uiDocument = GetComponent<UIDocument>();
            m_restartButton = m_uiDocument.rootVisualElement.Q<Button>("restart");
            m_titleScreenButton = m_uiDocument.rootVisualElement.Q<Button>("title-screen");
        }

        private void OnEnable()
        {
            m_restartButton.RegisterCallback<ClickEvent>(OnButtonClicked);
            m_restartButton.RegisterCallback<ClickEvent>(OnRestartClicked);
            m_titleScreenButton.RegisterCallback<ClickEvent>(OnButtonClicked);
            m_titleScreenButton.RegisterCallback<ClickEvent>(OnTitleScreenClicked);
        }

        private void OnDisable()
        {
            m_restartButton.UnregisterCallback<ClickEvent>(OnButtonClicked);
            m_restartButton.UnregisterCallback<ClickEvent>(OnRestartClicked);
            m_titleScreenButton.UnregisterCallback<ClickEvent>(OnButtonClicked);
            m_titleScreenButton.UnregisterCallback<ClickEvent>(OnTitleScreenClicked);
        }

        private void OnButtonClicked(ClickEvent evt)
        {
            Time.timeScale = 1;
        }

        private void OnRestartClicked(ClickEvent evt)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }

        private void OnTitleScreenClicked(ClickEvent evt)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("StartMenu");
            Time.timeScale = 1;
        }
    }
}
