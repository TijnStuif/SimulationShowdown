using System;
using UnityEngine.UIElements;

namespace Pickups
{
    [Serializable]
    public class Visual : VisualElement
    {
        // uh the guide made a private readonly Information field, except that it's never used. at all.
        // it's private and readonly, while nothing in this class uses it besides the constructor
        // which already has access to the Information object since it's a constructor parameter
        
        // guess we're doing constructors now
        public Visual(Information information)
        {
            name = information.itemName;
            // guide uses:
            // name = $"{information.itemName}";
            // but itemName already is a string so ?????
            style.backgroundImage = information.icon.texture;
            
            AddToClassList("visual-item");
            AddToClassList("no-display");
        }
    }
}
