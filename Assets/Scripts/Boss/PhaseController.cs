using System;
using System.Collections.Generic;
using Boss.Attack;
using UnityEngine;

namespace Boss
{
    public class PhaseController : MonoBehaviour
    {
        [SerializeField] private Controller bossController;
        private List<Phase> phases;
        private string currentPhase;

        private void Start()
        {
            phases = new List<Phase>{
                new Phase("Phase 1", new List<IAttack> {new Laser(), new CloseRange()}),
                new Phase("Phase 2", new List<IAttack> {new InvertedControls(), new LowGravity()}),
                new Phase("Phase 3", new List<IAttack> {new SceneFlip()}),
                new Phase("Phase 4", new List<IAttack> {})
            };
            bossController.OnDamaged += UpdatePhase;
            currentPhase = "Phase 1";
        }

        private void UpdatePhase(int damage)
        {
            switch (currentPhase)
            {
                case "Phase 1":
                    currentPhase = "Phase 2";
                    Debug.Log(currentPhase);
                    break;
                case "Phase 2":
                    currentPhase = "Phase 3";
                    Debug.Log(currentPhase);
                    break;
                case "Phase 3":
                    currentPhase = "Phase 4";
                    Debug.Log(currentPhase);
                    break;
                case "Phase 4":
                Debug.Log(currentPhase);
                    break;
            }
        } 
    }
}