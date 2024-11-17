using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PauseMenu
{
    public class Controller : MonoBehaviour
    {
        private VisualElement m_root;
        private Button m_resumeButton;
        // I don't know any solution other to replicate a constructor using
        // UnityEngine.Instantiate() instead
        // so I'm manually assigning it when instantiating.
        public StateController StateController { private get; set; }
        
        // get necessary UI components
        void Awake()
        {
            m_root = GetComponent<UIDocument>().rootVisualElement;
            m_resumeButton = m_root.Q<Button>("resume");
        }

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
            try
            {
                StateController.Unpause();
            }
            catch (NullReferenceException e)
            {
                Debug.Log($"Exception msg: {e.Message}");
                Debug.Log("Can't toggle pause game\nstate manager is not initialized!");
                throw;
            }
        }
    }
}
