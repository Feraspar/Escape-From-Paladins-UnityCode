using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private Sound[] musicTracks;
    private int currentTrackIndex = 0;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        musicTracks = Array.FindAll(sounds, sound => sound.isMusic);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        //Play("BackgroundMusic_1");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!musicTracks[currentTrackIndex].source.isPlaying && SceneManager.GetActiveScene().buildIndex != 0)
        {
            PlayNextMusicTrack();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
            Stop();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.trackName == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Play();
    }

    public void Stop()
    {
        foreach (Sound s in musicTracks)
        {
            s.source.Stop();
        }
    }

    public void PlayNextMusicTrack()
    {
        if (musicTracks.Length == 0)
        {
            Debug.LogWarning("No music tracks found.");
            return;
        }

        musicTracks[currentTrackIndex].source.Stop();

        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        musicTracks[currentTrackIndex].source.Play();
    }
}
