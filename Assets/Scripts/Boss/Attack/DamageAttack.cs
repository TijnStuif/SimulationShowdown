using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Boss.Attack
{
    public abstract class DamageAttack : MonoBehaviour
    {
        public static event Action<int> PlayerDamaged;
        protected int Damage {get; set;}
        

        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("PlayerTag"))
            {
                PlayerDamaged?.Invoke(Damage);
            }
        }
    }
}
