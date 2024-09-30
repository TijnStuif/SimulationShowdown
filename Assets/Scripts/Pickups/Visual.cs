using System;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pickups
{
    [Serializable]
    public class Visual : VisualElement
    {
        // I figured out why they put this field in the guide
        // (see UpdateInventory)
        private readonly Information m_information;
        
        // guess we're doing constructors now
        public Visual(Information information)
        {
            m_information = information;
            name = information.itemName;
            // guide uses:
            // name = $"{information.itemName}";
            // but itemName already is a string so ?????
            style.backgroundImage = information.icon.texture;
            
            AddToClassList("visual-item");
            AddToClassList("no-display");
            RegisterCallback<ClickEvent>(UpdateInventory);
        }

        // guess we're doing destructors now
        ~Visual()
        {
           UnregisterCallback<ClickEvent>(UpdateInventory); 
        }

        public void UpdateInventory(ClickEvent e)
        { 
            Inventory.Instance.UpdateUiInfo(m_information);
        }
    }
}
