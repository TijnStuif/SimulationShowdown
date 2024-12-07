using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Player
{
    public class HealthBar : MonoBehaviour
    {
        VisualElement m_uiDocument;
        public Transform player;
        VisualElement healthbar;
        ProgressBar progressbar;
        Label playertext;
        private Camera cam;
        public UIDocument healthbarDocument;
    
        private void Awake()
        {
            cam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
            
            //acccess the UI document
            m_uiDocument = GetComponent<UIDocument>().rootVisualElement;

            //access the healthbar VisualElement and the progressBar
            healthbar = m_uiDocument.Q<VisualElement>("healthbar");
            progressbar = m_uiDocument.Q<ProgressBar>("healthbar");
            playertext = m_uiDocument.Q<Label>("Playertext");

            m_uiDocument.Add(healthbar);

            healthbar.style.width = 500;
            healthbar.style.left = -580;

            playertext.style.width = 500;
            playertext.style.top = 940;
            playertext.style.left = -580;

        }

        private void Update()
        {
            //Access the health of the player from the player script
            int health = GameObject.Find("Player").GetComponent<Player.Controller>().currentHealth;

            progressbar.value = health;
        }
    }
}
