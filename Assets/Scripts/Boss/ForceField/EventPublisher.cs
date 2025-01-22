using System;
using UnityEngine;

namespace Boss.ForceField
{
    public enum State
    {
       Active = 1,
       Inactive = 0
    }
    public class EventPublisher : MonoBehaviour
    {
        public Action<State> StateChanged;

        private void OnEnable()
        {
            StateChanged?.Invoke(State.Active);
        }

        private void OnDisable()
        {
            StateChanged?.Invoke(State.Inactive);
        }
    }
}
