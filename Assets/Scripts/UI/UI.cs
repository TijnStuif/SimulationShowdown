using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private Button m_continueGame;
        private Button m_inventory;
        private VisualElement m_menus;
        private VisualElement m_root;
        private Button m_settings;

        // Start is called before the first frame update
        void Awake()
        {
            m_root = uiDocument.rootVisualElement;
            m_menus = m_root.Q<VisualElement>("Menus");
            m_menus.style.display = DisplayStyle.None;
            m_continueGame = m_menus.Q<Button>("ContinueGame");
            m_inventory = m_menus.Q<Button>("Inventory");
            m_settings = m_menus.Q<Button>("Settings");
        }
        
        private void OnDisable()
        {
            PrototypeCat.PauseGame.GamePaused -= ToggleMenus;
            m_continueGame.UnregisterCallback<ClickEvent>(OnContinueButtonClicked);
        }

        private void OnEnable()
        {
            PrototypeCat.PauseGame.GamePaused += ToggleMenus;
            m_continueGame.RegisterCallback<ClickEvent>(OnContinueButtonClicked);
        }

        private void CloseMenus()
        {
            StateManager.ChangePauseState(false);
            m_menus.style.display = DisplayStyle.None;
        }
        
        private void OnContinueButtonClicked(ClickEvent e) => CloseMenus(); 
        
        private void OpenMenus()
        { 
            StateManager.ChangePauseState(true);
            m_menus.style.display = DisplayStyle.Flex;
        }
        
        private void ToggleMenus()
        {
            StateManager.TogglePauseState();
            m_menus.style.display = m_menus.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        }

    }
}
