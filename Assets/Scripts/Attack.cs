using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IAttack
{
    public AttackType Type => AttackType.Environment;
    
    // the Execute method is called when the attack is executed
    public void Execute()
    {
        
    }

}
