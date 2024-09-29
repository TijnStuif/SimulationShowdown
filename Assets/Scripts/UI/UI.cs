// bad code starts here (this shouldn't be in one file)

using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private bool m_menuActive;
        private Toggle m_fullscreen;
        private VisualElement m_menus;
        private VisualElement m_root;
        private VisualElement m_settings;
        private Button m_toContinueGame;
        private Button m_toInventory;
        private Button m_toSettings;

        // Start is called before the first frame update
        void Awake()
        {
            m_root = uiDocument.rootVisualElement;
            
            m_menus = m_root.Q<VisualElement>("Menus");
            m_toContinueGame = m_menus.Q<Button>("ToContinueGame");
            m_toInventory = m_menus.Q<Button>("ToInventory");
            m_toSettings = m_menus.Q<Button>("ToSettings");

            m_settings = m_menus.Q<VisualElement>("Settings");
            m_fullscreen = m_settings.Q<Toggle>("Fullscreen");
            m_fullscreen.value = Screen.fullScreen;

            m_settings.style.display = DisplayStyle.None;
            m_menus.style.display = DisplayStyle.None;
        }
        
        private void OnDisable()
        {
            PrototypeCat.PauseGame.GamePaused -= MenusToggle;
            m_toContinueGame.UnregisterCallback<ClickEvent>(OnContinueButtonClicked);
            m_toSettings.UnregisterCallback<ClickEvent>(OnSettingsButtonClicked);
            m_toInventory.UnregisterCallback<ClickEvent>(OnInventoryButtonClicked);
            m_fullscreen.UnregisterCallback<ClickEvent>(OnFullscreenToggled);
        }

        private void OnEnable()
        {
            PrototypeCat.PauseGame.GamePaused += MenusToggle;
            m_toContinueGame.RegisterCallback<ClickEvent>(OnContinueButtonClicked);
            m_toSettings.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
            m_toInventory.RegisterCallback<ClickEvent>(OnInventoryButtonClicked);
            m_fullscreen.RegisterCallback<ClickEvent>(OnFullscreenToggled);
        }

        private void MenusClose()
        {
            StateManager.ChangePauseState(false);
            m_menus.style.display = DisplayStyle.None;
            m_menuActive = false;
        }
        private void MenusOpen()
        { 
            StateManager.ChangePauseState(true);
            m_menus.style.display = DisplayStyle.Flex;
            m_menuActive = true;
        }
        private void MenusToggle()
        {
            StateManager.TogglePauseState();
            m_menus.style.display = m_menus.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
            m_menuActive = !m_menuActive;
        }
        
        private void OnContinueButtonClicked(ClickEvent e) => MenusClose();
        
        private void OnFullscreenToggled(ClickEvent e)
        {
            Debug.Log(Screen.fullScreen);
            Screen.fullScreen = !Screen.fullScreen;
            Debug.Log(Screen.fullScreen);
        }

        private void OnInventoryButtonClicked(ClickEvent e) => Debug.Log("inventory button clicked");

        private void OnSettingsButtonClicked(ClickEvent e) => ToggleSettings();
        
        private void ToggleSettings()
        { 
            m_settings.style.display = m_settings.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void Update()
        {
            // scuffed code to keep the checkmark/toggle
            if (m_menuActive)
                m_fullscreen.value = Screen.fullScreen;
        }
    }
}
