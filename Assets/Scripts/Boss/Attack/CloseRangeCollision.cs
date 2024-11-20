using System;
using UnityEngine;

namespace Boss.Attack
{
    public class CloseRangeCollision : DamageAttack
    {
        protected void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "PlayerTag")
            {
                InvokePlayerDamaged(CloseRange.Damage);
            }
        }
    }
}