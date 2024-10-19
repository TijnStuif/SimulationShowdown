using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    // soort attack
    public enum AttackType
    {
       Environment,
       direct,
    }

    AttackType Type { get; }
    
    // the Execute method is called when the attack is executed
    void Execute();
}
