using UnityEngine;
using UnityEngine.SceneManagement;

namespace PrototypeCat
{
    public class ReloadScene : MonoBehaviour
    {
        private void OnReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
