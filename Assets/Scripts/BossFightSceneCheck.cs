using UnityEngine;


public class BossFightSceneCheck : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.bossFightMusic);
    }
}
