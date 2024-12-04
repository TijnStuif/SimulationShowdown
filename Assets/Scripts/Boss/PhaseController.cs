using System;
using UnityEngine;

namespace Boss
{
    public class PhaseController : MonoBehaviour
    {
        [SerializeField] private Controller bossController;
        private enum Phase { 
            One, 
            Two, 
            Three, 
            Four 
        }
        private Phase currentPhase;

        private void Start()
        {
            currentPhase = Phase.One;
            bossController.OnDamaged += UpdatePhase;
        }

        private void UpdatePhase(int damage)
        {
            switch (currentPhase)
            {
                case Phase.One:
                    currentPhase = Phase.Two;
                    Debug.Log("Phase Two");
                    break;
                case Phase.Two:
                    currentPhase = Phase.Three;
                    Debug.Log("Phase Three");
                    break;
                case Phase.Three:
                    currentPhase = Phase.Four;
                    Debug.Log("Phase Four");
                    break;
                case Phase.Four:
                    Debug.Log("Boss defeated");
                    break;
            }
        } 
    }
}