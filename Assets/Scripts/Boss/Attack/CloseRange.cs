using UnityEngine;
using System;

namespace Boss.Attack
{
    public class CloseRange : DamageAttack, IAttack
    { 
        Player.V2.Controller player;
        public Type Type => Type.Direct;
        
        private GameObject CloseRangeAttackIndicator;
        private GameObject CloseRangeAttackObject;
        private GameObject boss;
        private Vector3 CloseRangeAttackIndicatorPosition = new Vector3(200, 0, 20);
        private Vector3 CloseRangeAttackPosition;
        private Vector3 CloseRangeAttackOriginalPosition = new Vector3(250, 0, 20);
        AudioManager audioManager;

        private void Start()
        {
            boss = FindObjectOfType<Boss.Controller>().gameObject;
            CloseRangeAttackPosition = boss.transform.position;
            // instantiate attack objects
            CloseRangeAttackIndicator = GameObject.Find("CloseRangeAttackIndicator");
            CloseRangeAttackObject = GameObject.Find("CloseRangeAttack");
            // move them far away
            Reset();
            audioManager = FindObjectOfType<AudioManager>();
        }

        public void Execute()
        {
            CloseRangeAttackPosition = boss.transform.position;
            CloseRangeAttackIndicator.transform.position = CloseRangeAttackPosition;
            audioManager.PlaySFX(audioManager.bossCloseRangeSFX);
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
