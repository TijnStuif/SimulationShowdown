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
        Phase currentPhase {get; set;}

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
                    break;
                case Phase.Two:
                    currentPhase = Phase.Three;
                    break;
                case Phase.Three:
                    currentPhase = Phase.Four;
                    break;
                case Phase.Four:
                    break;
            }
        } 
    }
}