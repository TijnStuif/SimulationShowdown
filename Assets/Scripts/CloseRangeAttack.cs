using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CloseRangeAttack : MonoBehaviour, IAttack

{ 
    public AttackType Type => AttackType.Direct;
    [SerializeField]private GameObject CloseRangeAttackIndicator;
    [SerializeField]private GameObject CloseRangeAttackObject;

    //Different locations for the attack to move them in- and outside of the arena 
    private Vector3 CloseRangeAttackIndicatorPosition = new Vector3(0, 0, 5);
    private Vector3 CloseRangeAttackPosition = new Vector3(0, 5, 5);
    private Vector3 CloseRangeAttackOriginalPosition = new Vector3(0, 0, 30);

    //Execute runs once in a while
    //It is called by the IAttack interface and executes the close range attack
    //Invoke is used to delay the attack and the reset of the attack
    public void Execute()
    {
        CloseRangeAttackIndicator.transform.position = CloseRangeAttackIndicatorPosition;
        Invoke(nameof(InitiateCloseRangeAttack), 2f);
        Invoke(nameof(ResetAttackPositions), 5f);
    }

 

    public void InitiateCloseRangeAttack()
    {
        CloseRangeAttackObject.transform.position = CloseRangeAttackPosition; 
    }
    public void ResetAttackPositions()
    {
        CloseRangeAttackIndicator.transform.position = CloseRangeAttackOriginalPosition;
        CloseRangeAttackObject.transform.position = CloseRangeAttackOriginalPosition;
    }

    
}
