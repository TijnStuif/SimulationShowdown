using UnityEngine;


public class BossFightSceneCheck : MonoBehaviour
{
    private AudioManager audioManager;
    private void OnEnable()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlayMusic(audioManager.bossFightMusic);
    }
}
