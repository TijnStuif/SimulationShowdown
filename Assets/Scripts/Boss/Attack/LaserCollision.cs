using System;
using UnityEngine;

namespace Boss.Attack
{
    public class LaserCollision : DamageAttack
    {
        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "PlayerTag")
            {
                InvokePlayerDamaged(Laser.Damage);
            }
        }
    }
}