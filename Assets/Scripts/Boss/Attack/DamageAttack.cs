using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Boss.Attack
{
    public abstract class DamageAttack : MonoBehaviour
    {
        public static event Action<int> PlayerDamaged;
        protected static int Damage { get; set; }

        public void InvokePlayerDamaged(int damage)
        {
            PlayerDamaged?.Invoke(damage);
        }
    }
}
