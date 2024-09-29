using System;
using UnityEngine;

namespace Pickups.Base
{
    // wait this is pretty cool actually
    [CreateAssetMenu(fileName ="NewIntel", menuName ="Pickups/Intel")]
    public class Base : ScriptableObject
    {
        public string Id = Guid.NewGuid().ToString();
        public string Name;
        public string Description;
        public Sprite Icon;
    }
}
