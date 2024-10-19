using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
    {
       Environment,
       Direct,
    }

public interface IAttack
{
    AttackType Type { get; }

    // the Execute method is called when the attack is executed
    void Execute();
}
