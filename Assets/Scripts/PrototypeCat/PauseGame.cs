using System;
using UnityEngine;

namespace PrototypeCat
{
    public class PauseGame : MonoBehaviour
    {
        public static event Action GamePaused;
        // invoke event if there are subscribers to it
        private void OnMenuSwitch() => GamePaused?.Invoke();

    }
}