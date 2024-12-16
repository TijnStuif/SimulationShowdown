using UnityEngine;

public class MainMenuSceneCheck : MonoBehaviour
{
    private static AudioManager AudioManager => AudioManager.Instance;
    private void OnEnable()
    {
        AudioManager.PlayMusic(AudioManager.mainMenuMusic);
    }
}