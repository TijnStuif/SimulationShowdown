using UnityEngine;
using System;

namespace Boss.Attack
{
    public class CloseRange : DamageAttack, IAttack
    { 
        Player.Controller player;
        public Type Type => Type.Direct;
        
        [SerializeField] private GameObject CloseRangeAttackIndicatorPrefab;
        [SerializeField] private GameObject CloseRangeAttackObjectPrefab;
        
        private GameObject CloseRangeAttackIndicator;
        private GameObject CloseRangeAttackObject;
        private GameObject boss;
        private Vector3 CloseRangeAttackIndicatorPosition = new Vector3(200, 0, 20);
        private Vector3 CloseRangeAttackPosition;
        private Vector3 CloseRangeAttackOriginalPosition = new Vector3(250, 0, 20);

        private void Awake()
        {
            boss = FindObjectOfType<Boss.Controller>().gameObject;
            CloseRangeAttackPosition = boss.transform.position;
            // instantiate attack objects
            CloseRangeAttackIndicator = Instantiate(CloseRangeAttackIndicatorPrefab);
            CloseRangeAttackObject = Instantiate(CloseRangeAttackObjectPrefab);
            // move them far away
            Reset();
        }

        public void Execute()
        {
            CloseRangeAttackPosition = boss.transform.position;
            CloseRangeAttackIndicator.transform.position = CloseRangeAttackPosition;
            Invoke(nameof(InitiateCloseRangeAttack), 1f);
            Invoke(nameof(Reset), 2f);
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
