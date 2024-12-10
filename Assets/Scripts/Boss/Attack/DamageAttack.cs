using System;
using UnityEngine;

namespace Boss.Attack
{
    public class DamageAttack : MonoBehaviour
    {
        
        public static event Action<int> PlayerDamaged;
        //add damage via the inspector
        [SerializeField] private int damage;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerDamaged?.Invoke(damage);
            }
        }
    }
}