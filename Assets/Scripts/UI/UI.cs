// bad code starts here (this shouldn't be in one file)

using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UI : MonoBehaviour
    {

        public bool BgmToggled => m_bgm.value;
        
        [SerializeField] private UIDocument uiDocument;

        private Toggle m_bgm;
        private VisualElement m_bodies;
        private VisualElement m_inventory;
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
            // parents that own a lot
            m_root = uiDocument.rootVisualElement;
            m_menus = m_root.Q<VisualElement>("Menus");
            m_bodies = m_menus.Q<VisualElement>("Bodies");
            
            // menu buttons
            m_toContinueGame = m_menus.Q<Button>("ToContinueGame");
            m_toInventory = m_menus.Q<Button>("ToInventory");
            m_toSettings = m_menus.Q<Button>("ToSettings");

            
            // settings
            m_settings = m_bodies.Q<VisualElement>("Settings");
            m_bgm = m_settings.Q<Toggle>("BGM");
            m_fullscreen = m_settings.Q<Toggle>("Fullscreen");
            
            // inventory
            m_inventory = m_bodies.Q<VisualElement>("Inventory");
        }
        
        private void OnDisable()
        {
            PrototypeCat.PauseGame.GamePaused -= MenusToggle;
            
            m_toContinueGame.UnregisterCallback<ClickEvent>(OnContinueButtonClicked);
            m_toSettings.UnregisterCallback<ClickEvent>(OnSettingsButtonClicked);
            m_toInventory.UnregisterCallback<ClickEvent>(OnInventoryButtonClicked);
            
            m_fullscreen.UnregisterCallback<ClickEvent>(OnFullscreenToggled);
            m_bgm.UnregisterCallback<ClickEvent>(OnBGMToggled);
        }

        private void OnEnable()
        {
            PrototypeCat.PauseGame.GamePaused += MenusToggle;
            
            m_toContinueGame.RegisterCallback<ClickEvent>(OnContinueButtonClicked);
            m_toSettings.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
            m_toInventory.RegisterCallback<ClickEvent>(OnInventoryButtonClicked);
            
            m_fullscreen.RegisterCallback<ClickEvent>(OnFullscreenToggled);
            m_bgm.RegisterCallback<ClickEvent>(OnBGMToggled);
        }

        private void MenusClose()
        {
            StateManager.ChangePauseState(false);
            m_menus.AddToClassList("no-display");
            m_menuActive = false;
        }
        private void MenusOpen()
        { 
            StateManager.ChangePauseState(true);
            m_menus.RemoveFromClassList("no-display");
            m_menuActive = true;
        }

        private void MenusToggle()
        {
            StateManager.TogglePauseState();
            m_menus.ToggleInClassList("no-display");
            m_menuActive = !m_menuActive;
        }

        private static void OnBGMToggled(ClickEvent e)
        {
           Debug.Log("BGM has been toggled"); 
        }
        
        private void OnContinueButtonClicked(ClickEvent e) => MenusClose();
        
        private static void OnFullscreenToggled(ClickEvent e) => Screen.fullScreen = !Screen.fullScreen;

        private void OnInventoryButtonClicked(ClickEvent e) => m_inventory.ToggleInClassList("no-display");


        private void OnSettingsButtonClicked(ClickEvent e) => m_settings.ToggleInClassList("no-display");

        private void Update()
        {
            // scuffed code to keep the checkmark/toggle
            if (m_menuActive)
                m_fullscreen.value = Screen.fullScreen;
        }
    }
}
