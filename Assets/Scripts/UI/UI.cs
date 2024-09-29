// bad code starts here (this shouldn't be in one file)

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private DropdownField m_fullscreen;
        private VisualElement m_menus;
        private VisualElement m_root;
        private VisualElement m_settings;
        private Button m_toContinueGame;
        private Button m_toInventory;
        private Button m_toSettings;

        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log(UnityEngine.Device.Screen.fullScreenMode);
            Debug.Log(Screen.fullScreenMode);
            m_root = uiDocument.rootVisualElement;
            
            m_menus = m_root.Q<VisualElement>("Menus");
            m_toContinueGame = m_menus.Q<Button>("ToContinueGame");
            m_toInventory = m_menus.Q<Button>("ToInventory");
            m_toSettings = m_menus.Q<Button>("ToSettings");

            m_settings = m_menus.Q<VisualElement>("Settings");
            m_fullscreen = m_settings.Q<DropdownField>("Fullscreen");

            m_settings.style.display = DisplayStyle.None;
            m_menus.style.display = DisplayStyle.None;
            
            InitFullscreenDropdown();
        }
        
        private void OnDisable()
        {
            PrototypeCat.PauseGame.GamePaused -= ToggleMenus;
            m_toContinueGame.UnregisterCallback<ClickEvent>(OnContinueButtonClicked);
            m_toSettings.UnregisterCallback<ClickEvent>(OnSettingsButtonClicked);
            m_toInventory.UnregisterCallback<ClickEvent>(OnInventoryButtonClicked);
            // m_fullscreen.UnregisterCallback<ClickEvent>(OnFullscreenToggled);
            m_fullscreen.UnregisterValueChangedCallback(OnFullscreenChanged);
        }

        private void OnEnable()
        {
            PrototypeCat.PauseGame.GamePaused += ToggleMenus;
            m_toContinueGame.RegisterCallback<ClickEvent>(OnContinueButtonClicked);
            m_toSettings.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
            m_toInventory.RegisterCallback<ClickEvent>(OnInventoryButtonClicked);
            m_fullscreen.RegisterValueChangedCallback(OnFullscreenChanged);
        }

        private void OnFullscreenChanged(ChangeEvent<string> change)
        {
            Debug.Log($"old: {Screen.fullScreenMode}");
            // parse string to the FullScreenMode enum, if successful, change fullscreen mode
            // if(Enum.TryParse(change.newValue, out FullScreenMode newEnum))
            //     Screen.fullScreenMode = newEnum;
            // else
            // {
            //     throw new Exception("parse failed ):");
            // }
            Screen.fullScreenMode = (FullScreenMode)m_fullscreen.index;
            // Debug.Log($"new value: {change.newValue}");
            // Debug.Log($"new enum: {newEnum}");
            Debug.Log($"index: {m_fullscreen.index}");
            Debug.Log($"new enum: {(FullScreenMode)m_fullscreen.index}");
            Debug.Log($"new: {Screen.fullScreenMode}");
        }

        private void CloseMenus()
        {
            StateManager.ChangePauseState(false);
            m_menus.style.display = DisplayStyle.None;
        }

        private void InitFullscreenDropdown()
        {
            Func<Type, List<string>> enumToStringList = type => Enum.GetNames(type).ToList();
            m_fullscreen.choices = enumToStringList(typeof(FullScreenMode));
            m_fullscreen.index = Convert.ToInt32(Screen.fullScreenMode);
        }
        
        private void OnContinueButtonClicked(ClickEvent e) => CloseMenus();
        
        private void OnInventoryButtonClicked(ClickEvent e) => Debug.Log("inventory button clicked");

        private void OnSettingsButtonClicked(ClickEvent e) => ToggleSettings();
        
        private void OpenMenus()
        { 
            StateManager.ChangePauseState(true);
            m_menus.style.display = DisplayStyle.Flex;
        }
        
        private void ToggleSettings()
        { 
            m_settings.style.display = m_settings.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        private void ToggleMenus()
        {
            StateManager.TogglePauseState();
            m_menus.style.display = m_menus.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        }

    }
}
