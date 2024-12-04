using System;
using UnityEngine;

namespace Boss
{
    public class PhaseController : MonoBehaviour
    {
        [SerializeField] private Controller bossController;
        private int phaseThreshold = 25;
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
        }

        private void UpdatePhase()
        {
            if (bossController.currentHealth <= phaseThreshold)
            {
                switch (currentPhase)
                {
                    case Phase.One:
                        currentPhase = Phase.Two;
                        break;
                    case Phase.Two:
                        currentPhase = Phase.Three;
                        break;
                    case Phase.Three:
                        currentPhase = Phase.Four;
                        break;
                    case Phase.Four:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        
    }
}