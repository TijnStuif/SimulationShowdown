using System.Collections;
using UnityEngine;

namespace Boss.Attack
{
    public class LowGravity : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

        [SerializeField] private float gravityAmount = -1.6f;
        
        public void Execute()
        {
            if (Physics.gravity.y == gravityAmount)
            {
                Physics.gravity = new Vector3(0, -9.81f, 0);
                return;
            } else {
                Physics.gravity = new Vector3(0, gravityAmount, 0);
            }
            
        }
    }
}
