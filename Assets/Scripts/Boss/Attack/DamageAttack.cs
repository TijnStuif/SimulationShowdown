using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Boss.Attack
{
    public abstract class DamageAttack : MonoBehaviour
    {
        public static event Action<int> PlayerDamaged;
        protected int Damage { get; set; }

        protected void InvokePlayerDamaged(int damage)
        {
            PlayerDamaged?.Invoke(damage);
        }

        protected abstract void OnTriggerEnter(Collider other);
    }
}
