using UnityEngine;

namespace PrototypeCat
{
    public class PauseGame : MonoBehaviour
    {
        private void OnMenuSwitch()
        {
            // branchless programming lol
            // explanation:
            // -(1 - 1) = -0 = 0
            // -(0 - 1) = 1
            // so I sorta inverted 1 to 0 and 0 to 1 with math that coincidentally works
            Time.timeScale = -(Time.timeScale - 1);
        }
    }
}
