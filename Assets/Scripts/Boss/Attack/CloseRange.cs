using UnityEngine;

namespace Boss.Attack
{
    public class CloseRange : MonoBehaviour, IAttack

    { 
        public Type Type => Type.Direct;
        [SerializeField]private GameObject CloseRangeAttackIndicator;
        [SerializeField]private GameObject CloseRangeAttackObject;
        private Vector3 CloseRangeAttackIndicatorPosition = new Vector3(0, 0, 5);
        private Vector3 CloseRangeAttackPosition = new Vector3(0, 5, 5);
        private Vector3 CloseRangeAttackOriginalPosition = new Vector3(0, 0, 30);
        private int timer;
        public void Execute()
        {
            CloseRangeAttackIndicator.transform.position = CloseRangeAttackIndicatorPosition;
            Invoke(nameof(InitiateCloseRangeAttack), 2f);
            Invoke(nameof(ResetAttackPositions), 5f);
            Debug.Log("Attack");
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
}
