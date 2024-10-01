using UnityEditor;
using UnityEngine;

namespace Pickups.ItemDrop
{
    public class DataHolder : MonoBehaviour
    {
        public Pickups.Information Information { get; set; }

        private void Start()
        {
            Information = Instantiate(ItemManager.Instance.GetRandomPickupInformation());
        }
    }
}
