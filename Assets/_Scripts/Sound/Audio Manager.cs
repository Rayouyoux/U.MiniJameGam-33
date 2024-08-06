using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] MusicSounds, SFXSounds;
    public AudioSource MusicSource, SFXSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(MusicSounds, music => music.Name == name);

        if (sound == null) return;

        MusicSource.clip = sound.Clip;
        MusicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(SFXSounds, sfx => sfx.Name == name);

        if (sound == null) return;

        SFXSource.PlayOneShot(sound.Clip);
    }

    public void SetMusicVolume(float volume)
    {
        MusicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}
