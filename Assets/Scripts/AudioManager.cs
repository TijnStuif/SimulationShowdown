using System.Collections;
using System.Collections.Generic;
using Boss;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] AudioSource musicSource;
    public AudioClip mainMenuMusic;
    public AudioClip bossFightMusic;
    public AudioClip winScreenMusic;
    public AudioClip gameOverScreenMusic;
    [Header("SFX")]
    [SerializeField] AudioSource audioSource;
    public AudioClip buttonSFX;
    public AudioClip playerDamagedSFX;
    public AudioClip playerTeleportedSFX;
    public AudioClip[] bossDamagedSFX;
    public AudioClip bossLaserSFX;
    public AudioClip bossCloseRangeSFX;
    public AudioClip bossInputReverserSFX;
    public AudioClip bossSceneFlipSFX;
    public AudioClip bossLowGravitySFX;
    public AudioClip bossGlitchSFX;
    public AudioClip pickupSFX;

    public void PlayMusic(AudioClip musicFile)
    {
        StopCurrentMusic();
        musicSource.clip = musicFile;
        musicSource.Play();
    }

    public void DampenMusic()
    {
        musicSource.volume *= 0.5f;
    }

    public void UndampenMusic()
    {
        musicSource.volume *= 2;
    }

    void StopCurrentMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip sfxFile)
    {
        audioSource.PlayOneShot(sfxFile);
    }
}
