using System;
using UnityEngine;

namespace Pickups
{
    // wait this is pretty cool actually
    [CreateAssetMenu(fileName ="NewPickupInformation", menuName ="Pickups/Information")]
    public class Information : ScriptableObject
    {
        public string id = Guid.NewGuid().ToString();
        public string itemName;
        public string description;
        public Sprite icon;
        public int tileSize; // Almost feels like I'm coding match 3 again
        public PickupType type;
    }

    public enum PickupType
    {
        Intel,
        Money,
    }
}
