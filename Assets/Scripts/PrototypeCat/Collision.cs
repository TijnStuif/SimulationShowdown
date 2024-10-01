using System;
using Pickups.ItemDrop;
using UnityEngine;

namespace PrototypeCat
{
    public class Collision : MonoBehaviour
    {
        public static event EventHandler<Pickups.Information> ItemPickedUp;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyHitbox"))
            {
                Debug.Log("Ow");
            }
            if (other.CompareTag("Pickup"))
            {
                var dataHolder = other.gameObject.GetComponentInParent<DataHolder>();
                ItemPickedUp?.Invoke(this, dataHolder.Information);
                Destroy(other.gameObject);
            }
        }
    }
}
