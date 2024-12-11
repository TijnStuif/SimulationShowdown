using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public static class Compatibility
{
    public static bool IsV1 => SceneManager.GetActiveScene().name == "GameSceneV1";
}
