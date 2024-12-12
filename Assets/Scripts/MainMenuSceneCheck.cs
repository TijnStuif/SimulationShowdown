using UnityEngine;

public class MainMenuSceneCheck : MonoBehaviour
{
    private AudioManager audioManager;
    private void OnEnable()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlayMusic(audioManager.mainMenuMusic);
    }
}