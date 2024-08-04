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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
}
