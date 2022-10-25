using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    #region Static instance;
    private static AudioManager instance;
    public static AudioManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null) {
                    instance = new GameObject("Spawn AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }

            return instance;
        }
        private set {
            instance = value;
        }
    }



    internal void PlayMusic(string v, int v1) {
        throw new NotImplementedException();
    }
    #endregion

    #region Fields
    public AudioMixerGroup masterMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup deck1Mixer;
    public AudioMixerGroup deck2Mixer;
    public AudioMixerGroup sfxMixer;


    public AudioSource deck1Source;
    public AudioSource deck2Source;
    // two AudioSources created so that smooth transition between music can occur
    // one sfx general source for now?
    public AudioSource sfxSource;

    public float defaultMasterVolume = 0.3f;
    public float defaultMusicVolume = 0.3f;
    public float defaultEffectsVolume = 0.3f;

    public AudioClip[] musicClips;
    public AudioClip[] soundClips;

    public int currentTrack = 0;

    private bool firstMusicSourceIsPlaying;
    #endregion

    private void Awake() {
        // Make sure we don't destroy this instance
        //DontDestroyOnLoad(this.gameObject);

        // Create audio sources and save them as references
        deck1Source = this.gameObject.AddComponent<AudioSource>();
        deck1Source.outputAudioMixerGroup = deck1Mixer;
        deck2Source = this.gameObject.AddComponent<AudioSource>();
        deck2Source.outputAudioMixerGroup = deck2Mixer;
        sfxSource = this.gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = sfxMixer;



        // Loop music tracks
        deck1Source.loop = true;
        deck2Source.loop = true;
        //SetMusicVolume(defaultMusicVolume);

    }

    private void Start() {

        PlayMusic(musicClips[currentTrack]);

    }

    public void PlayTrack(int _number) {
        PlayMusic(musicClips[_number]);
    }

    public void PlayNext() {

        if(currentTrack == musicClips.Length) {
            currentTrack = 0;
        } else {
            currentTrack++;
        }

        PlayMusic(musicClips[currentTrack]);

    }

    public void PlayMusic(AudioClip musicClip) {
        // Determine which source is active
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? deck1Source : deck2Source;

        activeSource.clip = musicClip;
        activeSource.volume = defaultMusicVolume;
        //musicMixer.audioMixer.SetFloat("MusicVolume", defaultMusicVolume);
        activeSource.Play();
    }


    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1.0f) {
        // Determine which audio source is active
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? deck1Source : deck2Source;
        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }
    public void PlayMusicWithCrossFade(AudioClip musicClip, float transitionTime = 1.0f) {
        // Determine which audio source is active
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? deck1Source : deck2Source;
        AudioSource newSource = (firstMusicSourceIsPlaying) ? deck2Source : deck1Source;

        // Swap the source
        firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        // Set the fields of the audio source, then start the coroutine to crossfade
        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
    }

    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime) {
        // Make sure the source is active and playing 
        if (!activeSource.isActiveAndEnabled)
            activeSource.Play();

        float t = 0.0f;

        // Fade out
        for (t = 0; t < transitionTime; t += Time.deltaTime) {
            activeSource.volume = ((t / transitionTime));
            //musicMixer.audioMixer.SetFloat("MusicVolume", ((t / transitionTime)));

            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();
    }
    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime) {
        float t = 0.0f;

        // Fade out
        for (t = 0; t <= transitionTime; t += Time.deltaTime) {
            original.volume = (1 - (t / transitionTime));
            newSource.volume = ((t / transitionTime));
            yield return null;
        }

        original.Stop();
    }

    public void PlaySFX(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }
    public void PlaySFX(AudioClip clip, float volume) {
        sfxSource.PlayOneShot(clip, volume);

    }

    public void SetMusicVolume(float _volume) {
        //musicMixer.audioMixer.SetFloat("MusicVolume", _volume);
        deck1Source.volume = _volume;
        deck2Source.volume = _volume;
    }
    public void SetSFXVolume(float _volume) {
        //musicMixer.audioMixer.SetFloat("SFXVolume", _volume);
        sfxSource.volume = _volume;
    }
}