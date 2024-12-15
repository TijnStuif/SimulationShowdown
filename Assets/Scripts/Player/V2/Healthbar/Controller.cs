using UnityEngine;
using UnityEngine.UIElements;

namespace Player.V2.Healthbar
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Player.V2.Controller m_controller;
        private VisualElement m_uiDocument;
        private VisualElement m_healthbar;
        private ProgressBar m_progressbar;
        private Label m_playertext;
    
        private void Awake()
        {
            // access the UI document
            m_uiDocument = GetComponent<UIDocument>().rootVisualElement;

            // access UI Elements
            m_healthbar = m_uiDocument.Q<VisualElement>("healthbar");
            m_progressbar = m_uiDocument.Q<ProgressBar>("healthbar");
            m_playertext = m_uiDocument.Q<Label>("Playertext");

            m_healthbar.style.width = 500;
            m_healthbar.style.left = -580;

            m_playertext.style.width = 500;
            m_playertext.style.top = 940;
            m_playertext.style.left = -580;

        }

        // this used to call GameObject.Find and GetComponent<T> every frame
        // please don't do that
        private void FixedUpdate()
        {
            m_progressbar.value = m_controller.currentHealth;
        }
    }
}
