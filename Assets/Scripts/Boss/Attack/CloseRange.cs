using UnityEngine;

namespace Boss.Attack
{
    public class CloseRange : MonoBehaviour, IAttack
    { 
        public Type Type => Type.Direct;
        
        [SerializeField] private GameObject CloseRangeAttackIndicatorPrefab;
        [SerializeField] private GameObject CloseRangeAttackObjectPrefab;
        
        private GameObject CloseRangeAttackIndicator;
        private GameObject CloseRangeAttackObject;
        
        private Vector3 CloseRangeAttackIndicatorPosition = new Vector3(0, 0, 5);
        private Vector3 CloseRangeAttackPosition = new Vector3(0, 5, 5);
        private Vector3 CloseRangeAttackOriginalPosition = new Vector3(0, 0, 30);

        private void Awake()
        {
            // instantiate attack objects
            CloseRangeAttackIndicator = Instantiate(CloseRangeAttackIndicatorPrefab);
            CloseRangeAttackObject = Instantiate(CloseRangeAttackObjectPrefab);
            // move them far away
            Reset();
        }

        public void Execute()
        {
            CloseRangeAttackIndicator.transform.position = CloseRangeAttackIndicatorPosition;
            Invoke(nameof(InitiateCloseRangeAttack), 2f);
            Invoke(nameof(Reset), 5f);
        }
        
        public void InitiateCloseRangeAttack()
        {
            CloseRangeAttackObject.transform.position = CloseRangeAttackPosition; 
        }

        private void Reset()
        {
            CloseRangeAttackIndicator.transform.position = CloseRangeAttackOriginalPosition;
            CloseRangeAttackObject.transform.position = CloseRangeAttackOriginalPosition;
        }
    }
}
