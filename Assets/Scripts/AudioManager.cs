using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip mainMenuMusic;
    [SerializeField] AudioClip bossFightMusic;
    [SerializeField] AudioClip winScreenMusic;
    [SerializeField] AudioClip loseScreenMusic;
    [Header("SFX")]
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        musicSource.clip = mainMenuMusic;
        musicSource.Play();
    }
}
