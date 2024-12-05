using System;
using System.Collections.Generic;
using Boss.Attack;
using UnityEngine;

namespace Boss
{
    public class PhaseController : MonoBehaviour
    {
        [SerializeField] private Controller bossController;
        public enum Phase{
            Phase1,
            Phase2,
            Phase3,
            Phase4
        }
        public Dictionary<Phase, List<IAttack>> phases;
        public Phase currentPhase;

        private void Awake()
        {
            phases = new Dictionary<Phase, List<IAttack>>{
                {Phase.Phase1, new List<IAttack>{new Laser(), new CloseRange()}},
                {Phase.Phase2, new List<IAttack>{new LowGravity(), new InvertedControls()}},
                {Phase.Phase3, new List<IAttack>{new SceneFlip()}},
                {Phase.Phase4, new List<IAttack>{}},
            };
            bossController.OnDamaged += UpdatePhase;
            currentPhase = Phase.Phase1;
        }

        private void UpdatePhase(int damage)
        {
            switch (currentPhase)
            {
                case Phase.Phase1:
                    currentPhase = Phase.Phase2;
                    Debug.Log(currentPhase);
                    break;
                case Phase.Phase2:
                    currentPhase = Phase.Phase3;
                    Debug.Log(currentPhase);
                    break;
                case Phase.Phase3:
                    currentPhase = Phase.Phase4;
                    Debug.Log(currentPhase);
                    break;
                case Phase.Phase4:
                Debug.Log(currentPhase);
                    break;
            }
        }
    }
}