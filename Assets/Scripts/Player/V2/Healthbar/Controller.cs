using UnityEngine;
using UnityEngine.UIElements;

namespace Player.V2.Healthbar
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Player.V2.Controller m_controller;
        private VisualElement m_uiDocument;
        private VisualElement healthbar;
        private ProgressBar progressbar;
        private Label playertext;
    
        private void Awake()
        {
            // access the UI document
            m_uiDocument = GetComponent<UIDocument>().rootVisualElement;

            // access UI Elements
            healthbar = m_uiDocument.Q<VisualElement>("healthbar");
            progressbar = m_uiDocument.Q<ProgressBar>("healthbar");
            playertext = m_uiDocument.Q<Label>("Playertext");

            healthbar.style.width = 500;
            healthbar.style.left = -580;

            playertext.style.width = 500;
            playertext.style.top = 940;
            playertext.style.left = -580;

        }

        // this used to call GameObject.Find and GetComponent<T> every frame
        // please don't do that
        private void FixedUpdate()
        {
            progressbar.value = m_controller.currentHealth;
        }
    }
}
