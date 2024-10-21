using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace StartMenu
{
    public class Controller : MonoBehaviour
    {
        private VisualElement m_root;
        private Button m_startButton;
        
        // get necessary ui components
        void Awake()
        {
            m_root = GetComponent<UIDocument>().rootVisualElement;
            m_startButton = m_root.Q<Button>("start");
        }

        // register 
        private void OnEnable()
        {
            m_startButton.RegisterCallback<ClickEvent>(OnStartClicked);
        }

        private void OnDisable()
        {
            m_startButton.UnregisterCallback<ClickEvent>(OnStartClicked);
        }

        private void OnStartClicked(ClickEvent evt)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}